using System;

namespace GeometricStructures
{
    public class GeomPoint
    {
        private Matrix matrix;
        public double x
        {
            get
            {
                return matrix[0, 0];
            }
            private set { }
        }
        public double y
        {
            get
            {
                return matrix[1, 0];
            }
            private set { }
        }
        public double z
        {
            get
            {
                return matrix[2, 0];
            }
            private set { }
        }

        public GeomPoint(double x, double y)
        {
            matrix = new Matrix(new double[,] { { x }, { y }, { 0 } });
        }

        public GeomPoint(double x, double y, double z)
        {
            matrix = new Matrix(new double[,] { { x }, { y }, { z } });
        }

        public GeomPoint(Matrix data)
        {
            if (data.NbCols != 1)
            {
                throw new Exception("Ошибка создания точки, она должна содержать одну колонку.");
            }

            if (data.NbRows != 2 && data.NbRows != 3)
            {
                throw new Exception("Ошибка создания точки, она должна иметь только 2 или 3 ряда.");
            }

            matrix = data;
        }

        public GeomPoint(double[,] data) : this(new Matrix(data)) { }

        public static GeomPoint operator +(GeomPoint A, GeomPoint B)
        {
            return new GeomPoint(A.matrix + B.matrix);
        }

        public static GeomPoint operator -(GeomPoint A, GeomPoint B)
        {
            return new GeomPoint(A.matrix - B.matrix);
        }

        public static GeomPoint operator *(GeomPoint A, Matrix T)
        {
            return new GeomPoint(A.matrix * T);
        }

        public static GeomPoint operator *(Matrix T, GeomPoint A)
        {
            return new GeomPoint(T * A.matrix);
        }

        public static GeomPoint operator *(GeomPoint A, double x)
        {
            return new GeomPoint(A.matrix * x);
        }

        public static GeomPoint operator *(double x, GeomPoint A)
        {
            return new GeomPoint(x * A.matrix);
        }

        public double distance(GeomPoint B)
        {
            return Math.Sqrt(Math.Pow(x - B.x, 2) + Math.Pow(y - B.y, 2) + Math.Pow(z - B.z, 2));
        }
    }
}
