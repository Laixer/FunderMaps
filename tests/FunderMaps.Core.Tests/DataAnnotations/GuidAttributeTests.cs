using FunderMaps.Core.DataAnnotations;
using System;
using Xunit;

namespace FunderMaps.Core.Tests.DataAnnotations
{
    public class GuidAttributeTests
    {
        [Fact]
        public void IsValidOnInput()
        {
            // Arrange
            var attr = new GuidAttribute();

            // Assert
            Assert.True(attr.IsValid(Guid.NewGuid()));
        }

        [Fact]
        public void IsInvalidOnInput()
        {
            // Arrange
            var attr = new GuidAttribute();

            // Assert
            Assert.False(attr.IsValid(Guid.Empty));
        }
    }
}
