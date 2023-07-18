using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace FunderMaps.AspNetCore.DataProtection;

public class KeystoreXmlRepository : IXmlRepository
{
    private readonly FunderMaps.Core.Interfaces.Repositories.IKeystoreRepository _keystoreRepository;

    public KeystoreXmlRepository(FunderMaps.Core.Interfaces.Repositories.IKeystoreRepository keystoreRepository)
    {
        _keystoreRepository = keystoreRepository ?? throw new ArgumentNullException(nameof(keystoreRepository));
    }

    public IReadOnlyCollection<XElement> GetAllElements()
    {
        var rr = _keystoreRepository.ListAllAsync(Core.Navigation.All).ToListAsync().GetAwaiter().GetResult();

        return rr.Select(x => XElement.Parse(x.Value)).ToList().AsReadOnly();
    }

    public void StoreElement(XElement element, string friendlyName)
    {
        var v = element.ToString(SaveOptions.DisableFormatting);

        _keystoreRepository.AddAsync(new FunderMaps.Core.Entities.KeyStore()
        {
            Name = friendlyName,
            Value = v,
        }).GetAwaiter().GetResult();
    }
}
