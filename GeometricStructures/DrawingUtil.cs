using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GeometricStructures
{
    public static class DrawingUtil
    {
        public static void DrawLine(GeomPoint point1, GeomPoint point2, Graphics G, Pen pen)
        {
            PointF f_point1 = new PointF((float)point1.x, (float)point1.y);
            PointF f_point2 = new PointF((float)point2.x, (float)point2.y);

            G.DrawLine(pen, f_point1, f_point2);
        }

        public static void DrawEllipse(Circle circle, Graphics G, Pen pen)
        {
            double x = circle.center.x - circle.radius;
            double y = circle.center.y - circle.radius;
            double d = circle.radius * 2;

            G.DrawEllipse(pen, (float)x, (float)y, (float)d, (float)d);
        }

        public static void FillEllipse(Circle circle, Graphics G, Brush brush)
        {
            double x = circle.center.x - circle.radius;
            double y = circle.center.y - circle.radius;
            double d = circle.radius * 2;

            G.FillEllipse(brush, (float)x, (float)y, (float)d, (float)d);
        }
    }
}