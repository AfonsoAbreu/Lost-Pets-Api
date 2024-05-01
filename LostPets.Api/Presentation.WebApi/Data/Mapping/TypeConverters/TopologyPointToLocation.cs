using AutoMapper;
using NetTopologySuite.Geometries;
using Presentation.WebApi.Data.DTOs;

namespace Presentation.WebApi.Data.Mapping.TypeConverters
{
    public class TopologyPointToLocation : ITypeConverter<Point, LocationDTO>
    {
        public LocationDTO Convert(Point source, LocationDTO destination, ResolutionContext context)
        {
            return new LocationDTO
            {
                latitude = source.Y, 
                longitude = source.X
            };
        }
    }
}
