using GeometricStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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

        private const int startPositionX = 1108, startPositionY = 209;
        private const int offsetX = 61, offsetY = 45;

        private List<TextBox> textBoxes = new List<TextBox>();

        public Form2()
        {
            InitializeComponent();

            button1.Click += DrawBezierCurve;

            width = pictureBox1.Width;
            height = pictureBox1.Height;

            G = pictureBox1.CreateGraphics();
            G.TranslateTransform(width / 2, height / 2);
            G.ScaleTransform(1, -1);

            InitDefaultTextBoxes(pointsCount);
        }

        // Инициализирует начальные поля координаты точки кривой Безье
        private void InitDefaultTextBoxes(Control pointsCountControl)
        {
            string[] defaultCoords = { "-500", "50", "-400", "70", "-300", "-100", "-120", "100", "0", "0",
                "100", "-140", "200", "-10", "300", "-20", "400", "-100", "500", "100" };

            for (int i = 0; i < 10; i++)
            {
                CreateCoordinatesTextBox("coordX" + i, new Point(startPositionX, startPositionY + i * offsetY), defaultCoords[2 * i]);
                CreateCoordinatesTextBox("coordY" + i, new Point(startPositionX + offsetX, startPositionY + i * offsetY), defaultCoords[2 * i + 1]);
            }
        }

        private List<GeomPoint> ReadCurvePoints()
        {
            List<GeomPoint> curvePoints = new List<GeomPoint>();

            for (int i = 0; i < textBoxes.Count; i += 2)
            {
                TextBox tb1 = textBoxes[i], tb2 = textBoxes[i + 1];
                if (tb1.Text == "" || tb2.Text == "")
                {
                    throw new Exception("Не все поля заполнены!");
                }

                try
                {
                    double x = Convert.ToDouble(tb1.Text), y = Convert.ToDouble(tb2.Text);
                    curvePoints.Add(new GeomPoint(x, y));
                }
                catch (Exception)
                {
                    ErrorMessage("Ошибка чтения координат");
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
            ErrorMessage("");

            G.Clear(Color.White);

            G.ScaleTransform(1, -1);
            DrawingUtil.DrawCoordinates(G, width, height, blackbrush, 50);
            G.ScaleTransform(1, -1);

            try
            {
                List<GeomPoint> curvePoints = ReadCurvePoints();

                DrawShapeLine(curvePoints);

                List<GeomPoint> segmentedPoints = SegmentPoints(curvePoints);

                DrawBezier(segmentedPoints);

            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }

        }

        private static GeomPoint P(double t, List<GeomPoint> points)
        {
            double x = Math.Pow(1 - t, 3) * points[0].x + 3 * Math.Pow(1 - t, 2) * t * points[1].x + 3 * (1 - t) * Math.Pow(t, 2) * points[2].x + Math.Pow(t, 3) * points[3].x;
            double y = Math.Pow(1 - t, 3) * points[0].y + 3 * Math.Pow(1 - t, 2) * t * points[1].y + 3 * (1 - t) * Math.Pow(t, 2) * points[2].y + Math.Pow(t, 3) * points[3].y;
            return new GeomPoint(x, y);
        }

        private void DrawShapeLine(List<GeomPoint> curvePoints)
        {
            for (int i = 1; i < curvePoints.Count; i++)
            {
                DrawingUtil.DrawLineBresenham(curvePoints[i - 1], curvePoints[i], G, blackbrush, linesBrushWidth);
            }
        }

        private void DrawBezier(List<GeomPoint> curvePoints)
        {
            int counter = 0;

            while (counter < curvePoints.Count - 1)
            {
                List<GeomPoint> currPoints = curvePoints.GetRange(counter, 4);

                double accuracy = 0;

                for (int i = 1; i < 4; i++)
                {
                    accuracy += currPoints[i - 1].distance(currPoints[i]);
                }

                for (int i = 0; i <= accuracy; i++)
                {
                    double t = i / accuracy;

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

        private void ErrorMessage(string error)
        {
            label4.Text = error;
        }

        // Динамически создает поле одной координаты точки для кривой Безье
        private void CreateCoordinatesTextBox(string name, Point position, string text = "")
        {
            TextBox coordTextBox = new TextBox()
            {
                Name = name,
                Location = new Point(position.X, position.Y),
                Size = new Size(55, 29),
                Text = text
            };
            textBoxes.Add(coordTextBox);
            Controls.Add(coordTextBox);
        }

        // Удаляет последние два поля, представляющие координату точки
        private void RemoveLastCoordinatesTextBox()
        {
            Controls.Remove(textBoxes[textBoxes.Count - 1]);
            Controls.Remove(textBoxes[textBoxes.Count - 2]);
            textBoxes.RemoveRange(textBoxes.Count - 2, 2);
        }

        // Вызывается, когда значение поля [pointsCount] изменяется
        private void pointsCount_ValueChanged(object sender, EventArgs e)
        {
            int initialCount = textBoxes.Count / 2;
            int difference = (int)pointsCount.Value - initialCount;
            
            // Если разница в значении и в количестве координат меньше нуля, то лишнии координаты удаляются
            if (difference < 0)
            {
                for (int i = 0; i < -difference; i++)
                {
                    RemoveLastCoordinatesTextBox();
                }
            }
            // Если разница больше нуля, добавляются необходимые поля с координатами
            else
            {
                for (int i = 0; i < difference; i++)
                {
                    int order = (initialCount + i);
                    CreateCoordinatesTextBox("coordX" + order, new Point(startPositionX, startPositionY + offsetY * order));
                    CreateCoordinatesTextBox("coordY" + order, new Point(startPositionX + offsetX, startPositionY + offsetY * order));
                }
            }
        }
    }
}