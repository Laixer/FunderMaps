namespace Bogus.DataSets
{
    public static class BogusDataSetsExtensions
    {
        /// <summary>
        ///     Generate a new random password.
        /// </summary>
        /// <param name="length">Length of password. Defaults to 12.</param>
        /// <returns>Random generated password.</returns>
        public static string Password(this Randomizer randomizer, int length = 12)
            => randomizer.String2(length, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()_-==+`~");
    }
}
