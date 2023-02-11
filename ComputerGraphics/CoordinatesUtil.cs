using GeometricStructures;

namespace ComputerGraphics
{
    public class CoordinatesUtil
    {
        public static GeomPoint pointToWFFormat(GeomPoint point, GeomPoint coordinatesCenter)
        {
            float newX = point[0, 0] + coordinatesCenter[0, 0];
            float newY = coordinatesCenter[1, 0] - point[1, 0];
            return new GeomPoint(newX, newY);
        }

        public static float[] ellipseToWFFormat(Circle circle, GeomPoint coordinatesCenter)
        {
            float[] parameters = new float[4];
            parameters[0] = circle.center[0, 0] + coordinatesCenter[0, 0] - circle.radius;
            parameters[1] = coordinatesCenter[1, 0] - circle.center[1, 0] - circle.radius;
            parameters[2] = circle.radius * 2;
            parameters[3] = circle.radius * 2;
            return parameters;
        }

    }
}
