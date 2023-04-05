﻿using System;

namespace GeometricStructures
{
    public class Matrix
    {
        private double[,] data;

        public int NbRows { get; private set; }

        public int NbCols { get; private set; }
        public double this[int i, int j]
        {
            get
            {
                return data[i, j];
            }
            private set
            {
                data[i, j] = value;
            }
        }

        public Matrix(int m, int n)
        {
            if (m < 1)
            {
                throw new Exception($"Ошибка создания матрицы, значение рядов m должно быть >= 1.");
            }
            if (n < 1)
            {
                throw new Exception($"Ошибка создания матрицы, значение колонок n должно быть >= 1.");
            }

            NbRows = m;
            NbCols = n;

            data = new double[m, n];
            FillMatrix(0);
        }

        public Matrix(double[,] data)
        {
            NbRows = data.GetLength(0);
            NbCols = data.GetLength(1);

            if (NbRows < 1)
            {
                throw new Exception($"Ошибка создания матрицы, значение рядов должно быть >= 1.");
            }
            if (NbCols < 1)
            {
                throw new Exception($"Ошибка создания матрицы, значение колонок должно быть >= 1.");
            }

            this.data = new double[NbRows, NbCols];

            for (int i = 0; i < NbRows; i++)
            {
                for (int j = 0; j < NbCols; j++)
                {
                    this.data[i, j] = data[i, j];
                }
            }
        }

        public Matrix(Matrix A) : this(A.data) { }

        public static Matrix operator -(Matrix A)
        {
            Matrix res = new Matrix(A.NbRows, A.NbCols);

            for (int i = 0; i < A.NbRows; i++)
            {
                for (int j = 0; j < A.NbCols; j++)
                {
                    res.data[i, j] = -A.data[i, j];
                }
            }

            return res;
        }

        public static Matrix Identity(uint size)
        {
            Matrix identity = new Matrix(new double[size, size]);
            for (uint i = 0; i < size; i++)
            {
                for (uint j = 0; j < size; j++)
                {
                    identity.data[i, j] = (i == j) ? 1 : 0; 
                }
            }
            return identity;
        }

        /// Возвращает матрицу поворота вокруг осей X, Y и Z на rX, rY и rZ радиан
        public static Matrix RotationBy(double rX, double rY, double rZ)
        {
            Matrix rotX = new Matrix(new double[3, 3]
             {
                { 1, 0, 0 },
                { 0, Math.Cos(rX), -Math.Sin(rX) },
                { 0, Math.Sin(rX), Math.Cos(rX) }
             });

            Matrix rotY = new Matrix(new double[3, 3]
            {
                { Math.Cos(rY), 0, Math.Sin(rY) },
                { 0,1,0 },
                { -Math.Sin(rY), 0, Math.Cos(rY) }
            });

            Matrix rotZ = new Matrix(new double[3, 3]
            {
                { Math.Cos(rZ), -Math.Sin(rZ), 0 },
                { Math.Sin(rZ), Math.Cos(rZ), 0 },
                { 0, 0, 1 }
            });

            return rotX * rotY * rotZ;
        }

        // Решение СЛАУ методом Гаусса
        public static Matrix SolveLinearEquation(Matrix A, Matrix B)
        {
            Matrix identity = new Matrix(A);
            Matrix result = new Matrix(B); 
            if (B.NbCols != 1 || A.NbCols != A.NbRows || A.NbCols != B.NbRows)
            {
                throw new Exception("Для решения линейного уравнения матрица B должна иметь 1 столбец, а матрица A быть квадратной с той же размерностью");
            }

            for (int i = 0; i < identity.NbRows; i++)
            {
                for (int k = 0; k < identity.NbRows; k++)
                {
                    double coef;
                    if (k == i)
                    {
                        coef = identity[i, i];
                        for (int j = 0; j < identity.NbCols; j++)
                        {
                            identity[k, j] /= coef;
                        }
                        result[k, 0] /= coef;
                    }
                    else
                    {
                        coef = identity[k, i] / identity[i, i];
                        for (int j = 0; j < identity.NbCols; j++)
                        {
                            identity[k, j] -= coef * identity[i, j];
                        }
                        result[k, 0] -= coef * B[i, 0];
                    }
                }
            }
            return result;
        }

