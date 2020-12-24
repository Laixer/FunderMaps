using FunderMaps.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace FunderMaps.Core.Tests.Entities
{
    public class UserTests
    {
        [Fact]
        public void ValidateUser()
        {
            // Arrange
            var entity1 = new User { Email = "info@example.org" };
            var entity2 = new User { };

            // Act
            entity1.Validate();

            // Assert
            Assert.True(entity1.IsValidated);
            Assert.Contains("email", Assert.Throws<ValidationException>(() => entity2.Validate()).Message, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void EqualsUser()
        {
            // Arrange
            var entity1 = new User
            {
                Id = Guid.NewGuid(),
                Email = "info@example.org",
                Role = Types.ApplicationRole.Guest,
            };
            var entity2 = new User
            {
                Id = entity1.Id,
                Email = "info@example.org",
                Role = Types.ApplicationRole.Guest,
            };
            var entity3 = new User
            {
                Id = Guid.NewGuid(),
                Email = "info@example.org",
                Role = Types.ApplicationRole.Guest,
            };

            // Assert
            Assert.True(entity1.Equals(entity2));
            Assert.False(entity2.Equals(entity3));
            Assert.False(entity2.Equals(null));
            Assert.False(entity2 == null);
        }
    }
}
