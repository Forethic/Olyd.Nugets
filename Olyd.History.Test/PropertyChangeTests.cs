using System.ComponentModel;

namespace Olyd.History.Test
{
    [TestFixture]
    public class PropertyChangeTests
    {
        internal class TestClass : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private string _name;
            public string Name
            {
                get => _name;
                set
                {
                    if (_name != value)
                    {
                        _name = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                    }
                }
            }
        }

        [Test]
        public void Redo_SetsPropertyToNewValue()
        {
            // Arrange
            var target = new TestClass() { Name = "OldValue" };
            var propertyChange = new PropertyChange(target, nameof(TestClass.Name), "OldValue", "NewValue");

            // Act
            propertyChange.Redo();

            // Assert
            Assert.That(target.Name, Is.EqualTo("NewValue"));
        }

        [Test]
        public void Undo_RevertsPropertyToOldValue()
        {
            // Arrange
            var target = new TestClass() { Name = "NewValue" };
            var propertyChange = new PropertyChange(target, nameof(TestClass.Name), "OldValue", "NewValue");

            // Act
            propertyChange.Undo();

            // Assert
            Assert.That(target.Name, Is.EqualTo("OldValue"));
        }

        [Test]
        public void Redo_TriggersPropertyChangedEvent()
        {
            // Arrange
            var target = new TestClass() { Name = "OldValue" };
            var propertyChange = new PropertyChange(target, nameof(TestClass.Name), "OldValue", "NewValue");

            string changedPropertyName = null;
            target.PropertyChanged += (sender, args) => changedPropertyName = args.PropertyName;

            // Act
            propertyChange.Redo();

            // Assert
            Assert.That(changedPropertyName, Is.EqualTo(nameof(TestClass.Name)));
        }

        [Test]
        public void Uedo_TriggersPropertyChangedEvent()
        {
            // Arrange
            var target = new TestClass() { Name = "NewValue" };
            var propertyChange = new PropertyChange(target, nameof(TestClass.Name), "OldValue", "NewValue");

            string changedPropertyName = null;
            target.PropertyChanged += (sender, args) => changedPropertyName = args.PropertyName;

            // Act
            propertyChange.Undo();

            // Assert
            Assert.That(changedPropertyName, Is.EqualTo(nameof(TestClass.Name)));
        }

        [Test]
        public void Constructor_ThrowsExceptionForInvalidProperty()
        {
            // Arrange
            var target = new TestClass();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new PropertyChange(target, "InvalidProperty", "OldValue", "NewValue"));
        }

        [Test]
        public void Constructor_ThrowsExceptionForTypeMismatch()
        {
            // Arrange
            var target = new TestClass();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new PropertyChange(target, nameof(TestClass.Name), 123, "NewValue"));
        }
    }
}