namespace Olyd.History.Test
{
    [TestFixture]
    public class ListAddChangeTests
    {
        [Test]
        public void Constructor_ShouldThrowException_WhenCollectionIsNull()
        {
            // Arrange
            IList<object> collection = null;
            var target = new object();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ListAddChange<object>(collection, target));
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenTargetIsNull()
        {
            // Arrange
            var collection = new List<object>();
            object target = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ListAddChange<object>(collection, target));
        }

        [Test]
        public void Redo_AddsTargetToList()
        {
            // Arrange
            var target = new object();
            var collection = new List<object>();
            var listRemoveChange = new ListAddChange<object>(collection, target);

            // Act
            listRemoveChange.Redo();

            // Assert
            Assert.That(collection, Contains.Item(target));
        }

        [Test]
        public void Undo_RemovesTargetFromList()
        {
            // Arrange
            var target = new object();
            var collection = new List<object>() { target };
            var listRemoveChange = new ListAddChange<object>(collection, target);

            // Act
            listRemoveChange.Undo();

            // Assert
            Assert.That(collection, Does.Not.Contain(target));
        }

        [Test]
        public void Redo_ShouldNotThrow_WhenRedoIsCalledMultipleTimes()
        {
            // Arrange 
            var target = new object();
            var collection = new List<object>();
            var listRemoveChange = new ListAddChange<object>(collection, target);

            // Act
            listRemoveChange.Redo();
            listRemoveChange.Redo(); // Redo aggin

            // Assert
            Assert.That(collection, Has.Count.EqualTo(1));
        }

        [Test]
        public void Undo_ShouldNotThrow_WhenUndoIsCalledMultipleTimes()
        {
            // Arrange 
            var target = new object();
            var collection = new List<object>() { target };
            var listRemoveChange = new ListAddChange<object>(collection, target);

            // Act
            listRemoveChange.Undo();
            listRemoveChange.Undo(); // Redo aggin

            // Assert
            Assert.That(collection, Is.Empty);
        }
    }
}
