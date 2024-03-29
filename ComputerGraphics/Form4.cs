﻿using GeometricStructures;
using System;
using System.Collections.Generic;
using System.Drawing;

using System.Windows.Forms;

namespace ComputerGraphics
{
    public partial class Form4 : Form
    {
        private readonly int _width;
        private readonly int _height;

        private readonly double _linesBrushWidth = 1;
        private readonly double _windowWidth = 1;

        private Graphics _graphics;

        private Brush _blackBrush = new SolidBrush(Color.Black);
        private Brush _blueBrush = new SolidBrush(Color.Blue);
        private Brush _greenBrush = new SolidBrush(Color.Green);
        private Brush _redBrush = new SolidBrush(Color.Red);
        private Brush _purpleBrush = new SolidBrush(Color.Purple);

        private List<GeomPoint[]> _segmentPoints = new List<GeomPoint[]>();
        private List<GeomPoint> _windowPoints = new List<GeomPoint>();

        double x_left;
        double x_right;
        double y_bottom;
        double y_top;

        public Form4()
        {
            InitializeComponent();

            button1.Click += Button1_Click;

            _width = pictureBox1.Width;
            _height = pictureBox1.Height;

            _graphics = pictureBox1.CreateGraphics();
            _graphics.TranslateTransform(_width / 2, _height / 2);
            _graphics.ScaleTransform(1, -1);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            _graphics.Clear(Color.White);

            _graphics.ScaleTransform(1, -1);

            DrawingUtil.DrawCoordinates2D(_graphics, _width, _height, _blackBrush, 50);

            _graphics.ScaleTransform(1, -1);

            DrawWindow();

            GenerateSegments();

            DrawVisibleSegments();
        }

        private void DrawWindow()
        {
            _windowPoints = new List<GeomPoint>
            {
                new GeomPoint(Double.Parse(textBox1.Text), Double.Parse(textBox2.Text)), // Left top
                new GeomPoint(Double.Parse(textBox3.Text), Double.Parse(textBox2.Text)), // Right top
                new GeomPoint(Double.Parse(textBox3.Text), Double.Parse(textBox4.Text)), // Right bottom
                new GeomPoint(Double.Parse(textBox1.Text), Double.Parse(textBox4.Text)) // Left buttom
            };

            for (int i = 0; i < 4; i++)
            {
                DrawingUtil.DrawLineBresenham(_windowPoints[i], _windowPoints[(i + 1) % 4], _graphics, _blueBrush, _windowWidth);
            }
        }

        private void GenerateSegments()
        {
            _segmentPoints = new List<GeomPoint[]>();

            int segmentsNum = Int32.Parse(textBox5.Text);

            Random rand = new Random();

            for (int i = 0; i < segmentsNum; i++)
            {
                double x1 = _width / 2 * rand.NextDouble() * Math.Pow(-1, rand.Next(2));
                double y1 = _height / 2 * rand.NextDouble() * Math.Pow(-1, rand.Next(2));

                double x2 = _width / 2 * rand.NextDouble() * Math.Pow(-1, rand.Next(2));
                double y2 = _height / 2 * rand.NextDouble() * Math.Pow(-1, rand.Next(2));

                GeomPoint p1 = new GeomPoint(x1, y1);
                GeomPoint p2 = new GeomPoint(x2, y2);

                _segmentPoints.Add(new GeomPoint[] { p1, p2 });

                DrawingUtil.DrawLineBresenham(p1, p2, _graphics, _redBrush, _linesBrushWidth);
            }
        }

        private bool IfInsideWindow(GeomPoint point)
        {
            return point.x > x_left && point.x < x_right && point.y > y_bottom && point.y < y_top;
        }

