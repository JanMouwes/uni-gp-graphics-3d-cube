using System;
using System.Text;
using System.Threading;

namespace MatrixTransformations
{
    public class Matrix
    {
        public readonly float[,] mat = new float[4, 4];

        public Matrix(float m11, float m12, float m13, float m14,
                      float m21, float m22, float m23, float m24,
                      float m31, float m32, float m33, float m34,
                      float m41, float m42, float m43, float m44)
        {
            mat[0, 0] = m11;
            mat[0, 1] = m12;
            mat[0, 2] = m13;
            mat[0, 3] = m14;
            mat[1, 0] = m21;
            mat[1, 1] = m22;
            mat[1, 2] = m23;
            mat[1, 3] = m24;
            mat[2, 0] = m31;
            mat[2, 1] = m32;
            mat[2, 2] = m33;
            mat[2, 3] = m34;
            mat[3, 0] = m41;
            mat[3, 1] = m42;
            mat[3, 2] = m43;
            mat[3, 3] = m44;
        }

        public Matrix(float m11, float m12, float m13,
                      float m21, float m22, float m23,
                      float m31, float m32, float m33) : this(
            m11, m12, m13, 0,
            m21, m22, m23, 0,
            m31, m32, m33, 0,
            0, 0, 0, 1
        ) { }

        public Matrix(float m11, float m12,
                      float m21, float m22) : this(
            m11, m12, 0,
            m21, m22, 0,
            0, 0, 1) { }

        public Matrix() : this(1, 0, 0, 1) { }

        public Matrix(Vector v) : this()
        {
            mat[0, 0] = v.x;
            mat[1, 0] = v.y;
            mat[2, 0] = v.z;
            mat[3, 0] = v.w;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            Matrix m = new Matrix();

            for (int r = 0; r < m.mat.GetLength(0); r++)
            {
                for (int c = 0; c < m.mat.GetLength(1); c++) { m.mat[r, c] = m1.mat[r, c] + m2.mat[r, c]; }
            }

            return m;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            Matrix m = new Matrix();
            for (int r = 0; r < m.mat.GetLength(0); r++)
                for (int c = 0; c < m.mat.GetLength(1); c++)
                    m.mat[r, c] = m1.mat[r, c] - m2.mat[r, c];

            return m;
        }

        public static Matrix operator *(Matrix m1, float f)
        {
            Matrix m = new Matrix();
            for (int r = 0; r < m1.mat.GetLength(0); r++)
                for (int c = 0; c < m1.mat.GetLength(1); c++)
                    m.mat[r, c] = m1.mat[r, c] * f;

            return m;
        }

        public static Matrix operator *(float f, Matrix m1)
        {
            return m1 * f;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix resultMatrix = new Matrix();

            for (int x = 0; x < resultMatrix.mat.GetLength(0); x++) // Loop through columns of resultMatrix
            {
                for (int y = 0; y < resultMatrix.mat.GetLength(1); y++) // Loop through rows of resultMatrix
                {
                    resultMatrix.mat[x, y] = 0; // Reset matrix-position 

                    for (int i = 0; i < resultMatrix.mat.GetLength(0); i++) // Loop through row-to-column mapped items
                    {
                        resultMatrix.mat[x, y] += m1.mat[x, i] * m2.mat[i, y];
                    }
                }
            }

            return resultMatrix;
        }

        public static Vector operator *(Matrix m1, Vector v)
        {
            Matrix m = new Matrix(v);
            Matrix result = m1 * m;

            return result.ToVector();
        }

        public static Matrix Identity()
        {
            return new Matrix();
        }

        public Vector ToVector()
        {
            return new Vector(mat[0, 0], mat[1, 0], mat[2, 0], mat[3, 0]);
        }

        public static Matrix ScaleMatrix(float s)
        {
            Matrix m = new Matrix();

            m.mat[0, 0] = s;
            m.mat[1, 1] = s;
            m.mat[2, 2] = s;

            return m;
        }

        public static Matrix RotateMatrix(float degreesX, float degreesY, float degreesZ)
        {
            Matrix matrixX = RotateXMatrix(degreesX);
            Matrix matrixY = RotateYMatrix(degreesY);
            Matrix matrixZ = RotateZMatrix(degreesZ);

            return matrixZ * matrixY * matrixX;
        }

        public static Matrix RotateXMatrix(float degrees)
        {
            float radians = degrees * (float) (Math.PI / 180);
            Matrix rotate = new Matrix(
                1, 0, 0,
                0, (float) Math.Cos(radians), (float) -Math.Sin(radians),
                0, (float) Math.Sin(radians), (float) Math.Cos(radians)
            );

            return rotate;
        }

        public static Matrix RotateYMatrix(float degrees)
        {
            float radians = degrees * (float) (Math.PI / 180);
            Matrix rotate = new Matrix(
                (float) Math.Cos(radians), 0, (float) Math.Sin(radians),
                0, 1, 0,
                (float) -Math.Sin(radians), 0, (float) Math.Cos(radians)
            );

            return rotate;
        }

        public static Matrix RotateZMatrix(float degrees)
        {
            float radians = degrees * (float) (Math.PI / 180);
            Matrix rotate = new Matrix(
                (float) Math.Cos(radians), (float) -Math.Sin(radians),
                (float) Math.Sin(radians), (float) Math.Cos(radians)
            );

            return rotate;
        }

        public static Matrix TranslateMatrix(Vector t)
        {
            Matrix translate = new Matrix();
            translate.mat[0, 3] = t.x;
            translate.mat[1, 3] = t.y;
            translate.mat[2, 3] = t.z;

            return translate;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int r = 0; r < mat.GetLength(0); r++)
            {
                for (int c = 0; c < mat.GetLength(1); c++) { sb.Append($"{mat[r, c]} , "); }

                sb.Append("\n");
            }

            return sb.ToString();
        }

        public static Matrix ViewMatrix(float distToCam, float phi, float theta)
        {
            float sinTheta = (float) Math.Sin(theta);
            float sinPhi = (float) Math.Sin(phi);
            float cosTheta = (float) Math.Cos(theta);
            float cosPhi = (float) Math.Cos(phi);


            Matrix matrix = new Matrix(
                -sinTheta, cosTheta, 0, 0,
                -cosTheta * cosPhi, -cosPhi  * sinTheta, sinPhi, 0,
                cosTheta  * sinPhi, sinTheta * sinPhi, cosPhi, -distToCam,
                0, 0, 0, 1
            );

            return matrix;
        }

        public static Matrix ProjectionMatrix(float distToScreen, float distTotal)
        {
            float scaleNumber = -(distToScreen / distTotal);

            return new Matrix(scaleNumber, 0, 0, scaleNumber);
        }
    }
}