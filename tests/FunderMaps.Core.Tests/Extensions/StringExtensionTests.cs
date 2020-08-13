using FunderMaps.Core.Extensions;
using System;
using Xunit;

namespace FunderMaps.Core.Tests.Extensions
{
    /// <summary>
    ///     Testing class for <see cref="FunderMaps.Core.Extensions.StringExtensions"/>
    /// </summary>
    public sealed class StringExtensionTests
    {
        [Fact]
        public void EmptyStringThrows()
        {
            // Arrange
            var str = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => str.ThrowIfNullOrEmpty());
        }

        [Fact]
        public void NullStringThrows()
        {
            // Arrange
            var str = null as string;

            // Assert.
            Assert.Throws<ArgumentNullException>(() => str.ThrowIfNullOrEmpty());
        }

        [Fact]
        public void NonEmptyStringdoesNotThrow()
        {
            // Arrange
            var str = "This is my nonempty string";

            // Act
            str.ThrowIfNullOrEmpty();

            // Assert.
            Assert.False(string.IsNullOrEmpty(str));
        }
    }
}
