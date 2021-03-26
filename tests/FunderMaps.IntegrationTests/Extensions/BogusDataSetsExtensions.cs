namespace Bogus.DataSets
{
    public static class BogusDataSetsExtensions
    {
        public static string Password(this Randomizer randomizer, int length = 12)
            => randomizer.String2(length, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()_-==+`~");
    }
}
