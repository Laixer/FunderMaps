using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.WebApi.Profiles
{
    /// <summary>
    ///     Mapping profile.
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public UserProfile()
            => CreateMap<User, UserDto>().ReverseMap();
    }
}
