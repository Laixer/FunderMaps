using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.WebApi.Profiles
{
    /// <summary>
    ///     Mapping profile.
    /// </summary>
    public class AddressProfile : Profile
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AddressProfile()
            => CreateMap<Address, AddressDto>().ReverseMap();
    }
}
