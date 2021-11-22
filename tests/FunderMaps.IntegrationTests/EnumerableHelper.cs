using System.Collections;

namespace FunderMaps.IntegrationTests;

public abstract class EnumerableHelper<T> : IEnumerable<object[]>
{
    protected abstract IEnumerable<T> GetEnumerableEntity();

    protected virtual IEnumerable<object[]> GetData()
        => GetEnumerableEntity().Select(s => new object[] { s });

    public IEnumerator<object[]> GetEnumerator() => GetData().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
