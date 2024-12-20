using Olyd.Extensions;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Windows;

namespace Olyd.Wpf.SingleStartup
{
    /// <summary>
    /// Singleton process mode, ensuring that only one instance of the application is running at any time.
    /// If the current process is already running, subsequent processes will act as clients, connecting to the server and passing startup arguments.
    /// </summary>
    public class SingletonStartUp
    {
        #region [[ Inner DataStructure ]]

        /// <summary>
        /// Server connection status
        /// </summary>
        private enum PipeStatus
        {
            /// <summary>
            /// Waiting for connection
            /// </summary>
            WaitForConnect,

            /// <summary>
            /// Reading message
            /// </summary>
            ReadMsg,
        }

        /// <summary>
        /// Process running role
        /// </summary>
        private enum RunRole
        {
            /// <summary>
            /// Not yet started or initialized
            /// </summary>
            None,

            /// <summary>
            /// Running as the server role
            /// </summary>
            RunInServer,

            /// <summary>
            /// Running as the client role
            /// </summary>
            RunInClient,
        }

        #endregion

        #region [[ Constructors ]]

        /// <summary>
        /// Constructor to initialize the SingletonStartUp instance.
        /// </summary>
        public SingletonStartUp(ISingleApp app)
        {
            App = app;
        }

        #endregion

        /// <summary>
        /// The actual application object
        /// </summary>
        public ISingleApp App { get; private set; }

        /// <summary>
        /// Flag indicating if the application is exiting
        /// </summary>
        public bool InExiting { get; set; }

        private readonly object _lock = new();
        private RunRole _runRole = RunRole.None; // The role of the current process
        private Thread _parseArgsThread; // Thread for parsing startup arguments
        private Thread _runInServerThread; // Thread for the server to listen for pipe connections
        private NamedPipeServerStream _pipeServer; // Server side pipe stream
        private readonly AutoResetEvent _parseArgsAuto = new(false); // Event to signal the argument parsing thread
        private readonly ConcurrentQueue<string> _nextRunArgsList = new(); // Queue for the next arguments to be processed

        #region Skeleton Methods

        /// <summary>
        /// Run the process and start relevant services
        /// </summary>
        public async Task Run(string[] args)
        {
            _pipeServer = null;

            // 1. Try to establish a named pipe server (if the server is not started, subsequent processes act as clients and connect)
            // 2. If the pipe connection fails, act as a client and send startup arguments to the server
            // 3. Retry until successful connection or run as client
            do
            {
                try
                {
                    var pipeName = GetApplicationInstanceID();

                    // Create a pipe with access control
                    var ps = new PipeSecurity();
                    ps.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));

                    // Create a named pipe server
                    _pipeServer = NamedPipeServerStreamAcl.Create(pipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 0, 0, ps);

                    _runRole = RunRole.RunInServer;

                    InitThread();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    // If the server is already running, try to act as a client
                    _runRole = await RunInClient(args);
                }
            } while (_runRole == RunRole.None);

            // If the role is client, exit the application
            if (_runRole == RunRole.RunInClient)
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Initialize argument parsing thread and pipe listening thread
        /// </summary>
        private void InitThread()
        {
            _parseArgsThread = new Thread(new ThreadStart(ParseArgs))
            {
                IsBackground = true,
                Name = "ParseArgs",
            };
            _parseArgsThread.Start();

            _runInServerThread = new Thread(new ThreadStart(RunInServer))
            {
                IsBackground = true,
                Name = "RunInServer",
            };
            _runInServerThread.Start();
        }

        /// <summary>
        /// Server thread that waits for client connections and handles messages
        /// </summary>
        private async void RunInServer()
        {
            while (!InExiting)
            {
                try
                {
                    _pipeServer.WaitForConnection(); // Wait for client connection

                    // After connection, read the message from the client
                    const int bufferLength = 1024;
                    var buffer = new byte[bufferLength];
                    using (var stream = new MemoryStream())
                    {
                        while (true)
                        {
                            var bytesRead = await _pipeServer.ReadAsync(buffer.AsMemory(0, bufferLength)).ConfigureAwait(false);

                            if (bytesRead == 0) { break; }
                            stream.Write(buffer, 0, bytesRead);
                        }

                        stream.Seek(0, SeekOrigin.Begin);

                        var msg = Encoding.UTF8.GetString(stream.ToArray());
                        _nextRunArgsList.Enqueue(msg); // Add the message to the argument queue
                        _parseArgsAuto.Set(); // Signal the parsing thread to process the arguments
                    }

                    _pipeServer.Disconnect(); // Disconnect from the client

                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort(); // Reset abort and terminate the current thread
                }
                catch (Exception)
                {
                    // If there is an error in the connection or pipe, restart the server
                    OnExit();
                    Environment.Exit(0);
                }
            }

            if (InExiting)
            {
                OnExit();
            }
        }

        /// <summary>
        /// Parse the startup arguments received from the client
        /// </summary>
        private void ParseArgs()
        {
            while (true)
            {
                if (_nextRunArgsList.IsEmpty)
                {
                    _parseArgsAuto.WaitOne(); // Wait for startup arguments
                }

                if (InExiting) { return; }

                if (_nextRunArgsList.TryDequeue(out string msg))
                {
                    ParseMsgAndNextRun(msg); // Parse and start the next process
                }
            }
        }

