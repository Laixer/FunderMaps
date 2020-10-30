using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace FunderMaps.Core.Tests.Entities
{
    public class AddressTests
    {
        [Fact]
        public void ValidateAddress()
        {
            // Arrange
            var entity1 = new Address
            {
                Id = "gfm-address",
                BuildingNumber = "12",
                Street = "street",
                IsActive = true,
                ExternalId = "external-1",
                ExternalSource = ExternalDataSource.NlOsm,
                City = "Delft",
                BuildingId = "gfm-building",
            };
            var entity2 = new Address
            {
                BuildingNumber = "12",
                Street = "street",
                IsActive = true,
                ExternalId = "external-1",
                ExternalSource = ExternalDataSource.NlCbs,
                City = "Amsterdam",
                BuildingId = "gfm-building",
            };

            // Act
            entity1.Validate();

            // Assert
            Assert.True(entity1.IsValidated);
            Assert.Contains("id", Assert.Throws<ValidationException>(() => entity2.Validate()).Message, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void EqualsAddress()
        {
            // Arrange
            var entity1 = new Address
            {
                Id = "gfm-address",
                BuildingNumber = "12",
                Street = "street",
                IsActive = true,
                ExternalId = "external-1",
                ExternalSource = ExternalDataSource.NlBag,
                City = "Delft",
                BuildingId = "gfm-building",
            };
            var entity2 = new Address
            {
                Id = "gfm-address",
                BuildingNumber = "12",
                Street = "street",
                IsActive = true,
                ExternalId = "external-1",
                ExternalSource = ExternalDataSource.NlBag,
                City = "Delft",
                BuildingId = "gfm-building",
            };
            var entity3 = new Address
            {
                Id = "gfm-address-2",
                BuildingNumber = "81",
                Street = "lane",
                IsActive = false,
                ExternalId = "external-2",
                ExternalSource = ExternalDataSource.NlCbs,
                City = "Gouda",
                BuildingId = "gfm-building-2",
            };

            // Assert
            Assert.True(entity1.Equals(entity2));
            Assert.False(entity2.Equals(entity3));
            Assert.False(entity2.Equals(null));
            Assert.False(entity2 == null);
        }
    }
}
