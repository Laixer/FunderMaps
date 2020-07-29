using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.WebApi.Profiles
{
    /// <summary>
    ///     Mapping profile.
    /// </summary>
    public class OrganizationProfile : Profile
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationProfile()
        {
            CreateMap<Organization, OrganizationDto>().ReverseMap();
            CreateMap<OrganizationProposal, OrganizationProposalDto>().ReverseMap();
        }
    }
}