        /// <summary>
        /// Connect as a client and pass startup arguments to the server
        /// </summary>
        private async Task<RunRole> RunInClient(string[] args)
        {
            RunRole runRole;
            try
            {
                var pipeClient = new NamedPipeClientStream(
                     serverName: ".",
                     pipeName: GetApplicationInstanceID(),
                     direction: PipeDirection.Out,
                     options: PipeOptions.Asynchronous);

                pipeClient.Connect(10000); // Wait for 10 seconds to connect

                // Convert the startup arguments to byte array and write to the pipe
                var data = Encoding.UTF8.GetBytes(PackageArgs(args));
                await pipeClient.WriteAsync(data).ConfigureAwait(false);
                runRole = RunRole.RunInClient;

                pipeClient.Close();
                pipeClient = null;
            }
            catch (Exception)
            {
                runRole = RunRole.None;
            }

            return runRole;
        }

        /// <summary>
        /// Start another process and pass the startup arguments
        /// </summary>
        private void OnNextStartUp(string[] args)
        {
            if (App == null)
            {
                return;
            }

            lock (_lock)
            {
                try
                {
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate { App.NextRun(args); });
                }
                catch (Exception)
                {
                }
            }
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Exits the application and performs necessary cleanup.
        /// </summary>
        private void OnExit()
        {
            // Check if the pipe server exists before attempting to close it.
            if (_pipeServer != null)
            {
                _pipeServer.Close();  // Close the pipe server.
                _pipeServer = null;   // Nullify the reference to ensure proper cleanup.
            }
        }

        /// <summary>
        /// Parses the startup arguments sent from another process, handling cases where arguments are enclosed in double quotes.
        /// </summary>
        /// <param name="msg">The raw startup arguments as a string.</param>
        private async void ParseMsgAndNextRun(string msg)
        {
            string[] args = Array.Empty<string>(); // Default to an empty argument array.
            var splitChar = new char[] { ' ' }; // Define the space character as the split delimiter.

            // Proceed if the message is not null or whitespace.
            if (!string.IsNullOrWhiteSpace(msg))
            {
                // If the message contains valid quotation marks, parse it differently.
                if (GetQuationIndex(msg, out Tuple<int, int> indexes))
                {
                    var list = new List<string>(); // List to hold parsed arguments.
                    while (true)
                    {
                        if (indexes.Item1 > 0) // If there is text before the first quotation mark, split it by space.
                        {
                            list.AddRange(msg[..indexes.Item1].Split(splitChar, StringSplitOptions.RemoveEmptyEntries));
                        }

                        // Extract the argument between the two quotes and add it to the list.
                        list.Add(msg.Substring(indexes.Item1 + 1, indexes.Item2 - indexes.Item1 - 1));

                        // If the second quote is at the end of the string, stop parsing.
                        if (indexes.Item2 == msg.Length - 1)
                        {
                            // msg 已经解析完成
                            break;
                        }

                        // Continue parsing the remaining string after the second quote.
                        msg = msg[(indexes.Item2 + 1)..];

                        // Check for the next valid quotation pair in the remaining string.
                        if (!GetQuationIndex(msg, out indexes))
                        {
                            // If no more quotes are found, split the remaining message by spaces and finish parsing.
                            list.AddRange(msg.Split(splitChar, StringSplitOptions.RemoveEmptyEntries));
                            break;
                        }
                    }
                    args = list.ToArray(); // Convert the list to an array.
                }
                else
                {
                    // If no valid quotes are found, split the string by spaces.
                    args = msg.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                }
            }

            // Execute the startup logic in a separate thread.
            await Task.Run(() => { OnNextStartUp(args); });
        }

        /// <summary>
        /// Retrieves a unique identifier for the application instance.
        /// </summary>
        /// <returns>The application assembly name as a unique instance ID.</returns>
        private string GetApplicationInstanceID()
        {
            // If the InstanceID is not already set, retrieve the assembly name to generate a unique ID.
            if (App.InstanceID.IsNullOrWhiteSpace())
            {
                var assembly = Assembly.GetEntryAssembly();
                App.InstanceID = assembly.GetName().Name;
            }

            return App.InstanceID;
        }

        private static string PackageArgs(IEnumerable<string> args)
        {
            if (args == null || !args.Any()) { return ""; }

            var sb = new StringBuilder();
            foreach (var arg in args)
            {
                if (arg.Contains(' ') || string.IsNullOrWhiteSpace(arg))
                {
                    sb.Append($"\"{arg}\"");
                }
                else
                {
                    sb.Append(arg);
                }
                sb.Append(' ');
            }
            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// Retrieves the positions of two valid double quotation marks (") in the input string.
        /// </summary>
        /// <param name="msg">Input string to search for valid quotation marks.</param>
        /// <param name="indexes">Out parameter to hold the positions of the two double quotes.</param>
        /// <returns>Returns true if two valid double quotes are found in the string; otherwise, false.</returns>
        private static bool GetQuationIndex(string msg, out Tuple<int, int> indexes)
        {
            indexes = new Tuple<int, int>(-1, -1); // Initialize output to invalid positions.

            // Return false if the string is null, empty, or contains only white space.
            if (string.IsNullOrWhiteSpace(msg))
            {
                return false;
            }

            var quotationChar = '"'; // Define the quotation mark character.
            var firstIndex = msg.IndexOf(quotationChar); // Find the first double quote.

            // If no double quote is found, return false.
            if (firstIndex < 0)
            {
                return false;
            }

            // If the first double quote is the last character, it's an invalid case (only one quote).
            if (firstIndex == msg.Length - 1)
            {
                return false;
            }

            var secondIndex = msg.IndexOf(quotationChar, firstIndex + 1); // Find the second double quote.

            // If the second double quote is not found, return false (only one quote).
            if (secondIndex < 0)
            {
                return false;
            }

            // If two valid double quotes are found, return their positions.
            indexes = new Tuple<int, int>(firstIndex, secondIndex);
            return true;
        }


        #endregion

    }
}
