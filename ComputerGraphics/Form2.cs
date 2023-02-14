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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ComputerGraphics
{
    public partial class Form2 : Form
    {
        private int width;
        private int height;

        private Graphics G;

        private Brush blackbrush = new SolidBrush(Color.Black);
        private Brush redbrush = new SolidBrush(Color.Red);

        private double coordBrushWidth = 1;
        private double linesBrushWidth = 2;

        private List<TextBox> textBoxes;

        public Form2()
        {
            InitializeComponent();

            button1.Click += DrawBezierCurve;

            width = pictureBox1.Width;
            height = pictureBox1.Height;

            G = pictureBox1.CreateGraphics();
            G.TranslateTransform(width / 2, height / 2);
            G.ScaleTransform(1, -1);

            textBoxes = new List<TextBox>();

            for (int i = 1; i <= 40; i++)
            {
                string name = "textBox" + i;
                textBoxes.Add((TextBox)(Controls.Find(name, false)[0]));
            }
        }

        private List<GeomPoint> ReadCurvePoints()
        {
            List<GeomPoint> curvePoints = new List<GeomPoint>();

            for (int i = 0; i < textBoxes.Count; i += 2)
            {
                TextBox tb1 = textBoxes[i];
                TextBox tb2 = textBoxes[i + 1];

                try
                {
                    if (tb1.Text != "" && tb2.Text != "")
                    {
                        double x = Convert.ToDouble(tb1.Text);
                        double y = Convert.ToDouble(tb2.Text);
                        curvePoints.Add(new GeomPoint(x, y));
                    }
                }
                catch (Exception)
                {
                    ErrorMesssage("Ошибка чтения координат!");
                }
            }

            return curvePoints;
        }

        private void DrawCoordinates(Graphics G)
        {
            DrawingUtil.DrawLineBresenham(new GeomPoint(0, -height / 2), new GeomPoint(0, height / 2), G, blackbrush, coordBrushWidth);
            DrawingUtil.DrawLineBresenham(new GeomPoint(-width / 2, 0), new GeomPoint(width / 2, 0), G, blackbrush, coordBrushWidth);
        }
        private void DrawBezierCurve(object sender, EventArgs e)
        {
            ErrorMesssage("");

            G.Clear(Color.White);

            DrawCoordinates(G);

            List<GeomPoint> curvePoints = ReadCurvePoints();

            DrawShapeLine(curvePoints);

            List<GeomPoint> segmentedPoints = SegmentPoints(curvePoints);

            DrawBezier(segmentedPoints);
        }

        private static GeomPoint P(double t, List<GeomPoint> points)
        {
            double x = Math.Pow(1 - t, 3) * points[0].x + 3 * Math.Pow(1 - t, 2) * t * points[1].x + 3 * (1 - t) * Math.Pow(t, 2) * points[2].x + Math.Pow(t, 3) * points[3].x;
            double y = Math.Pow(1 - t, 3) * points[0].y + 3 * Math.Pow(1 - t, 2) * t * points[1].y + 3 * (1 - t) * Math.Pow(t, 2) * points[2].y + Math.Pow(t, 3) * points[3].y;
            return new GeomPoint(x, y);
        }


        private void DrawShapeLine(List<GeomPoint> curvePoints)
        {
            for(int i = 1; i < curvePoints.Count; i++)
            {
                DrawingUtil.DrawLineBresenham(curvePoints[i-1], curvePoints[i], G, blackbrush, linesBrushWidth);
            }
        }
        private void DrawBezier(List<GeomPoint> curvePoints)
        {
            int counter = 0;

            while (counter < curvePoints.Count - 1)
            {
                List<GeomPoint> currPoints = curvePoints.GetRange(counter, 4);
                
                for (int i = 0; i <= 150; i++)
                {
                    double t = i / 150.0;

                    DrawingUtil.DrawPixel(P(t, currPoints), G, redbrush, 2f);
                }

                counter += 3;
            }
        }

        private List<GeomPoint> SegmentPoints(List<GeomPoint> curvePoints)
        {
            List<GeomPoint> segmentedPoins = new List<GeomPoint>(curvePoints);

            List<int> insertIndexs = new List<int>();

            int counter = 2;
            int length = curvePoints.Count;

            while (counter < length)
            {
                // 1я точка вокруг разделитлеьной
                if (counter == length - 1)
                {
                    segmentedPoins.Add(curvePoints[counter]);
                    break;
                }

                counter++;

                // 2я точка вокруг разделитлеьной             
                if (counter == length - 1)
                {
                    break;
                }
                insertIndexs.Add(counter);

                counter++;
            }

            for (int i = insertIndexs.Count - 1; i >= 0; i--)
            {
                int index = insertIndexs[i];
                segmentedPoins.Insert(index, (curvePoints[index] + curvePoints[index - 1]) * 0.5);
            }

            return segmentedPoins;
        }

        private void ErrorMesssage(string error)
        {
            label4.Text = error;
        }
    }
}