        private void DrawVisibleSegments()
        {
            GeomPoint wp0 = _windowPoints[0];
            GeomPoint wp2 = _windowPoints[2];

            x_left = wp0.x < wp2.x ? wp0.x : wp2.x;
            x_right = wp0.x < wp2.x ? wp2.x : wp0.x;
            y_bottom = wp0.y < wp2.y ? wp0.y : wp2.y;
            y_top = wp0.y < wp2.y ? wp2.y : wp0.y;

            foreach (var points in _segmentPoints)
            {
                GeomPoint p1 = points[0];
                GeomPoint p2 = points[1];

                // Тривиально невидимы
                if (p1.x < x_left && p2.x < x_left ||
                p1.x > x_right && p2.x > x_right ||
                p1.y < y_bottom && p2.y < y_bottom ||
                p1.y > y_top && p2.y > y_top)
                {
                    continue;
                }

                // Тривиально видимы
                if (IfInsideWindow(p1) && IfInsideWindow(p2))
                {
                    DrawingUtil.DrawLineBresenham(p1, p2, _graphics, _purpleBrush, 2);
                    continue;
                }

                // Крайние случаи
                // Вертикаль
                if (p1.x == p2.x)
                {
                    double y1 = IfInsideWindow(p1) ? p1.y : p1.y > y_top ? y_top : y_bottom;
                    double y2 = IfInsideWindow(p2) ? p2.y : p2.y > y_top ? y_top : y_bottom;
                    DrawingUtil.DrawLineBresenham(new GeomPoint(p1.x, y1), new GeomPoint(p1.x, y2), _graphics, _purpleBrush, 2);
                    continue;
                }

                // Горизонталь
                if (p1.y == p2.y)
                {
                    double x1 = IfInsideWindow(p1) ? p1.x : p1.x < x_left ? x_left : x_right;
                    double x2 = IfInsideWindow(p2) ? p2.x : p2.x < x_left ? x_left : x_right;
                    DrawingUtil.DrawLineBresenham(new GeomPoint(x1, p1.y), new GeomPoint(x2, p1.y), _graphics, _purpleBrush, 2);
                    continue;
                }

                // Определение нетривиальной видимости
                double t1 = (x_left - p1.x) / (p2.x - p1.x);
                GeomPoint pt1 = p1 + (p2 - p1) * t1;

                double t2 = (x_right - p1.x) / (p2.x - p1.x);
                GeomPoint pt2 = p1 + (p2 - p1) * t2;

                double t3 = (y_bottom - p1.y) / (p2.y - p1.y);
                GeomPoint pt3 = p1 + (p2 - p1) * t3;

                double t4 = (y_top - p1.y) / (p2.y - p1.y);
                GeomPoint pt4 = p1 + (p2 - p1) * t4;

                List<GeomPoint> intersections = new List<GeomPoint>();

                if (pt1.y >= y_bottom && pt1.y <= y_top && t1 >= 0 && t1 <= 1 && !intersections.Contains(pt1))
                    intersections.Add(pt1);

                if (pt2.y >= y_bottom && pt2.y <= y_top && t2 >= 0 && t2 <= 1 && !intersections.Contains(pt2))
                    intersections.Add(pt2);

                if (pt3.x <= x_right && pt3.x >= x_left && t3 >= 0 && t3 <= 1 && !intersections.Contains(pt3))
                    intersections.Add(pt3);

                if (pt4.x <= x_right && pt4.x >= x_left && t4 >= 0 && t4 <= 1 && !intersections.Contains(pt4))
                    intersections.Add(pt4);

                // Нетривиальная невидимость
                if (intersections.Count == 0)
                {
                    continue;
                }

                // Имеет 1 пересечение с окном
                if (intersections.Count == 1)
                {
                    GeomPoint innerPoint = IfInsideWindow(p1) ? p1 : p2;
                    DrawingUtil.DrawLineBresenham(intersections[0], innerPoint, _graphics, _purpleBrush, 2);
                    continue;
                }

                // Имеет 2 пересечения с окном
                if (intersections.Count == 2)
                {
                    DrawingUtil.DrawLineBresenham(intersections[0], intersections[1], _graphics, _purpleBrush, 2);
                    continue;
                }
            }
        }
    }
}
