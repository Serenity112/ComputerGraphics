using System;

namespace GeometricStructures
{
    public class Circle
    {
        public Circle(GeomPoint center, double radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public GeomPoint center { get; private set; }
        public double radius { get; private set; }


        public static GeomPoint[] GetTangentPoints(Circle circle, GeomPoint targetPoint)
        {
            GeomPoint[] result = new GeomPoint[2];

            double dx = targetPoint.x - circle.center.x;
            double dy = targetPoint.y - circle.center.y;
            double L = Math.Sqrt(dx * dx + dy * dy);

            double th = Math.Acos(circle.radius / L);

            double d = Math.Atan2(targetPoint.y - circle.center.y, targetPoint.x - circle.center.x);

            double d1 = d + th;
            double d2 = d - th;

            double T1x = circle.center.x + circle.radius * Math.Cos(d1);
            double T1y = circle.center.y + circle.radius * Math.Sin(d1);

            double T2x = circle.center.x + circle.radius * Math.Cos(d2);
            double T2y = circle.center.y + circle.radius * Math.Sin(d2);

            result[0] = new GeomPoint(T1x, T1y);
            result[1] = new GeomPoint(T2x, T2y);

            return result;
        }
        public static bool IfPointInsideCircle(GeomPoint point, Circle circle)
        {
            double sqrMagn = Math.Pow(circle.center.x - point.x, 2) + Math.Pow(circle.center.y - point.y, 2);

            return sqrMagn < Math.Pow(circle.radius, 2);
        }
    }
}