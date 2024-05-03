using AutoMapper;
using Infrastructure.Data.Entities;
using NetTopologySuite.Geometries;
using Presentation.WebApi.Data.DTOs;
using Presentation.WebApi.Data.DTOs.Variations;
using Presentation.WebApi.Data.Mapping.TypeConverters;

namespace Presentation.WebApi.Data.Mapping
{
    public class DTOMappingProfile : Profile
    {
        public DTOMappingProfile()
        {
            // Naming conventions
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;
            SourceMemberNamingConvention = PascalCaseNamingConvention.Instance;

            #region Mappings

            #region Entity -> DTO

            CreateMap<CommentDTO, Comment>();
            CreateMap<LocationDTO, Point>()
                .ConvertUsing<LocationToTopologyPoint>();
            CreateMap<MissingPetDTO, MissingPet>();
            CreateMap<PetDTO, Pet>();
            CreateMap<SightingDTO, Sighting>();
            CreateMap<SightingDTOWithRequiredMissingPetId, Sighting>();
            CreateMap<CommentDTOWithRequiredMissingPetId, Comment>();

            #endregion

            #region DTO -> Entity

            CreateMap<Comment, CommentDTO>();
            CreateMap<Point, LocationDTO>()
                .ConvertUsing<TopologyPointToLocation>();
            CreateMap<MissingPet, MissingPetDTO>();
            CreateMap<Pet, PetDTO>();
            CreateMap<Sighting, SightingDTO>();
            CreateMap<Sighting, SightingDTOWithRequiredMissingPetId>();
            CreateMap<Comment, CommentDTOWithRequiredMissingPetId>();

            #endregion

            #endregion
        }
    }
}
