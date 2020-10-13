using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types.Products;
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
            CreateMap<AnalysisProduct, AnalysisRiskDto>();
            CreateMap<Building, AddressBuildingDto>().ReverseMap();
            CreateMap<Contact, IncidentDto>().ReverseMap();
            CreateMap<Organization, ContractorDto>();
            CreateMap<Incident, IncidentDto>()
                .IncludeMembers(src => src.ContactNavigation)
                .ReverseMap();
            CreateMap<Inquiry, InquiryDto>().ReverseMap();
            CreateMap<InquiryFull, InquiryDto>()
                .ForMember(dest => dest.AuditStatus, o => o.MapFrom(src => src.State.AuditStatus))
                .ForMember(dest => dest.Reviewer, o => o.MapFrom(src => src.Attribution.Reviewer))
                .ForMember(dest => dest.Creator, o => o.MapFrom(src => src.Attribution.Creator))
                .ForMember(dest => dest.Owner, o => o.MapFrom(src => src.Attribution.Owner))
                .ForMember(dest => dest.Contractor, o => o.MapFrom(src => src.Attribution.Contractor))
                .ForMember(dest => dest.AccessPolicy, o => o.MapFrom(src => src.Access.AccessPolicy))
                .ForMember(dest => dest.CreateDate, o => o.MapFrom(src => src.Record.CreateDate))
                .ForMember(dest => dest.UpdateDate, o => o.MapFrom(src => src.Record.UpdateDate))
                .ReverseMap();
            CreateMap<InquirySample, InquirySampleDto>().ReverseMap();
            CreateMap<OrganizationProposal, OrganizationProposalDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Recovery, RecoveryDto>().ReverseMap();
            CreateMap<RecoverySample, RecoverySampleDto>().ReverseMap();
            CreateMap<User, ReviewerDto>();
        }
    }
}
