using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using System.Net.Http.Headers;
using static Olyd.History.Test.PropertyChangeTests;

namespace Olyd.History.Test
{
    [TestFixture]
    public class ChangeTrackerTests
    {
        private IList<object> _selectedItems;
        private ChangeTracker _changeTracker;

        [SetUp]
        public void Setup()
        {
            // 初始化选中的对象列表
            _selectedItems = new List<object>() { "Item1", "Item2" };

            // 初始化 ChangeTracker
            _changeTracker = new ChangeTracker(_selectedItems);
        }

        [TearDown]
        public void TearDown()
        {
            // 清理任何必要的资源
            _changeTracker?.Dispose();
            HistoryManager.Clear();
        }

        [Test]
        public void ChangesCount_ShouldReturnCorrectCount_WhenChangesAdded()
        {
            // Arrange
            var change1 = new PropertyChange(new TestClass(), nameof(TestClass.Name), "OldValue1", "NewValue1");
            var change2 = new PropertyChange(new TestClass(), nameof(TestClass.Name), "OldValue2", "NewValue2");

            // Act
            _changeTracker.AddChange(change1);
            _changeTracker.AddChange(change2);

            // Assert
            Assert.That(_changeTracker.ChangesCount, Is.EqualTo(2)); // Should be 2 after adding two changes
        }

        [Test]
        public void ChangesCount_ShouldReturnZero_WhenChangesSubmitted()
        {
            // Arrange
            var change = new PropertyChange(new TestClass(), nameof(TestClass.Name), "OldValue1", "NewValue1");

            // Act
            _changeTracker.AddChange(change);
            _changeTracker.Dispose(); // This should submit changes, clearing the list

            // Assert
            Assert.That(_changeTracker.ChangesCount, Is.EqualTo(0)); // Should be 0 after submitting changes
        }

        [Test]
        public void ChangeTracker_TracksBeforeAdnAfterSelectedItems()
        {
            // Arrange
            var newItems = new List<object> { "Item3", "Item4" };

            // Act
            _changeTracker.ChangeAfterSelectedItems(newItems.ToArray());

            // Assert
            Assert.That(_changeTracker.BeforeSelectedItems, Is.EqualTo(_selectedItems));
            Assert.That(_changeTracker.AfterSelectedItems, Is.EqualTo(newItems));
        }

        [Test]
        public void ChangeTracker_AddsChangesToHistoryOnDispose()
        {
            // Arrange
            var target = new TestClass();
            var change = new PropertyChange(target, nameof(TestClass.Name), "OldValue", "NewValue");
            _changeTracker.AddChange(change);

            // Act
            _changeTracker.Dispose();  // This should trigger history recording

            // Assert
            Assert.That(HistoryManager.CanUndo, Is.True); // There should be something in the history
        }

        [Test]
        public void ChangeTracker_DoesNotRecordEmptyChanges()
        {
            // Arrange
            _changeTracker.Dispose(); // Should not add any changes if no changes are made

            // Act & Assert
            Assert.That(HistoryManager.CanUndo, Is.False); // No changes should be recorded
        }

        [Test]
        public void ChangeTracker_HandlesNestedTrackers()
        {
            // Arrange
            var nestedItems = new List<object> { "NestedItem1", "NestedItem2" };

            // Act
            using (var nestedTracker = new ChangeTracker(nestedItems))
            {
                nestedTracker.ChangeAfterSelectedItems("NestedNewItem");
            }

            // Assert
            // Parent should still hold its changes, nested should be isolated
            Assert.That(_changeTracker.BeforeSelectedItems, Is.EqualTo(_selectedItems));
            Assert.That(_changeTracker.AfterSelectedItems, Is.EqualTo(_selectedItems));
        }

        [Test]
        public void ChangeTracker_HandlesMultipleChangesIncludingNestedChanges()
        {
            // Arrange

            var change1 = new PropertyChange(new TestClass(), nameof(TestClass.Name), "OldValue1", "NewValue1");
            var change2 = new PropertyChange(new TestClass(), nameof(TestClass.Name), "OldValue2", "NewValue2");

            _changeTracker.AddChange(change1);
            _changeTracker.AddChange(change2);

            // Act
            using (var nestedTracker = new ChangeTracker(new List<object> { "NestedItem1", "NestedItem2" }))
            {
                nestedTracker.AddChange(new PropertyChange(new TestClass(), nameof(TestClass.Name), "OldValue3", "NewValue3"));
            }

            // Act - Dispose parent tracker to record all changes
            Assert.That(_changeTracker.ChangesCount, Is.EqualTo(3)); // 2 parent + 1 nested change
        }
    }
}
