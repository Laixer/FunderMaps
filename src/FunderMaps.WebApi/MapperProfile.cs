using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;
using FunderMaps.WebApi.InputModels;

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
            CreateMap<Organization, ContractorDto>();
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
            CreateMap<User, ReviewerDto>();
            CreateMap<User, ReviewerDto>();

            // One way inputmodel
            // TODO This sort-of mashes with IncidentDTO
            CreateMap<IncidentInputModel, Incident>();

            // Enum DTO's
            // TODO Use DTO's everywhere and use reverse map as well
            CreateMap<FoundationTypeDTO, FoundationType>();
            CreateMap<FoundationDamageCauseDTO, FoundationDamageCause>();
            CreateMap<FoundationDamageCharacteristicsDTO, FoundationDamageCharacteristics>();
        }
    }
}
