using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.WebApi.Profiles
{
    /// <summary>
    /// Mapping profile.
    /// </summary>
    public class ProjectProfile : Profile
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        public ProjectProfile()
            => CreateMap<Project, ProjectDTO>().ReverseMap();
    }
}
