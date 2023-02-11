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

        private Pen linesPen = new Pen(Color.Black);
        private Pen pointPen = new Pen(Color.Red);

        private Brush fillbrush = new SolidBrush(Color.Green);

        private Graphics G;
        public Form1()
        {
            InitializeComponent();

            button1.Click += DrawTangent;

            linesPen.Width = 1;
            pointPen.Width = 2;


            width = pictureBox1.Width;
            height = pictureBox1.Height;

            G = pictureBox1.CreateGraphics();
            G.TranslateTransform(width / 2, height / 2);
            G.ScaleTransform(1, -1);
        }


        private void DrawCoordinates(Graphics G)
        {
            DrawingUtil.DrawLine(new GeomPoint(0, -height / 2), new GeomPoint(0, height / 2), G, linesPen);
            DrawingUtil.DrawLine(new GeomPoint(-width / 2, 0), new GeomPoint(width / 2, 0), G, linesPen);
        }

        private void DrawTangent(object sender, EventArgs e)
        {
            G.Clear(Color.White);

            DrawCoordinates(G);

            ErrorMessage("");

            try
            {
                Circle circle = new Circle(new GeomPoint(double.Parse(textBox1.Text), double.Parse(textBox2.Text)), double.Parse(textBox3.Text));

                DrawingUtil.DrawEllipse(circle, G, linesPen);


                GeomPoint targetPoint = new GeomPoint(double.Parse(textBox5.Text), double.Parse(textBox4.Text));

                if (Circle.IfPointInsideCircle(targetPoint, circle))
                {
                    ClearResultPoints();
                    throw new ArgumentException("Точка не может находиться внутри круга!");
                }

                GeomPoint[] tangentPoints = Circle.GetTangentPoints(circle, targetPoint);

                DrawingUtil.DrawLine(tangentPoints[0], targetPoint, G, pointPen);

                DrawingUtil.DrawLine(tangentPoints[1], targetPoint, G, pointPen);

                DrawResultPoints(tangentPoints[0], tangentPoints[1]);

                DrawingUtil.FillEllipse(new Circle(tangentPoints[0], 3), G, fillbrush);
                DrawingUtil.FillEllipse(new Circle(tangentPoints[1], 3), G, fillbrush);
                DrawingUtil.FillEllipse(new Circle(targetPoint, 3), G, fillbrush);
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

        private void DrawResultPoints(GeomPoint point1, GeomPoint point2)
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