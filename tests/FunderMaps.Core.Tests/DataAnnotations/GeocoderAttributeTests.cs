using FunderMaps.Core.DataAnnotations;
using Xunit;

namespace FunderMaps.Core.Tests.DataAnnotations
{
    public class GeocoderAttributeTests : IClassFixture<GeocoderAttribute>
    {
        private readonly GeocoderAttribute _geocoderAttribute;

        public GeocoderAttributeTests(GeocoderAttribute geocoderAttribute)
        {
            _geocoderAttribute = geocoderAttribute;
        }

        [Fact]
        public void IsValidOnInput()
        {
            // Assert
            Assert.True(_geocoderAttribute.IsValid(null));
            Assert.True(_geocoderAttribute.IsValid("gfm-12345"));
        }

        [Fact]
        public void IsInvalidOnInput()
        {
            // Assert
            Assert.False(_geocoderAttribute.IsValid(""));
            Assert.False(_geocoderAttribute.IsValid("ggfm-12345"));
            Assert.False(_geocoderAttribute.IsValid("gfm12345"));
            Assert.False(_geocoderAttribute.IsValid("GFM-12345"));
        }
    }
}
