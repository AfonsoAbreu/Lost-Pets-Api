using AutoMapper;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Presentation.WebApi.Data.DTOs;

namespace Presentation.WebApi.Data.Mapping.TypeConverters
{
    public class LocationToTopologyPoint : ITypeConverter<LocationDTO, Point>
    {
        public Point Convert(LocationDTO source, Point destination, ResolutionContext context)
        {
            GeometryFactory geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            Point point = geometryFactory.CreatePoint(new Coordinate(source.longitude, source.latitude));

            return point;
        }
    }
}
