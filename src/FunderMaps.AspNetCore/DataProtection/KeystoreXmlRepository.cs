using System.Xml.Linq;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace FunderMaps.AspNetCore.DataProtection;

public class KeystoreXmlRepository : IXmlRepository
{
    private readonly IKeystoreRepository _keystoreRepository;

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public KeystoreXmlRepository(IKeystoreRepository keystoreRepository)
        => _keystoreRepository = keystoreRepository ?? throw new ArgumentNullException(nameof(keystoreRepository));

    public IReadOnlyCollection<XElement> GetAllElements()
    {
        var rr = _keystoreRepository.ListAllAsync(Core.Navigation.All).ToListAsync().GetAwaiter().GetResult();

        return rr.Select(x => XElement.Parse(x.Value)).ToList().AsReadOnly();
    }

    public void StoreElement(XElement element, string friendlyName)
    {
        _keystoreRepository.AddAsync(new FunderMaps.Core.Entities.KeyStore()
        {
            Name = friendlyName,
            Value = element.ToString(SaveOptions.DisableFormatting),
        }).GetAwaiter().GetResult();
    }
}
