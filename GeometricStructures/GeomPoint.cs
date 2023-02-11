namespace GeometricStructures
{
    public class GeomPoint
    {
        private Matrix matrix;
        public float this[int i, int j]
        {
            get
            {
                return matrix[i, j];
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