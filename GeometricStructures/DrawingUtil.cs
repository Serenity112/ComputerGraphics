using System;
using System.Drawing;

namespace GeometricStructures
{
    public static class DrawingUtil
    {
        private static Font defaultFont = new Font("Microsoft Sans Serif", 14, FontStyle.Regular, GraphicsUnit.Pixel, 0);

        public static void DrawCoordinates2D(Graphics G, double width, double height, Brush brush, int mark)
        {
            for (int i = 0; i >= -height / 2; i -= mark)
            {
                G.DrawString((-i).ToString(), defaultFont, brush, 0, i);
                G.DrawString(i.ToString(), defaultFont, brush, 0, -i);
            }

            for (int i = mark; i <= width / 2; i += mark)
            {
                G.DrawString(i.ToString(), defaultFont, brush, i, 0);
                G.DrawString((-i).ToString(), defaultFont, brush, -i, 0);
            }

            DrawLineBresenham(new GeomPoint(0, -height / 2), new GeomPoint(0, height / 2), G, brush, 1);
            DrawLineBresenham(new GeomPoint(-width / 2, 0), new GeomPoint(width / 2, 0), G, brush, 1);
        }

        public static void DrawCoordinates3D(Graphics G, double width, double height, Brush brush, int mark)
        {
            for (int y = 0; y >= -height / 2; y -= mark)
            {
                G.DrawString((-y).ToString(), defaultFont, brush, 0, y);
                G.DrawString(y.ToString(), defaultFont, brush, 0, -y);
            }

            for (int x = mark; x <= width / 2; x += mark)
            {
                G.DrawString(x.ToString(), defaultFont, brush, x, 0);
                G.DrawString((-x).ToString(), defaultFont, brush, -x, 0);
            }

            for (int z = mark; z <= Math.Sqrt(Math.Pow(width / 2, 2) + Math.Pow(height / 2, 2)); z += mark)
            {
                float new_X = (float)(0 - z);
                float new_Y = (float)(0 - z);

                G.DrawString(z.ToString(), defaultFont, brush, new_X, -new_Y);
                G.DrawString((-z).ToString(), defaultFont, brush, -new_X, new_Y);
            }

            DrawLineBresenham(new GeomPoint(-width / 2, (width/2) *  Math.Tan(Math.PI/4)), new GeomPoint(width / 2, -(width / 2) * Math.Tan(Math.PI / 4)), G, brush, 1);
            DrawLineBresenham(new GeomPoint(0, -height / 2), new GeomPoint(0, height / 2), G, brush, 1);
            DrawLineBresenham(new GeomPoint(-width / 2, 0), new GeomPoint(width / 2, 0), G, brush, 1);
        }

        public static void DrawLineBresenham(GeomPoint point1, GeomPoint point2, Graphics G, Brush brush, double brushWidth, bool dashed = false)
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

            byte skip = 0;
            for (int i = 0; i <= longest; i++)
            {
                if (dashed)
                {
                    if (skip == 0)
                    {
                        DrawPixel(x1, y1, G, brush, brushWidth);
                        skip = 5;
                    }
                    else skip--;
                }
                else DrawPixel(x1, y1, G, brush, brushWidth);

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