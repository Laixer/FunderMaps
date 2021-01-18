using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
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
                Geometry = "7b2274797065223a224d756c7469506f6c79676f6e222c22636f6f7264696e61746573223a5b5b5b5b352e3833333038343731312c35332e3039333332373638385d2c5b352e3833333033393531392c35332e3039333331343130385d2c5b352e3833333031393439362c35332e30393333333832325d2c5b352e3833323938353930392c35332e3039333332383132385d2c5b352e3833333036303236382c35332e3039333233383438355d2c5b352e38333331333930362c35332e3039333236323136365d2c5b352e3833333038343731312c35332e3039333332373638385d5d5d5d7d",
                ExternalId = "external-1",
                ExternalSource = ExternalDataSource.NlBag,
                BuildingType = BuildingType.Houseboat,
                NeighborhoodId = "gfm-neighborhood",
            };
            var entity2 = new Building
            {
                BuiltYear = DateTime.Now,
                IsActive = true,
                Geometry = "7b2274797065223a224d756c7469506f6c79676f6e222c22636f6f7264696e61746573223a5b5b5b5b342e3438373532373639362c35312e3934353936373435385d2c5b342e3438373433303336392c35312e3934353839323138385d2c5b342e3438373530323836342c35312e3934353835363534355d2c5b342e3438373539393935382c35312e39343539333138345d2c5b342e3438373532373639362c35312e3934353936373435385d5d5d5d7d",
                ExternalId = "external-1",
                ExternalSource = ExternalDataSource.NlCbs,
                BuildingType = BuildingType.MobileHome,
                NeighborhoodId = "gfm-neighborhood",
            };

            // Act
            entity1.Validate();

            // Assert
            Assert.True(entity1.IsValidated);
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
                Geometry = "7b2274797065223a224d756c7469506f6c79676f6e222c22636f6f7264696e61746573223a5b5b5b5b342e3234393139383431322c35312e3838363635353638395d2c5b342e3234393231363939392c35312e3838363639323638395d2c5b342e3234393132373332372c35312e3838363730393933335d2c5b342e32343931303837312c35312e3838363637323934325d2c5b342e3234393139383431322c35312e3838363635353638395d5d5d5d7d",
                ExternalId = "external-1",
                ExternalSource = ExternalDataSource.NlOsm,
            };
            var entity2 = new Building
            {
                Id = "gfm-building",
                BuiltYear = DateTime.Now,
                IsActive = true,
                Geometry = "7b2274797065223a224d756c7469506f6c79676f6e222c22636f6f7264696e61746573223a5b5b5b5b342e3234393139383431322c35312e3838363635353638395d2c5b342e3234393231363939392c35312e3838363639323638395d2c5b342e3234393132373332372c35312e3838363730393933335d2c5b342e32343931303837312c35312e3838363637323934325d2c5b342e3234393139383431322c35312e3838363635353638395d5d5d5d7d",
                ExternalId = "external-1",
                ExternalSource = ExternalDataSource.NlOsm,
            };
            var entity3 = new Building
            {
                Id = "gfm-building-2",
                BuiltYear = DateTime.Now,
                IsActive = false,
                Geometry = "7b2274797065223a224d756c7469506f6c79676f6e222c22636f6f7264696e61746573223a5b5b5b5b342e3439363839323836362c35312e3930303136363532335d2c5b342e3439363833393631322c35312e3930303137303934375d2c5b342e3439363833333735332c35312e3930303134333935345d2c5b342e3439363838373030352c35312e3930303133393632315d2c5b342e3439363839323836362c35312e3930303136363532335d5d5d5d7d",
                ExternalId = "external-2",
                ExternalSource = ExternalDataSource.NlBag,
            };

            // Assert
            Assert.True(entity1.Equals(entity2));
            Assert.False(entity2.Equals(entity3));
            Assert.False(entity2.Equals(null));
            Assert.False(entity2 == null);
        }
    }
}
