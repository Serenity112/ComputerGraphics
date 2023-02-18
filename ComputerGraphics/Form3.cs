using GeometricStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerGraphics
{
    public partial class Form3 : Form
    {
        private enum Axes
        {
            Ox,
            Oy,
            Oz,
        }
        private enum RotationSide
        {
            Right,
            Left,
        }

        private int width;
        private int height;

        private Graphics G;

        private Brush blackbrush = new SolidBrush(Color.Black);
        private Brush blueBrush = new SolidBrush(Color.Blue);
        private Brush redBrush = new SolidBrush(Color.Red);

        private double coordBrushWidth = 1;
        private double linesBrushWidth = 1;

        private List<GeomPoint> currentSurfacePoints;

        public Form3()
        {
            InitializeComponent();

            button1.Click += button1_Click;
            button2.Click += rotOXleft;
            button3.Click += rotOXright;
            button4.Click += rotOYleft;
            button5.Click += rotOYright;

            width = pictureBox1.Width;
            height = pictureBox1.Height;

            G = pictureBox1.CreateGraphics();
            G.TranslateTransform(width / 2, height / 2);
            G.ScaleTransform(1, -1);
        }

        private List<GeomPoint> ReadFormingPoints()
        {
            List<GeomPoint> points = new List<GeomPoint>();

            try
            {
                points.Add(new GeomPoint(Int32.Parse(textBox1.Text), Int32.Parse(textBox2.Text), Int32.Parse(textBox3.Text)));
                points.Add(new GeomPoint(Int32.Parse(textBox4.Text), Int32.Parse(textBox5.Text), Int32.Parse(textBox6.Text)));
                points.Add(new GeomPoint(Int32.Parse(textBox7.Text), Int32.Parse(textBox8.Text), Int32.Parse(textBox9.Text)));
                points.Add(new GeomPoint(Int32.Parse(textBox10.Text), Int32.Parse(textBox11.Text), Int32.Parse(textBox12.Text)));
            }
            catch (Exception) { }

            return points;
        }

        private List<GeomPoint> ShiftPerspective(List<GeomPoint> formingPoints)
        {
            List<GeomPoint> shifted_points = new List<GeomPoint>();

            GeomPoint VectorZoffset = new GeomPoint(-1, -1);

            foreach (GeomPoint point in formingPoints)
            {
                float old_X = (float)point.x;
                float old_Y = (float)point.y;
                float old_Z = (float)point.z;

                float new_X = (float)(point.x + point.z * VectorZoffset.x);
                float new_Y = (float)(point.y + point.z * VectorZoffset.y);

                shifted_points.Add(new GeomPoint(new_X, new_Y, old_Z));
            }

            return shifted_points;
        }

        private void drawFormingPoints(List<GeomPoint> formingPoints, List<GeomPoint> shiftedPoints)
        {
            for(int i = 0; i < formingPoints.Count; i++)
            {
                float old_X = (float)formingPoints[i].x;
                float old_Y = (float)formingPoints[i].y;
                float old_Z = (float)formingPoints[i].z;

                float new_X = (float)shiftedPoints[i].x;
                float new_Y = (float)shiftedPoints[i].y;

                G.DrawString($"{old_X:0.00}" + " " + $"{old_Y:0.00}" + " " + $"{old_Z:0.00}", this.Font, redBrush, new_X, -new_Y);
            }    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            G.Clear(Color.White);

            G.ScaleTransform(1, -1);

            DrawingUtil.DrawCoordinates3D(G, width, height, blackbrush, 50);

            currentSurfacePoints = ReadFormingPoints();

            List<GeomPoint> shiftedPoints = ShiftPerspective(currentSurfacePoints);

            drawFormingPoints(currentSurfacePoints, shiftedPoints);

            G.ScaleTransform(1, -1);

            DrawSurface(shiftedPoints);
        }

        private void DrawSurface(List<GeomPoint> formingPoints)
        {
            double step = 0.1;

            for (double c1 = 0; c1 <= 1; c1 += step)
            {
                GeomPoint point1 = BilinearInterpolation(formingPoints, 0, c1);
                GeomPoint point2 = BilinearInterpolation(formingPoints, 1, c1);
                DrawingUtil.DrawLineBresenham(point1, point2, G, blueBrush, linesBrushWidth);
            }

            for (double c2 = 0; c2 <= 1; c2 += step)
            {
                GeomPoint point1 = BilinearInterpolation(formingPoints, c2, 0);
                GeomPoint point2 = BilinearInterpolation(formingPoints, c2, 1);
                DrawingUtil.DrawLineBresenham(point1, point2, G, blueBrush, linesBrushWidth);
            }
        }

        private GeomPoint BilinearInterpolation(List<GeomPoint> formingPoints, double u, double v)
        {
            return (1 - u) * (1 - v) * formingPoints[0] + u * (1 - v) * formingPoints[1] + (1 - u) * v * formingPoints[2] + u * v * formingPoints[3];
        }

        private void rotOXright(object sender, EventArgs e)
        {
            rotateSurface(Axes.Ox, RotationSide.Right);
        }

        private void rotOXleft(object sender, EventArgs e)
        {
            rotateSurface(Axes.Ox, RotationSide.Left);
        }

        private void rotOYright(object sender, EventArgs e)
        {
            rotateSurface(Axes.Oy, RotationSide.Right);
        }

        private void rotOYleft(object sender, EventArgs e)
        {
            rotateSurface(Axes.Oy, RotationSide.Left);
        }

        private void rotateSurface(Axes axis, RotationSide side)
        {
            // Дефолт значения
            Matrix rotationMatrix = new Matrix(1, 1);
            double angle = 0;

            switch (axis)
            {
                case Axes.Ox:
                    angle = (Convert.ToDouble(textBox13.Text) / 180) * Math.PI;

                    if (side == RotationSide.Left)
                    {
                        angle = -angle;
                    }

                    rotationMatrix = new Matrix(new double[,] {
                        { 1, 0, 0 },
                        { 0, Math.Cos(angle), -Math.Sin(angle) },
                        { 0, Math.Sin(angle), Math.Cos(angle) }
                    });
                    break;
                case Axes.Oy:
                    angle = (Convert.ToDouble(textBox14.Text) / 180) * Math.PI;

                    if (side == RotationSide.Left)
                    {
                        angle = -angle;
                    }

                    rotationMatrix = new Matrix(new double[,] {
                        { Math.Cos(angle), 0, Math.Sin(angle) },
                        { 0, 1, 0 },
                        { -Math.Sin(angle), 0, Math.Cos(angle) }
                    });

                    break;
            }

            for (int i = 0; i < currentSurfacePoints.Count; i++)
            {
                currentSurfacePoints[i] = rotationMatrix * currentSurfacePoints[i];
            }

            G.Clear(Color.White);

            G.ScaleTransform(1, -1);

            DrawingUtil.DrawCoordinates3D(G, width, height, blackbrush, 50);

            List<GeomPoint> shiftedPoints = ShiftPerspective(currentSurfacePoints);

            drawFormingPoints(currentSurfacePoints, shiftedPoints);

            G.ScaleTransform(1, -1);

            DrawSurface(shiftedPoints);
        }
    }
}