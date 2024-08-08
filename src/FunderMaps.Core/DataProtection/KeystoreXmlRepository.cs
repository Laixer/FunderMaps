using System.Xml.Linq;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace FunderMaps.Core.DataProtection;

/// <summary>
///     Construct new instance.
/// </summary>
public class KeystoreXmlRepository(IKeystoreRepository keystoreRepository) : IXmlRepository
{
    public IReadOnlyCollection<XElement> GetAllElements()
    {
        // TODO: Use async/await.
        var rr = keystoreRepository.ListAllAsync().ToListAsync().GetAwaiter().GetResult();

        return rr.Select(x => XElement.Parse(x.Value)).ToList().AsReadOnly();
    }

    public void StoreElement(XElement element, string friendlyName)
    {
        keystoreRepository.AddAsync(new KeyStore
        {
            Name = friendlyName,
            Value = element.ToString(SaveOptions.DisableFormatting),
        }).GetAwaiter().GetResult();
    }
}
