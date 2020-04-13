using NUnit.Framework;

namespace MatrixTransformations.Tests
{
    public class MatrixTests
    {
        [SetUp]
        public void Setup() { }

        [TestCase(
            2, 4, -1, 3,
            2, 4, -1, 3,
            0, 20, -5, 5
        )]
        [TestCase(
            1, 0, 0, 1,
            1, 0, 0, 1,
            1, 0, 0, 1
        )]
        public void MultiplyTest(
            float m1_11, float m1_12, float m1_21, float m1_22,
            float m2_11, float m2_12, float m2_21, float m2_22,
            float exp_11, float exp_12, float exp_21, float exp_22
        )
        {
            Matrix m1 = new Matrix(m1_11, m1_12, m1_21, m1_22);

            Matrix m2 = new Matrix(m2_11, m2_12, m2_21, m2_22);

            Matrix expected = new Matrix(exp_11, exp_12, exp_21, exp_22);

            Matrix actual = m1 * m2;

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        public void MultiplyScalarTest(
            float m1_11, float m1_12, float m1_21, float m1_22,
            float scalar,
            float exp_11, float exp_12, float exp_21, float exp_22
        )
        {
            Matrix m1 = new Matrix(m1_11, m1_12, m1_21, m1_22);

            Matrix expected = new Matrix(exp_11, exp_12, exp_21, exp_22);

            Matrix actual = m1 * scalar;

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}