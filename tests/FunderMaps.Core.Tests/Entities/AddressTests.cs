using FunderMaps.Core.Entities;
using FunderMaps.Core.Entities.Geocoder;
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
            };
            var entity2 = new Address
            {
                BuildingNumber = "12",
                Street = "street",
                IsActive = true,
                ExternalId = "external-1",
                ExternalSource = ExternalDataSource.NlCbs,
            };

            // Act
            entity1.Validate();

            // Assert
            Assert.True(entity1.IsValidated);
            Assert.IsAssignableFrom<IGeocoderEntity<Address>>(entity1);
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
            };
            var entity2 = new Address
            {
                Id = "gfm-address",
                BuildingNumber = "12",
                Street = "street",
                IsActive = true,
                ExternalId = "external-1",
                ExternalSource = ExternalDataSource.NlBag,
            };
            var entity3 = new Address
            {
                Id = "gfm-address-2",
                BuildingNumber = "81",
                Street = "lane",
                IsActive = false,
                ExternalId = "external-2",
                ExternalSource = ExternalDataSource.NlCbs,
            };

            // Assert
            Assert.True(entity1.Equals(entity2));
            Assert.False(entity2.Equals(entity3));
            Assert.False(entity2.Equals(null));
            Assert.False(entity2 == null);
        }
    }
}
