﻿using System;

namespace GeometricStructures
{
    public class Matrix
    {
        private float[,] data;

        public int NbRows { get; private set; }

        public int NbCols { get; private set; }
        public float this[int i, int j]
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
                throw new Exception($"Ошибка создания матрицы, значение m должно быть >= 1.");
            }
            if (n < 1)
            {
                throw new Exception($"Ошибка создания матрицы, значение n должно быть >= 1.");
            }

            NbRows = m;
            NbCols = n;

            data = new float[m, n];
            FillMatrix(0);
        }

        public Matrix(float[,] data)
        {
            NbRows = data.GetLength(0);
            NbCols = data.GetLength(1);

            this.data = new float[NbRows, NbCols];

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

        public static Matrix operator +(Matrix A, float x)
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

        public static Matrix operator -(Matrix A, float x)
        {
            return A + (-x);
        }

        public static Matrix operator *(Matrix A, float x)
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

        public float[,] ToArray()
        {
            float[,] arr = new float[NbRows, NbCols];

            for (int i = 0; i < NbRows; i++)
            {
                for (int j = 0; j < NbCols; j++)
                {
                    arr[i, j] = data[i, j];
                }
            }

            return arr;
        }

        private void FillMatrix(float x)
        {
            for (int i = 0; i < NbRows; i++)
            {
                for (int j = 0; j < NbCols; j++)
                {
                    data[i, j] = x;
                }
            }
        }

    }
}