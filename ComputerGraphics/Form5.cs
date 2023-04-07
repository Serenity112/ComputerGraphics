using GeometricStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerGraphics
{
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

        private VolumetricObject _cube;

        public Form5()
        {
            InitializeComponent();

            _width = pictureBox1.Width;
            _height = pictureBox1.Height;

            _graphics = pictureBox1.CreateGraphics();
            _graphics.TranslateTransform(_width / 2, _height / 2);
            _graphics.ScaleTransform(1, -1);
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

            List<Surface> surfaces = new List<Surface>()
            {
                new Surface(new List<int>() { 0, 1, 2, 3 }),
                new Surface(new List<int>() { 4, 5, 6, 7 }),
                new Surface (new List<int>() { 0, 1, 5, 4 }),
                new Surface (new List<int>() { 2, 3, 7, 6 }),
                new Surface (new List<int>() { 0, 3, 7, 4 }),
                new Surface (new List<int>() { 1, 2, 6, 5 })
            };

            _cube = new VolumetricObject(points, surfaces);
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

        // Функция отбрасываения невидимых ребер
        private void DeleteInvisibleSurfaces()
        {
            for (int i = 0; i < _cube.Surfaces.Count; i++)
            {
                int k = 0;
                Surface currentSurface = _cube.Surfaces[i];
                int surfacePoint1 = currentSurface[0];
                int surfacePoint2 = currentSurface[1];
                int surfacePoint3 = currentSurface[2];

                int randomPoint = _cube.Surfaces[(i + 1) % _cube.Surfaces.Count][k];
                while (currentSurface.Contains(randomPoint) )
                {
                    randomPoint = _cube.Surfaces[(i + 1) % _cube.Surfaces.Count][++k];
                }

                GeomPoint normale = GeomPoint.VectorProduct(_cube.Vertices[surfacePoint3] - _cube.Vertices[surfacePoint1], _cube.Vertices[surfacePoint2] - _cube.Vertices[surfacePoint1]);

                if (GeomPoint.DotProduct(normale, _cube.Vertices[randomPoint] - _cube.Vertices[surfacePoint1]) > 0) normale *= -1;

                if (GeomPoint.DotProduct(normale, _cube.Vertices[surfacePoint1] - _spectatorPosition) > 0)
                {
                    _cube.Surfaces.Remove(currentSurface);
                    i--;
                }
            }
        }

        // Определение дистанции до плоскости обзора
        private double FindOrthogonalDistance(GeomPoint p)
        {
            double d = GeomPoint.DotProduct(_forwardDirection, p - _spectatorPosition) / GeomPoint.DotProduct(_forwardDirection, _forwardDirection);

            return (d >= _nearSurface) ? d : _nearSurface;
        }

        // Ассоциациативный массив с точками в трехмерном пространстве к проекциеям точек на экран
        private Dictionary<GeomPoint, GeomPoint> GetPointsPerspectiveProjection()
        {
            Dictionary<GeomPoint, GeomPoint> point3dTo2d = new Dictionary<GeomPoint, GeomPoint>();

            // Постоянный коэффициент удаления от точки обзора в зависимости от угла обзора
            double coeff = 2 * Math.Tan(_fieldOfView / 2 * _degToRad);

            foreach (GeomPoint vertex in _cube.Vertices)
            {
                // Находим дистанцию до плоскости относительно точки
                double d = FindOrthogonalDistance(vertex);

                // Строим вектор из центра плоскости в сторону точки
                GeomPoint surfaceVector = vertex - d * _forwardDirection - _spectatorPosition;

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
                GeomPoint screenPosition = new GeomPoint(screenUnits[kx, 0], screenUnits[ky, 0]) * (_width / (d * coeff));

                point3dTo2d.Add(vertex, screenPosition);
            }

            return point3dTo2d;
        }

        // Отрисовка каждого ребра
        private void Draw2DCube()
        {
            List<(int, int)> drawnEdges = new List<(int, int)>();
            VolumetricObject proj = _cube.Get2DProjection(GetPointsPerspectiveProjection());
            foreach (Surface surface in proj.Surfaces)
            {
                for (int i = 0; i < surface.Count; i++)
                {
                    int index1 = surface[i], index2 = surface[(i + 1) % surface.Count];
                    if (drawnEdges.Contains((index1, index2)) || drawnEdges.Contains((index2, index1))) continue;
                    DrawingUtil.DrawLineBresenham(proj.Vertices[index1], proj.Vertices[index2], _graphics, _redBrush, _linesBrushWidth);
                    drawnEdges.Add((index1, index2));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _graphics.Clear(Color.White);
            InitializeCube();
            InitializeSpectator();
            DeleteInvisibleSurfaces();
            Draw2DCube();
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
