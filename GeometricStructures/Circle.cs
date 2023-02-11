using System;

namespace GeometricStructures
{
    public class Circle
    {
        public Circle(GeomPoint center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public GeomPoint center { get; private set; }
        public float radius { get; private set; }
    }

    public class CircleUtils
    {
        public static GeomPoint[] GetTangentPointCoordinate(Circle cirlce, GeomPoint targetPoint)
        {
            GeomPoint[] result = new GeomPoint[2];

            double dx = targetPoint.x - cirlce.center.x;
            double dy = targetPoint.y - cirlce.center.y;
            double L = Math.Sqrt(dx * dx + dy * dy);

            double th = Math.Acos(cirlce.radius / L);

            double d = Math.Atan2(targetPoint.y - cirlce.center.y, targetPoint.x - cirlce.center.x);

            double d1 = d + th;
            double d2 = d - th;

            double T1x = cirlce.center.x + cirlce.radius * Math.Cos(d1);
            double T1y = cirlce.center.y + cirlce.radius * Math.Sin(d1);

            double T2x = cirlce.center.x + cirlce.radius * Math.Cos(d2);
            double T2y = cirlce.center.y + cirlce.radius * Math.Sin(d2);

            result[0] = new GeomPoint((float)T1x, (float)T1y);
            result[1] = new GeomPoint((float)T2x, (float)T2y);

            return result;
        }

        public static bool IfPointInsideCircle(GeomPoint point, Circle circle)
        {
            double distance = Math.Sqrt(Math.Pow(circle.center.x - point.x, 2) + Math.Pow(circle.center.y - point.y, 2));

            return distance < circle.radius;
        }
    }
}