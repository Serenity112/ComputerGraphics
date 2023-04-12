using System.Collections.Generic;

namespace GeometricStructures
{
    public class Surface : List<int>
    {
        public bool Invisible = false;

        public Surface(List<int> points) : base(points) { }
    }

    public class VolumetricObject
    {
        public VolumetricObject(GeomPoint[] vertices, List<Surface> surfaces)
        {
            Vertices = vertices;
            Surfaces = surfaces;
        }

        public GeomPoint[] Vertices;
        public List<Surface> Surfaces;

        public VolumetricObject Get2DProjection(Dictionary<GeomPoint, GeomPoint> translation)
        {
            GeomPoint[] translatedPoints = new GeomPoint[Vertices.Length];
            for (int i = 0; i < Vertices.Length; i++)
            {
                translatedPoints[i] = translation[Vertices[i]];
            }

            return new VolumetricObject(translatedPoints, Surfaces);
        }
    }
}
