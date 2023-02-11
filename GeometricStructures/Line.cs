using System;

namespace GeometricStructures
{
    public class Line
    {
        private Matrix matrix;
        public GeomPoint point1
        {
            get
            {
                return new GeomPoint(matrix[0, 0], matrix[0, 1]);
            }
            private set { }
        }

        public GeomPoint point2
        {
            get
            {
                return new GeomPoint(matrix[1, 0], matrix[1, 1]);
            }
            private set { }
        }

        public Line(Matrix data)
        {
            if (data.NbCols != 2)
            {
                throw new Exception("Ошибка создания линии, она должна содержать 2 колонки.");
            }

            if (data.NbCols != 2)
            {
                throw new Exception("Ошибка создания линии, она должна содержать 2 ряда");
            }

            matrix = data;
        }

        public Line(GeomPoint point1, GeomPoint point2)
        {
            matrix = new Matrix(new double[,] { { point1.x, point1.y }, { point2.x, point2.y } });
        }

        public Line(double x1, double y1, double x2, double y2)
        {
            matrix = new Matrix(new double[,] { { x1, y1 }, { x2, y2 } });
        }

        public static Line operator *(Line A, Matrix T)
        {
            return new Line(A.matrix * T);
        }

    }
}