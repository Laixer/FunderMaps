using FunderMaps.Core.DataAnnotations;
using System;
using Xunit;

namespace FunderMaps.Core.Tests.DataAnnotations
{
    public class GuidAttributeTests : IClassFixture<GuidAttribute>
    {
        private readonly GuidAttribute _guidAttribute;

        public GuidAttributeTests(GuidAttribute guidAttribute)
        {
            _guidAttribute = guidAttribute;
        }

        [Fact]
        public void IsValidOnInput()
        {
            // Assert
            Assert.True(_guidAttribute.IsValid(Guid.NewGuid()));
        }

        [Fact]
        public void IsInvalidOnInput()
        {
            // Assert
            Assert.False(_guidAttribute.IsValid(Guid.Empty));
        }
    }
}
