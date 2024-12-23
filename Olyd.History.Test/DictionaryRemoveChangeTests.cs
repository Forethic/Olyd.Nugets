namespace Olyd.History.Test
{
    [TestFixture]
    public class DictionaryRemoveChangeTests
    {
        [Test]
        public void Constructor_ShowThrowException_WhenDictionaryIsNull()
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";

            // Act & Assert 
            Assert.Throws<ArgumentNullException>(() => new DictionaryRemoveChange<string, string>(null, key, value));
        }

        [Test]
        public void Constructor_ShowThrowException_WhenKeyIsNull()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>();
            var value = "testValue";

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => new DictionaryRemoveChange<string, string>(dictionary, null, value));
        }

        [Test]
        public void Constructor_ShowThrowException_WhenValueIsNull()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>();
            var key = "testKey";

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => new DictionaryRemoveChange<string, string>(dictionary, key, null));
        }

        [Test]
        public void Redo_RemovesKeyValueFromDictionary()
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";
            var dictionary = new Dictionary<string, string>() { { key, value } };
            var dictionaryRemoveChange = new DictionaryRemoveChange<string, string>(dictionary, key, value);

            // Act  
            dictionaryRemoveChange.Redo();

            // Assert
            Assert.That(dictionary, Is.Empty);
            Assert.That(dictionary, Does.Not.ContainKey(key).And.Not.ContainValue(value));
        }

        [Test]
        public void Undo_AddsKeyValueBackToDictionary()
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";
            var dictionary = new Dictionary<string, string>();
            var dictionaryRemoveChange = new DictionaryRemoveChange<string, string>(dictionary, key, value);

            // Act
            dictionaryRemoveChange.Undo();

            // Assert
            Assert.That(dictionary, Has.Count.EqualTo(1));
            Assert.That(dictionary, Contains.Key(key).And.ContainValue(value));
        }

        [Test]
        public void Redo_ShouldNotThrow_WhenKeyDoesNotExistInDictionary()
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";
            var dictionary = new Dictionary<string, string>();
            var dictionaryRemoveChange = new DictionaryRemoveChange<string, string>(dictionary, key, value);

            // Act
            dictionaryRemoveChange.Redo();
            dictionaryRemoveChange.Redo();

            // Assert
            Assert.That(dictionary, Is.Empty);
            Assert.That(dictionary, Does.Not.ContainKey(key).And.Not.ContainValue(value));
        }

        [Test]
        public void Undo_ShouldNotThrow_WhenUndoIsCalledMultipleTimes()
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";
            var dictionary = new Dictionary<string, string>() { { key, value } };
            var dictionaryRemoveChange = new DictionaryRemoveChange<string, string>(dictionary, key, value);

            // Act
            dictionaryRemoveChange.Redo();
            dictionaryRemoveChange.Undo();
            dictionaryRemoveChange.Undo();

            // Assert
            Assert.That(dictionary, Has.Count.EqualTo(1));
            Assert.That(dictionary, Contains.Key(key).And.ContainValue(value));
        }
    }
}
