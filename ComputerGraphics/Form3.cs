using GeometricStructures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerGraphics
{
    public partial class Form3 : Form
    {
        private int width;
        private int height;

        private Graphics G;

        private Brush blackbrush = new SolidBrush(Color.Black);

        private double coordBrushWidth = 1;
        private double linesBrushWidth = 1;

        public Form3()
        {
            InitializeComponent();

            button1.Click += DrawSurface;

            width = pictureBox1.Width;
            height = pictureBox1.Height;

            G = pictureBox1.CreateGraphics();
            G.TranslateTransform(width / 2, height / 2);
            G.ScaleTransform(1, -1);
        }
        private void DrawCoordinates(Graphics G)
        {
            DrawingUtil.DrawLineBresenham(new GeomPoint(0, -height / 2), new GeomPoint(0, height / 2), G, blackbrush, coordBrushWidth);
            DrawingUtil.DrawLineBresenham(new GeomPoint(-width / 2, 0), new GeomPoint(width / 2, 0), G, blackbrush, coordBrushWidth);
        }

        private List<GeomPoint> ReadFormingPoints()
        {
            List<GeomPoint> points = new List<GeomPoint>();

            try
            {
                points.Add(new GeomPoint(Int32.Parse(textBox1.Text), Int32.Parse(textBox2.Text)));
                points.Add(new GeomPoint(Int32.Parse(textBox3.Text), Int32.Parse(textBox4.Text)));
                points.Add(new GeomPoint(Int32.Parse(textBox5.Text), Int32.Parse(textBox6.Text)));
                points.Add(new GeomPoint(Int32.Parse(textBox7.Text), Int32.Parse(textBox8.Text)));
            } catch (Exception) { }

            return points;
        }

        private void DrawSurface(object sender, EventArgs e)
        {
            G.Clear(Color.White);

            DrawCoordinates(G);

            List<GeomPoint> formingPoints = ReadFormingPoints();

            double step = 0.1;

            for (double c1 = 0; c1 <= 1; c1 += step)
            {
                GeomPoint point1 = BilinearInterpolation(formingPoints, 0, c1);
                GeomPoint point2 = BilinearInterpolation(formingPoints, 1, c1);
                DrawingUtil.DrawLineBresenham(point1, point2, G, blackbrush, linesBrushWidth);
            }

            for (double c2 = 0; c2 <= 1; c2 += step)
            {
                GeomPoint point1 = BilinearInterpolation(formingPoints, c2, 0);
                GeomPoint point2 = BilinearInterpolation(formingPoints, c2, 1);
                DrawingUtil.DrawLineBresenham(point1, point2, G, blackbrush, linesBrushWidth);
            }
        }

        private GeomPoint BilinearInterpolation(List<GeomPoint> formingPoints, double u, double v)
        {
            return (1 - u) * (1 - v) * formingPoints[0] + u * (1 - v) * formingPoints[1] + (1 - u) * v * formingPoints[2] + u * v * formingPoints[3];
        }
    }
}