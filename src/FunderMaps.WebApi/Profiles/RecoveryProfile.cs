using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.WebApi.Profiles
{
    /// <summary>
    ///     Mapping profile.
    /// </summary>
    public class RecoveryProfile : Profile
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RecoveryProfile()
        {
            CreateMap<Recovery, RecoveryDto>().ReverseMap();
            CreateMap<RecoverySample, RecoverySampleDto>().ReverseMap();
        }
    }
}