        public static Matrix operator ~(Matrix A)
        {
            return A.Transpose();
        }

        public Matrix Transpose()
        {
            Matrix res = new Matrix(NbRows, NbCols);

            for (int i = 0; i < NbRows; i++)
            {
                for (int j = 0; j < NbCols; j++)
                {
                    data[i, j] = data[j, i];
                }
            }

            return res;
        }

        public static Matrix operator +(Matrix A, Matrix B)
        {
            if (A.NbRows != B.NbRows || A.NbCols != B.NbCols)
            {
                throw new Exception("Нельзя складывать матрицы разного размера");
            }

            Matrix res = new Matrix(A.NbRows, A.NbCols);

            for (int i = 0; i < A.NbRows; i++)
            {
                for (int j = 0; j < A.NbCols; j++)
                {
                    res.data[i, j] = A.data[i, j] + B.data[i, j];
                }
            }

            return res;
        }

        public static Matrix operator -(Matrix A, Matrix B)
        {
            return A + (-B);
        }

        public static Matrix operator *(Matrix A, Matrix B)
        {
            if (A.NbCols != B.NbRows)
            {
                throw new Exception("Размеры матриц не совпадают, перемножить нельзя!");
            }

            Matrix res = new Matrix(A.NbRows, B.NbCols);

            for (int i = 0; i < res.NbRows; i++)
            {
                for (int j = 0; j < res.NbCols; j++)
                {
                    for (int k = 0; k < A.NbCols; k++)
                    {
                        res.data[i, j] += A.data[i, k] * B.data[k, j];
                    }
                }
            }

            return res;
        }

        public static Matrix operator +(Matrix A, double x)
        {
            Matrix res = new Matrix(A.NbRows, A.NbCols);

            for (int i = 0; i < A.NbRows; i++)
            {
                for (int j = 0; j < A.NbCols; j++)
                {
                    res.data[i, j] = A.data[i, j] + x;
                }
            }

            return res;
        }

        public static Matrix operator -(Matrix A, double x)
        {
            return A + (-x);
        }

        public static Matrix operator *(Matrix A, double x)
        {
            Matrix res = new Matrix(A.NbRows, A.NbCols);

            for (int i = 0; i < A.NbRows; i++)
            {
                for (int j = 0; j < A.NbCols; j++)
                {
                    res.data[i, j] = A.data[i, j] * x;
                }
            }

            return res;
        }

        public static Matrix operator *(double x, Matrix A)
        {
            return A * x;
        }

        public static bool operator ==(Matrix A, Matrix B)
        {
            if (ReferenceEquals(A, B))
                return true;

            if (A is null || B is null)
                return false;

            return A.Equals(B);
        }

        public static bool operator !=(Matrix A, Matrix B)
        {
            return !(A == B);
        }

        public double[,] ToArray()
        {
            double[,] arr = new double[NbRows, NbCols];

            for (int i = 0; i < NbRows; i++)
            {
                for (int j = 0; j < NbCols; j++)
                {
                    arr[i, j] = data[i, j];
                }
            }

            return arr;
        }

        private void FillMatrix(double x)
        {
            for (int i = 0; i < NbRows; i++)
            {
                for (int j = 0; j < NbCols; j++)
                {
                    data[i, j] = x;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Matrix))
                return false;

            var other = (Matrix)obj;
            if (other.data.GetLength(0) != data.GetLength(0) || other.data.GetLength(1) != data.GetLength(1))
                return false;

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    if (Math.Abs(other.data[i, j] - data[i, j]) > double.Epsilon)
                        return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = 17;

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    hashCode = hashCode * 31 + data[i, j].GetHashCode();
                }
            }

            return hashCode;
        }
    }
}
