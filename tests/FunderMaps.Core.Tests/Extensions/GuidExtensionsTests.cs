using FunderMaps.Core.Extensions;
using System;
using Xunit;

namespace FunderMaps.Core.Tests.Extensions
{
    /// <summary>
    ///     Testing class for <see cref="GuidExtensions"/>.
    /// </summary>
    public sealed class GuidExtensionsTests
    {
        [Fact]
        public void NewGuidDoesNotThrow()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            guid.ThrowIfNullOrEmpty();

            // Assert
            Assert.NotEqual(Guid.Empty, guid);
        }

        [Fact]
        public void EmptyGuidThrowsException()
        {
            // Arrange
            var guid = Guid.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => guid.ThrowIfNullOrEmpty());
        }
    }
}
