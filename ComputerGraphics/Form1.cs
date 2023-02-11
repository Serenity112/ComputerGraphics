using GeometricStructures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ComputerGraphics
{
    public partial class Form1 : Form
    {
        private GeomPoint coordinatesCenter;

        private int width;
        private int height;


        private Pen linesPen;
        private Pen pointPen;

        Graphics G;
        public Form1()
        {
            InitializeComponent();

            button1.Click += DrawTangent;

            linesPen = new Pen(Color.Black);
            linesPen.Width = 1;

            pointPen = new Pen(Color.Red);
            pointPen.Width = 2;

            width = pictureBox1.Width;
            height = pictureBox1.Height;

            coordinatesCenter = new GeomPoint(width / 2, height / 2);

            G = pictureBox1.CreateGraphics();
        }


        private void DrawCoordinates(Graphics G)
        {
            DrawLine(new GeomPoint(0, -height / 2), new GeomPoint(0, height / 2));
            DrawLine(new GeomPoint(-width / 2, 0), new GeomPoint(width / 2, 0));
        }

        private void DrawTangent(object sender, EventArgs e)
        {
            G.Clear(Color.White);

            DrawCoordinates(G);

            try
            {
                float circle_x0 = float.Parse(textBox1.Text);
                float circle_y0 = float.Parse(textBox2.Text);
                float radius = float.Parse(textBox3.Text);

                Circle circle = new Circle(new GeomPoint(circle_x0, circle_y0), radius);

                float[] patametrs = CoordinatesUtil.ellipseToWFFormat(circle, coordinatesCenter);

                GeomPoint targetPoint = new GeomPoint(float.Parse(textBox5.Text), float.Parse(textBox4.Text));

                if (CircleUtils.IfPointInsideCircle(targetPoint, circle))
                {
                    throw new ArgumentException("Точка не может находиться внутри круга!");
                }

                G.DrawEllipse(linesPen, patametrs[0], patametrs[1], patametrs[2], patametrs[3]);


                GeomPoint[] tangentPoints = CircleUtils.GetTangentPointCoordinate(circle, targetPoint);

                DrawLine(tangentPoints[0], targetPoint);

                DrawLine(tangentPoints[1], targetPoint);
            }
            catch (ArgumentException ex)
            {
                label13.Text = ex.Message;
            }
            catch (Exception ex)
            {
                label13.Text = "Ошибка ввода параметров!";
            }

        }

        private void DrawLine(GeomPoint point1, GeomPoint point2)
        {
            GeomPoint m_point1 = CoordinatesUtil.pointToWFFormat(point1, coordinatesCenter);
            GeomPoint m_point2 = CoordinatesUtil.pointToWFFormat(point2, coordinatesCenter);

            PointF f_point1 = new PointF(m_point1[0, 0], m_point1[1, 0]);
            PointF f_point2 = new PointF(m_point2[0, 0], m_point2[1, 0]);

            G.DrawLine(pointPen, f_point1, f_point2);
        }
    }
}
