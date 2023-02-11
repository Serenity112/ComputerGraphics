using GeometricStructures;

namespace ComputerGraphics
{
    public class CoordinatesUtil
    {
        public static GeomPoint pointToWFFormat(GeomPoint point, GeomPoint coordinatesCenter)
        {
            float newX = point.x + coordinatesCenter.x;
            float newY = coordinatesCenter.y - point.y;
            return new GeomPoint(newX, newY);
        }

        public static float[] ellipseToWFFormat(Circle circle, GeomPoint coordinatesCenter)
        {
            float[] parameters = new float[4];
            parameters[0] = circle.center.x + coordinatesCenter.x - circle.radius;
            parameters[1] = coordinatesCenter.y - circle.center.y - circle.radius;
            parameters[2] = circle.radius * 2;
            parameters[3] = circle.radius * 2;
            return parameters;
        }
    }
}
