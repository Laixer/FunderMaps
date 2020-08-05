using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Types;
using Xunit;

namespace FunderMaps.Core.Tests.Entities
{
    public class StateControlTests
    {
        private class TestEntity : StateControl
        {
        }

        [Fact]
        public void IsTodoByDefeault()
        {
            // Arrange
            var entity = new TestEntity();

            // Assert
            Assert.Equal(AuditStatus.Todo, entity.AuditStatus);
            Assert.True(entity.AllowWrite);
        }

        [Fact]
        public void TransitionToPendingFromTodo()
        {
            // Arrange
            var entity = new TestEntity
            {
                AuditStatus = AuditStatus.Todo
            };

            // Act
            entity.TransitionToPending();

            // Assert
            Assert.Equal(AuditStatus.Pending, entity.AuditStatus);
            Assert.True(entity.AllowWrite);
        }

        [Fact]
        public void TransitionToPendingFromPending()
        {
            // Arrange
            var entity = new TestEntity
            {
                AuditStatus = AuditStatus.Pending
            };

            // Act
            entity.TransitionToPending();

            // Assert
            Assert.Equal(AuditStatus.Pending, entity.AuditStatus);
            Assert.True(entity.AllowWrite);
        }

        [Fact]
        public void TransitionToPendingFromRejected()
        {
            // Arrange
            var entity = new TestEntity
            {
                AuditStatus = AuditStatus.Rejected
            };

            // Act
            entity.TransitionToPending();

            // Assert
            Assert.Equal(AuditStatus.Pending, entity.AuditStatus);
            Assert.True(entity.AllowWrite);
        }

        [Fact]
        public void TransitionToReviewFromPendig()
        {
            // Arrange
            var entity = new TestEntity
            {
                AuditStatus = AuditStatus.Pending
            };

            // Act
            entity.TransitionToReview();

            // Assert
            Assert.Equal(AuditStatus.PendingReview, entity.AuditStatus);
            Assert.False(entity.AllowWrite);
        }

        [Fact]
        public void TransitionToDoneFromReview()
        {
            // Arrange
            var entity = new TestEntity
            {
                AuditStatus = AuditStatus.PendingReview
            };

            // Act
            entity.TransitionToDone();

            // Assert
            Assert.Equal(AuditStatus.Done, entity.AuditStatus);
            Assert.False(entity.AllowWrite);
        }

        [Fact]
        public void TransitionToRejectedFromReview()
        {
            // Arrange
            var entity = new TestEntity
            {
                AuditStatus = AuditStatus.PendingReview
            };

            // Act
            entity.TransitionToRejected();

            // Assert
            Assert.Equal(AuditStatus.Rejected, entity.AuditStatus);
            Assert.False(entity.AllowWrite);
        }

        [Fact]
        public void MoveToPendingFromTodo()
        {
            // Arrange
            var entity = new TestEntity
            {
                AuditStatus = AuditStatus.Todo
            };

            // Act
            entity.MoveNext();

            // Assert
            Assert.Equal(AuditStatus.Pending, entity.AuditStatus);
            Assert.True(entity.AllowWrite);
        }

        [Fact]
        public void MoveToReviewFromPending()
        {
            // Arrange
            var entity = new TestEntity
            {
                AuditStatus = AuditStatus.Pending
            };

            // Act
            entity.MoveNext();

            // Assert
            Assert.Equal(AuditStatus.PendingReview, entity.AuditStatus);
            Assert.False(entity.AllowWrite);
        }

        [Fact]
        public void MoveToReviewFromReview()
        {
            // Arrange
            var entity = new TestEntity
            {
                AuditStatus = AuditStatus.PendingReview
            };

            // Act
            entity.MoveNext();

            // Assert
            Assert.Equal(AuditStatus.PendingReview, entity.AuditStatus);
            Assert.False(entity.AllowWrite);
        }

        [Fact]
        public void TransitionToDiscardedFromTodo()
        {
            // Arrange
            var entity = new TestEntity
            {
                AuditStatus = AuditStatus.PendingReview
            };

            // Act
            entity.TransitionToDiscarded();

            // Assert
            Assert.Equal(AuditStatus.Discarded, entity.AuditStatus);
            Assert.False(entity.AllowWrite);
        }

        [Fact]
        public void TransitionToDiscardedFromDone()
        {
            // Arrange
            var entity = new TestEntity
            {
                AuditStatus = AuditStatus.Done
            };

            // Assert
            Assert.Throws<StateTransitionException>(entity.TransitionToDiscarded);
        }
    }
}
