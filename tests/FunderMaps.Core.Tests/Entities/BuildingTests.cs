using FunderMaps.Core.Entities;
using FunderMaps.Core.Entities.Geocoder;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace FunderMaps.Core.Tests.Entities
{
    public class BuildingTests
    {
        [Fact]
        public void ValidateBuilding()
        {
            // Arrange
            var entity1 = new Building
            {
                Id = "gfm-building",
                BuiltYear = DateTime.Now,
                IsActive = true,
                Address = "gfm-address",
                ExternalId = "external-1",
                ExternalSource = "ext",
            };
            var entity2 = new Building
            {
                BuiltYear = DateTime.Now,
                IsActive = true,
                Address = "gfm-address",
                ExternalId = "external-1",
                ExternalSource = "ext",
            };

            // Act
            entity1.Validate();

            // Assert
            Assert.True(entity1.IsValidated);
            Assert.IsAssignableFrom<IGeocoderEntity<Building>>(entity1);
            Assert.Contains("id", Assert.Throws<ValidationException>(() => entity2.Validate()).Message, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void EqualsBuilding()
        {
            // Arrange
            var entity1 = new Building
            {
                Id = "gfm-building",
                BuiltYear = DateTime.Now,
                IsActive = true,
                Address = "gfm-address",
                ExternalId = "external-1",
                ExternalSource = "ext",
            };
            var entity2 = new Building
            {
                Id = "gfm-building",
                BuiltYear = DateTime.Now,
                IsActive = true,
                Address = "gfm-address",
                ExternalId = "external-1",
                ExternalSource = "ext",
            };
            var entity3 = new Building
            {
                Id = "gfm-building-2",
                BuiltYear = DateTime.Now,
                IsActive = false,
                Address = "gfm-address-2",
                ExternalId = "external-2",
                ExternalSource = "ext",
            };

            // Assert
            Assert.True(entity1.Equals(entity2));
            Assert.False(entity2.Equals(entity3));
            Assert.False(entity2.Equals(null));
            Assert.False(entity2 == null);
        }
    }
}
