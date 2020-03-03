using System;
using System.Text;

namespace MatrixTransformations
{
    public class Matrix
    {
        public readonly float[,] mat = new float[3, 3];

        public Matrix(float m11, float m12, float m13,
            float m21, float m22, float m23,
            float m31, float m32, float m33)
        {
            mat[0, 0] = m11;
            mat[0, 1] = m12;
            mat[0, 2] = m13;
            mat[1, 0] = m21;
            mat[1, 1] = m22;
            mat[1, 2] = m23;
            mat[2, 0] = m31;
            mat[2, 1] = m31;
            mat[2, 2] = m31;
        }

        public Matrix(float m11, float m12,
            float m21, float m22) : this()
        {
            mat[0, 0] = m11;
            mat[0, 1] = m12;
            mat[1, 0] = m21;
            mat[1, 1] = m22;
        }

        public Matrix() : this(1, 0, 0, 0, 1, 0, 0, 0, 1)
        { }

        public Matrix(Vector v) : this(v.x, 0, v.y, 0)
        {
            // Overloaded with Vector (empty matrix with col=1 filled by vector)
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            var m = new Matrix();
            for (int r = 0; r < m.mat.GetLength(0); r++)
            {
                for (int c = 0; c < m.mat.GetLength(1); c++)
                {
                    m.mat[r, c] = m1.mat[r, c] + m2.mat[r, c];
                }

            }
            return m;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            var m = new Matrix();
            for (int r = 0; r < m.mat.GetLength(0); r++)
            for (int c = 0; c < m.mat.GetLength(1); c++)
                m.mat[r, c] = m1.mat[r, c] - m2.mat[r, c];
            return m;
        }
        public static Matrix operator *(Matrix m1, float f)
        {
            var m = new Matrix();
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
            // broke
            var i = new Matrix(0,0,0,0);
            for (int n = 0; n < m1.mat.GetLength(0); n++)
            for (int k = 0; k < m1.mat.GetLength(1); k++)
            for (int m = 0; m < m2.mat.GetLength(1); m++)
            {
                i.mat[n, k] += m1.mat[n, m] * m2.mat[m, k];
            }
            return i;
        }

        public static Vector operator *(Matrix m1, Vector v)
        {
            //throw new NotImplementedException();
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
            return new Vector(mat[0, 0], mat[1, 0]);
        }

        public static Matrix ScaleMatrix(float s)
        {
            Matrix m = new Matrix();

            return s * m;
        }

        public static Matrix RotateMatrix(float degrees)
        {
            float radians = degrees * (float) (Math.PI / 180);
            Matrix rotate = new Matrix((float) Math.Cos(radians), (float) -Math.Sin(radians), (float) Math.Sin(radians), (float) Math.Cos(radians));

            return rotate;
        }

        public static Matrix TranslateMatrix(float tx, float ty)
        {
            Matrix translate = new Matrix();
            translate.mat[0, 2] = tx;
            translate.mat[1, 2] = ty;
            
            return translate;
        }

        public override string ToString()
        {
            return $"{mat[0,0]} , {mat[0,1]} , {mat[0,2]} , " +
                   $"{mat[1,0]} , {mat[1,1]} , {mat[1,2]} , " +
                   $"{mat[2,0]} , {mat[2,1]} , {mat[2,2]}";
        }
    }
}
