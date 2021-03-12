using Xunit;

namespace FunderMaps.IntegrationTests
{
    /// <summary>
    ///     Generate random theory data.
    /// </summary>
    public class RandomStringGeneratorP1 : TheoryData<string>
    {
        private readonly Bogus.Randomizer randomizer = new();

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RandomStringGeneratorP1()
        {
            var p1 = randomizer.String2(randomizer.Int(0, 128), "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()_-==+`~");

            Add(p1);
        }
    }

    /// <summary>
    ///     Generate random theory data.
    /// </summary>
    public class RandomStringGeneratorP2 : TheoryData<string, string>
    {
        private readonly Bogus.Randomizer randomizer = new();

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RandomStringGeneratorP2()
        {
            var p1 = randomizer.String2(randomizer.Int(0, 128), "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()_-==+`~");
            var p2 = randomizer.String2(randomizer.Int(0, 128), "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()_-==+`~");

            Add(p1, p2);
        }
    }
}
