using GeometricStructures;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerGraphics
{
    public partial class Form1 : Form
    {
        private int width;
        private int height;

        private Brush greenBrush = new SolidBrush(Color.Green);
        private Brush blackbrush = new SolidBrush(Color.Black);
        private Brush redbrush = new SolidBrush(Color.Red);

        private double coordBrushWidth = 1;
        private double linesBrushWidth = 2;
        private double pointBrushWidth = 5;

        private Graphics G;
        public Form1()
        {
            InitializeComponent();

            button1.Click += DrawTangent;

            width = pictureBox1.Width;
            height = pictureBox1.Height;

            G = pictureBox1.CreateGraphics();
            G.TranslateTransform(width / 2, height / 2);
            G.ScaleTransform(1, -1);

        }

        private void DrawTangent(object sender, EventArgs e)
        {
            G.Clear(Color.White);

            G.ScaleTransform(1, -1);
            DrawingUtil.DrawCoordinates(G, width, height, blackbrush, 50);
            G.ScaleTransform(1, -1);

            ErrorMessage("");

            try
            {
                Circle circle = new Circle(new GeomPoint(double.Parse(textBox1.Text), double.Parse(textBox2.Text)), double.Parse(textBox3.Text));
                DrawingUtil.DrawCircleBresenham(circle, G, blackbrush, linesBrushWidth);

                GeomPoint targetPoint = new GeomPoint(double.Parse(textBox5.Text), double.Parse(textBox4.Text));

                if (Circle.IfPointInsideCircle(targetPoint, circle))
                {
                    DrawingUtil.DrawPixel(targetPoint, G, greenBrush, pointBrushWidth);
                    ClearResultPoints();
                    throw new ArgumentException("Точка не может находиться внутри круга!");
                }

                GeomPoint[] tangentPoints = Circle.GetTangentPoints(circle, targetPoint);
                DrawingUtil.DrawLineBresenham(tangentPoints[0], targetPoint, G, redbrush, linesBrushWidth);
                DrawingUtil.DrawLineBresenham(tangentPoints[1], targetPoint, G, redbrush, linesBrushWidth);

                PrintResultPoints(tangentPoints[0], tangentPoints[1]);

                DrawingUtil.DrawPixel(tangentPoints[0], G, greenBrush, pointBrushWidth);
                DrawingUtil.DrawPixel(tangentPoints[1], G, greenBrush, pointBrushWidth);
                DrawingUtil.DrawPixel(targetPoint, G, greenBrush, pointBrushWidth);

            }
            catch (ArgumentException ex)
            {
                ErrorMessage(ex.Message);
            }
            catch (Exception)
            {
                ErrorMessage("Ошибка ввода параметров!");
            }

        }

        private void PrintResultPoints(GeomPoint point1, GeomPoint point2)
        {
            label14.Text = $"X1: {point1.x:0.000}";
            label15.Text = $"Y1: {point1.y:0.000}";

            label21.Text = $"X2: {point2.x:0.000}";
            label20.Text = $"Y2: {point2.y:0.000}";
        }

        private void ClearResultPoints()
        {
            label14.Text = "";
            label15.Text = "";

            label21.Text = "";
            label20.Text = "";
        }

        private void ErrorMessage(string error)
        {
            label13.Text = error;
        }
    }
}