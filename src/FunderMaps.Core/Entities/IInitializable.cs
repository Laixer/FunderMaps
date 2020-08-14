namespace FunderMaps.Core.Entities
{
    public interface IInitializable<TEntity>
        where TEntity : class
    {
        void InitializeDefaults();

        void InitializeDefaults(TEntity other);
    }
}
