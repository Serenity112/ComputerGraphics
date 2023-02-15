using System;
using System.Drawing;

namespace GeometricStructures
{
    public static class DrawingUtil
    {
        public static void DrawLineBresenham(GeomPoint point1, GeomPoint point2, Graphics G, Brush brush, double brushWidth)
        {
            int x1 = Convert.ToInt32(point1.x);
            int y1 = Convert.ToInt32(point1.y);
            int x2 = Convert.ToInt32(point2.x);
            int y2 = Convert.ToInt32(point2.y);

            int w = x2 - x1;
            int h = y2 - y1;

            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;

            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);

            if (longest <= shortest)
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1;
                else if (h > 0) dy2 = 1;
                dx2 = 0;
            }

            int numerator = longest >> 1;

            for (int i = 0; i <= longest; i++)
            {
                DrawPixel(x1, y1, G, brush, brushWidth);

                numerator += shortest;

                if (numerator >= longest)
                {
                    numerator -= longest;
                    x1 += dx1;
                    y1 += dy1;
                }
                else
                {
                    x1 += dx2;
                    y1 += dy2;
                }
            }
        }

        public static void DrawCircleBresenham(Circle circle, Graphics G, Brush brush, double brushWidth)
        {
            double x0 = circle.center.x;
            double y0 = circle.center.y;
            double radius = circle.radius;

            double x = 0;
            double y = radius;
            double d = 3 - 2 * radius;

            drawCircle(x0, y0, x, y, G, brush, brushWidth);

            while (y >= x)
            {
                x++;

                if (d > 0)
                {
                    y--;
                    d = d + 4 * (x - y) + 10; 
                }
                else
                {
                    d = d + 4 * x + 6;
                }
                    
                drawCircle(x0, y0, x, y, G, brush, brushWidth);
            }
        }

        private static void drawCircle(double xc, double yc, double x, double y, Graphics G, Brush brush, double brushWidth)
        {
            DrawPixel(xc + x, yc + y, G, brush, brushWidth);
            DrawPixel(xc - x, yc + y, G, brush, brushWidth);
            DrawPixel(xc + x, yc - y, G, brush, brushWidth);
            DrawPixel(xc - x, yc - y, G, brush, brushWidth);
            DrawPixel(xc + y, yc + x, G, brush, brushWidth);
            DrawPixel(xc - y, yc + x, G, brush, brushWidth);
            DrawPixel(xc + y, yc - x, G, brush, brushWidth);
            DrawPixel(xc - y, yc - x, G, brush, brushWidth);
        }

        public static void DrawPixel(GeomPoint point, Graphics G, Brush brush, double brushWidth)
        {
            DrawPixel(point.x, point.y, G, brush, brushWidth);
        }

        public static void DrawPixel(double x, double y, Graphics G, Brush brush, double brushWidth)
        {
            G.FillRectangle(brush, (float)x, (float)y, (float)brushWidth, (float)brushWidth);
        }
    }
}