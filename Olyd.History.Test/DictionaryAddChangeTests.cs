namespace Olyd.History.Test
{
    [TestFixture]
    public class DictionaryAddChangeTests
    {
        [Test]
        public void Constructor_ShowThrowException_WhenDictionaryIsNull()
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";

            // Act & Assert 
            Assert.Throws<ArgumentNullException>(() => new DictionaryAddChange<string, string>(null, key, value));
        }

        [Test]
        public void Constructor_ShowThrowException_WhenKeyIsNull()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>();
            var value = "testValue";

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => new DictionaryAddChange<string, string>(dictionary, null, value));
        }

        [Test]
        public void Constructor_ShowThrowException_WhenValueIsNull()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>();
            var key = "testKey";

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => new DictionaryAddChange<string, string>(dictionary, key, null));
        }

        [Test]
        public void Redo_AddsKeyValueToDictionary()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>();
            var key = "testKey";
            var value = "testValue";
            var dictionaryAddChange = new DictionaryAddChange<string, string>(dictionary, key, value);

            // Act  
            dictionaryAddChange.Redo();

            // Assert
            Assert.That(dictionary, Contains.Key(key).And.ContainValue(value));
        }

        [Test]
        public void Undo_RemovesKeyValueFromDictionary()
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";
            var dictionary = new Dictionary<string, string>() { { key, value } };
            var dictionaryAddChange = new DictionaryAddChange<string, string>(dictionary, key, value);

            // Act
            dictionaryAddChange.Undo();

            // Assert
            Assert.That(dictionary, Does.Not.ContainKey(key).And.Not.ContainValue(value));
        }

        [Test]
        public void Redo_ShouldNotAddDuplicateKey()
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";
            var dictionary = new Dictionary<string, string>() { { key, value } };
            var dictionaryAddChange = new DictionaryAddChange<string, string>(dictionary, key, value);

            // Act
            dictionaryAddChange.Redo();
            dictionaryAddChange.Redo();

            // Assert
            Assert.That(dictionary, Has.Count.EqualTo(1));
            Assert.That(dictionary, Contains.Key(key).And.ContainValue(value));
        }

        [Test]
        public void Undo_ShouldNotThrow_WhenUndoIsCalledMultipleTimes()
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";
            var dictionary = new Dictionary<string, string>() { { key, value } };
            var dictionaryAddChange = new DictionaryAddChange<string, string>(dictionary, key, value);

            // Act
            dictionaryAddChange.Redo();
            dictionaryAddChange.Undo();
            dictionaryAddChange.Undo();

            // Assert
            Assert.That(dictionary, Is.Empty);
            Assert.That(dictionary, Does.Not.ContainKey(key).And.Not.ContainValue(value));
        }
    }
}
