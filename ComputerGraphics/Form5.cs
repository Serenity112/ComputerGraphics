using GeometricStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerGraphics
{
    public partial class Form5 : Form
    {
        public enum SurfaceInvisibilityMode
        {
            NOT_DISPLAYABLE,
            DASHED_DISPLAYABLE,
            FULLY_DISPLAYABLE
        }

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

        private SurfaceInvisibilityMode _edgesDisplayMode;

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
        private void DetectInvisibleSurfaces()
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
                    currentSurface.Invisible = true;
                }
            }
        }

        // Определение дистанции до плоскости обзора
        private double FindOrthogonalDistance(GeomPoint p)
        {
            double d = GeomPoint.DotProduct(_forwardDirection, p - _spectatorPosition);

            return (d >= _nearSurface) ? d : _nearSurface;
        }

        // Ассоциациативный массив с точками в трехмерном пространстве к проекциеям точек на экран
        private Dictionary<GeomPoint, GeomPoint> GetPointsPerspectiveProjection()
        {
            double maximumError = 1e-12;
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
                    if (!(_currentViewRotation[i, 0] <= maximumError && _currentViewRotation[i, 0] >= -maximumError))
                    {
                        kx = i;
                        break;
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    if (!(_currentViewRotation[i, 1] <= maximumError && _currentViewRotation[i, 1] >= -maximumError) && i != kx)
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
                GeomPoint screenPosition = new GeomPoint(screenUnits[0, 0], screenUnits[1, 0]) * (_width / (d * coeff));

                point3dTo2d.Add(vertex, screenPosition);
            }

            return point3dTo2d;
        }

        private void DrawEdge(GeomPoint vertex1, GeomPoint vertex2)
        {
            switch (_edgesDisplayMode)
            {
                case SurfaceInvisibilityMode.NOT_DISPLAYABLE:
                    break;
                case SurfaceInvisibilityMode.FULLY_DISPLAYABLE:
                case SurfaceInvisibilityMode.DASHED_DISPLAYABLE:
                    DrawingUtil.DrawLineBresenham(vertex1, vertex2, _graphics, _redBrush, _linesBrushWidth, _edgesDisplayMode == SurfaceInvisibilityMode.DASHED_DISPLAYABLE);
                    break;
            }
        }

        // Отрисовка каждого ребра
        private void Draw2DCube()
        {
            List<(int, int)> drawnVisibleEdges = new List<(int, int)>(), drawnInvisibleEdges = new List<(int, int)>();
            VolumetricObject proj = _cube.Get2DProjection(GetPointsPerspectiveProjection());
            foreach (Surface surface in proj.Surfaces)
            {
                for (int i = 0; i < surface.Count; i++)
                {
                    int index1 = surface[i], index2 = surface[(i + 1) % surface.Count];

                    if (drawnVisibleEdges.Contains((index1, index2)) || drawnVisibleEdges.Contains((index2, index1))) continue;
                    else if (!surface.Invisible)
                    {
                        DrawingUtil.DrawLineBresenham(proj.Vertices[index1], proj.Vertices[index2], _graphics, _redBrush, _linesBrushWidth);
                        drawnVisibleEdges.Add((index1, index2));
                    }

                    if (surface.Invisible && !(drawnInvisibleEdges.Contains((index1, index2)) || drawnInvisibleEdges.Contains((index2, index1))))
                    {
                        DrawEdge(proj.Vertices[index1], proj.Vertices[index2]);
                        drawnInvisibleEdges.Add((index1, index2));
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _graphics.Clear(Color.White);
            
            switch (edgesDisplayMode.Text)
            {
                case "Отображаются пунктиром":
                    _edgesDisplayMode = SurfaceInvisibilityMode.DASHED_DISPLAYABLE;
                    break;
                case "Отображаются полностью":
                    _edgesDisplayMode = SurfaceInvisibilityMode.FULLY_DISPLAYABLE;
                    break;
                default:
                    _edgesDisplayMode = SurfaceInvisibilityMode.NOT_DISPLAYABLE;
                    break;
            }

            InitializeCube();
            InitializeSpectator();
            DetectInvisibleSurfaces();
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
