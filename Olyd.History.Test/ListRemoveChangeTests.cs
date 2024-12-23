using System.Formats.Asn1;

namespace Olyd.History.Test
{
    [TestFixture]
    public class ListRemoveChangeTests
    {
        [Test]
        public void Constructor_ShouldThrowException_WhenCollectionIsNull()
        {
            // Arrange
            IList<object> collection = null;
            var target = new object();
            int index = 0;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ListRemoveChange<object>(collection, target, index));
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenTargetIsNull()
        {
            // Arrange
            var collection = new List<object>();
            object target = null;
            int index = 0;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ListRemoveChange<object>(collection, target, index));
        }

        [Test]
        public void Redo_RemovesTargetFromList()
        {
            // Arrange
            var target = new object();
            var collection = new List<object>() { target };
            var index = collection.IndexOf(target);
            var listRemoveChange = new ListRemoveChange<object>(collection, target, index);

            // Act
            listRemoveChange.Redo();

            // Assert
            Assert.That(collection, Does.Not.Contain(target));
        }

        [Test]
        public void Undo_AddsTargetBackToList()
        {
            // Arrange
            var target = new object();
            var collection = new List<object>() { target };
            var index = collection.IndexOf(target);
            var listRemoveChange = new ListRemoveChange<object>(collection, target, index);

            // Act
            listRemoveChange.Redo();
            listRemoveChange.Undo();

            // Assert
            Assert.That(collection, Contains.Item(target));
        }

        [Test]
        public void Redo_ShouldNotThrow_WhenRedoIsCalledMultipleTimes()
        {
            // Arrange 
            var target = new object();
            var collection = new List<object>() { target };
            var index = collection.IndexOf(target);
            var listRemoveChange = new ListRemoveChange<object>(collection, target, index);

            // Act
            listRemoveChange.Redo();
            listRemoveChange.Redo(); // Redo aggin

            // Assert
            Assert.That(collection, Is.Empty);
        }

        [Test]
        public void Undo_ShouldNotThrow_WhenUndoIsCalledMultipleTimes()
        {
            // Arrange 
            var target = new object();
            var collection = new List<object>() { target };
            var index = collection.IndexOf(target);
            var listRemoveChange = new ListRemoveChange<object>(collection, target, index);

            // Act
            listRemoveChange.Redo();
            listRemoveChange.Undo();
            listRemoveChange.Undo(); // Redo aggin

            // Assert
            Assert.That(collection, Has.Count.EqualTo(1));
            Assert.That(collection, Contains.Item(target));
        }
    }
}
