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
            CreateMap<Address, AddressBuildingDto>()
                .IncludeMembers(src => src.BuildingNavigation)
                .ForMember(dest => dest.AddressId, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.BuildingId, o => o.MapFrom(src => src.BuildingNavigation.Id))
                .ForMember(dest => dest.BuildingGeometry, o => o.MapFrom(src => src.BuildingNavigation.Geometry));
            CreateMap<Building, AddressBuildingDto>().ReverseMap();
            CreateMap<Contact, IncidentDto>().ReverseMap();
            CreateMap<Organization, ContractorDto>();
            CreateMap<Incident, IncidentDto>()
                .IncludeMembers(src => src.ContactNavigation)
                .ReverseMap();
            CreateMap<Inquiry, InquiryDto>().ReverseMap();
            CreateMap<InquirySample, InquirySampleDto>().ReverseMap();
            CreateMap<Organization, OrganizationDto>().ReverseMap();
            CreateMap<OrganizationProposal, OrganizationProposalDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Recovery, RecoveryDto>().ReverseMap();
            CreateMap<RecoverySample, RecoverySampleDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, ReviewerDto>();
        }
    }
}
