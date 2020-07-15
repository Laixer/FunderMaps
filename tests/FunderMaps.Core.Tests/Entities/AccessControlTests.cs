using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using Xunit;

namespace FunderMaps.Core.Tests.Entities
{
    public class AccessControlTests
    {
        private class TestEntity : AccessControl
        {
        }

        [Fact]
        public void IsPrivateByDefeault()
        {
            // Arrange
            var entity = new TestEntity();

            // Assert
            Assert.Equal(AccessPolicy.Private, entity.AccessPolicy);
        }

        [Fact]
        public void IsPrivateReturnsTrue()
        {
            // Arrange
            var entity = new TestEntity();
            entity.AccessPolicy = AccessPolicy.Private;

            // Assert
            Assert.True(entity.IsPrivate);
            Assert.False(entity.IsPublic);
        }

        [Fact]
        public void IsPublicReturnsTrue()
        {
            // Arrange
            var entity = new TestEntity();
            entity.AccessPolicy = AccessPolicy.Public;

            // Assert
            Assert.True(entity.IsPublic);
            Assert.False(entity.IsPrivate);
        }
    }
}
