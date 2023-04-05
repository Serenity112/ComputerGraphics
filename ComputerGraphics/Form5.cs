using GeometricStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerGraphics
{
    using AdjacencyList = Dictionary<GeomPoint, List<GeomPoint>>;

    public partial class Form5 : Form
    {
        private readonly int _width;
        private readonly int _height;

        private readonly double _linesBrushWidth = 1;
        private const double _nearSurface = 0.5;
        private const double _degToRad = Math.PI / 180;
        private double _fieldOfView;

        private Graphics _graphics;

        private Brush _blackBrush = new SolidBrush(Color.Black);
        private Brush _blueBrush = new SolidBrush(Color.Blue);
        private Brush _greenBrush = new SolidBrush(Color.Green);
        private Brush _redBrush = new SolidBrush(Color.Red);
        private Brush _purpleBrush = new SolidBrush(Color.Purple);

        private GeomPoint _spectatorPosition;
        private readonly Matrix _initViewRotation = Matrix.Identity(3);
        private Matrix _currentViewRotation;
        private GeomPoint _forwardDirection;
        private readonly AdjacencyList _graph;

        public Form5()
        {
            InitializeComponent();

            _width = pictureBox1.Width;
            _height = pictureBox1.Height;

            _graphics = pictureBox1.CreateGraphics();
            _graphics.TranslateTransform(_width / 2, _height / 2);
            _graphics.ScaleTransform(1, -1);

            _graph = new AdjacencyList();

            InitializeCube();
        }

        // Initializing 3D cube graph with vertices and edges
        private void InitializeCube()
        {
            GeomPoint[] points =
            {
                // lower surface
                new GeomPoint(1, 0, 2),
                new GeomPoint(1, 0, 4),
                new GeomPoint(-1, 0, 4),
                new GeomPoint(-1, 0, 2),
                // upper surface
                new GeomPoint(1, 2, 2),
                new GeomPoint(1, 2, 4),
                new GeomPoint(-1, 2, 4),
                new GeomPoint(-1, 2, 2),
            };

            _graph.Add(points[0], new List<GeomPoint>() { points[1], points[3], points[4] });
            _graph.Add(points[1], new List<GeomPoint>() { points[0], points[2], points[5] });
            _graph.Add(points[2], new List<GeomPoint>() { points[1], points[3], points[6] });
            _graph.Add(points[3], new List<GeomPoint>() { points[0], points[2], points[7] });
            _graph.Add(points[4], new List<GeomPoint>() { points[5], points[7], points[0] });
            _graph.Add(points[5], new List<GeomPoint>() { points[4], points[6], points[1] });
            _graph.Add(points[6], new List<GeomPoint>() { points[5], points[7], points[2] });
            _graph.Add(points[7], new List<GeomPoint>() { points[4], points[6], points[3] });
        }

        // Initializing spectator point and view direction
        private void InitializeSpectator()
        {
            _fieldOfView = Convert.ToDouble(fieldOfView.Text);
            _spectatorPosition = new GeomPoint(
                    Convert.ToDouble(spectatorX.Text),
                    Convert.ToDouble(spectatorY.Text),
                    Convert.ToDouble(spectatorZ.Text)
                );

            double rotX = (double)viewRotationX.Value * _degToRad;
            double rotY = (double)viewRotationY.Value * _degToRad;
            double rotZ = (double)viewRotationZ.Value * _degToRad;

            _currentViewRotation = _initViewRotation * Matrix.RotationBy(rotX, rotY, rotZ);
            _forwardDirection = new GeomPoint(_currentViewRotation[0, 2], _currentViewRotation[1, 2], _currentViewRotation[2, 2]);
        }

        // Функция отбрасываения невидимых граней
        // Eще не реализована :)
        private void DeleteInvisibleSurfaces()
        {

        }

        // Определение дистанции до плоскости обзора
        private double? FindOrthogonalDistance(GeomPoint p)
        {
            double d = GeomPoint.ScalarProduct(_forwardDirection, p - _spectatorPosition) / GeomPoint.ScalarProduct(_forwardDirection, _forwardDirection);

            return (d >= _nearSurface) ? d : (double?)null; 
        }

        // Проекция куба на плоскости обзора
        private AdjacencyList Get2DProjection()
        {
            Dictionary<GeomPoint, GeomPoint> point3dTo2d = new Dictionary<GeomPoint, GeomPoint>();

            // Постоянный коэффициент удаления от точки обзора в зависимости от угла обзора
            double coeff = 2 * Math.Tan(_fieldOfView / 2 * _degToRad);

            foreach (GeomPoint vertex in _graph.Keys)
            {
                // Находим дистанцию до плоскости относительно точки
                double? d = FindOrthogonalDistance(vertex);
                if (d == null) continue;

                // Строим вектор из центра плоскости в сторону точки
                GeomPoint surfaceVector = vertex - d.Value * _forwardDirection - _spectatorPosition;

                // Определяем используемые строки для решения СЛАУ
                int kx = -1, ky = -1;
                for (int i = 0; i < 3; i++)
                {
                    if (_currentViewRotation[i, 0] != 0)
                    {
                        kx = 0;
                        break;
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    if (_currentViewRotation[i, 1] != 0 && i != kx)
                    {
                        ky = i;
                        break;
                    }
                }

                // Здесь происходит сжимающее отображение из трехмерного пространства в двухмерное
                // Определяем длины векторов в новом базисе - эти длины эквивалентны координатам X и Y на экране
                Matrix screenUnits = Matrix.SolveLinearEquation(new Matrix(new double[2, 2]
                {
                    { _currentViewRotation[kx, 0], _currentViewRotation[kx, 1] },
                    { _currentViewRotation[ky, 0], _currentViewRotation[ky, 1] }
                }), new Matrix(new double[2, 1] {
                    { ((Matrix)surfaceVector)[kx, 0] }, { ((Matrix)surfaceVector)[ky, 0] } }
                ));

                // Точка с учетом искажений перспективы
                GeomPoint screenPosition = new GeomPoint(screenUnits[kx, 0], screenUnits[ky, 0]) * (_width / (d.Value * coeff));

                point3dTo2d.Add(vertex, screenPosition);
            }

            AdjacencyList graph2d = new AdjacencyList();
            foreach (GeomPoint key in _graph.Keys)
            {
                graph2d.Add(point3dTo2d[key], new List<GeomPoint>());
                foreach (GeomPoint val in _graph[key])
                {
                    graph2d[point3dTo2d[key]].Add(point3dTo2d[val]);
                }
            }

            return graph2d;
        }

        // Отрисовка каждого ребра
        // Сейчас каждое ребро рисуется по два раза - нужно что то придумать
        private void Draw2DCube(AdjacencyList graph2d)
        {
            foreach (GeomPoint first in graph2d.Keys)
            {
                foreach(GeomPoint second in graph2d[first])
                {
                    DrawingUtil.DrawLineBresenham(first, second, _graphics, _redBrush, _linesBrushWidth);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _graphics.Clear(Color.White);
            InitializeSpectator();
            DeleteInvisibleSurfaces();
            Draw2DCube(Get2DProjection());
        }

        private void viewRotationX_ValueChanged(object sender, EventArgs e)
        {
            if (viewRotationX.Value < viewRotationX.Minimum + 1) viewRotationX.Value += 360;
            else if (viewRotationX.Value > viewRotationX.Maximum - 1) viewRotationX.Value -= 360;
        }

        private void viewRotationY_ValueChanged(object sender, EventArgs e)
        {
            if (viewRotationY.Value < viewRotationY.Minimum + 1) viewRotationY.Value += 360;
            else if (viewRotationY.Value > viewRotationY.Maximum - 1) viewRotationY.Value -= 360;
        }

        private void viewRotationZ_ValueChanged(object sender, EventArgs e)
        {
            if (viewRotationZ.Value < viewRotationZ.Minimum + 1) viewRotationZ.Value += 360;
            else if (viewRotationZ.Value > viewRotationZ.Maximum - 1) viewRotationZ.Value -= 360;
        }
    }
}
