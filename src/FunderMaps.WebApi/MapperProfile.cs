using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.WebApi
{
    /// <summary>
    ///     Mapper profile for proper DTO mapping.
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        ///     Setup mapping profiles.
        /// </summary>
        public MapperProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Contact, IncidentDto>().ReverseMap();
            CreateMap<Incident, IncidentDto>()
                .IncludeMembers(s => s.ContactNavigation)
                .ReverseMap();
            CreateMap<Inquiry, InquiryDto>().ReverseMap();
            CreateMap<InquirySample, InquirySampleDto>().ReverseMap();
            CreateMap<Organization, OrganizationDto>().ReverseMap();
            CreateMap<OrganizationProposal, OrganizationProposalDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Recovery, RecoveryDto>().ReverseMap();
            CreateMap<RecoverySample, RecoverySampleDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
