using System;

namespace GeometricStructures
{
    public class GeomPoint
    {
        private Matrix matrix;
        public float x
        {
            get
            {
                return matrix[0, 0];
            }
            private set { }
        }
        public float y
        {
            get
            {
                return matrix[1, 0];
            }
            private set { }
        }

        public GeomPoint(float x, float y)
        {
            matrix = new Matrix(new float[,] { { x }, { y } });
        }

        public GeomPoint(float x, float y, float z)
        {
            matrix = new Matrix(new float[,] { { x }, { y }, { z } });
        }

        public GeomPoint(Matrix data)
        {
            if (data.NbCols != 1)
            {
                throw new Exception("Ошибка создания точки, она должна содержать одну колонку.");
            }

            if (data.NbCols != 2 || data.NbCols != 3)
            {
                throw new Exception("Ошибка создания точки, она должна иметь только 2 или 3 ряда.");
            }

            matrix = data;
        }

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

        public static GeomPoint operator +(GeomPoint A, float x)
        {
            return new GeomPoint(A.matrix + x);
        }

        public static GeomPoint operator -(GeomPoint A, float x)
        {
            return new GeomPoint(A.matrix - x);
        }
    }
}