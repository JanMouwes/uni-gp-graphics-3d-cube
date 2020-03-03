using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MatrixTransformations
{
    public partial class Form1 : Form
    {
        private List<Vector> vb;
        private Matrix m;

        // Axes
        AxisX x_axis;
        AxisY y_axis;

        // Objects
        Square square;
        private Square square2;
        private Square square3;
        private Square square4;

        // Window dimensions
        const int WIDTH = 800;
        const int HEIGHT = 600;

        public Form1()
        {
            InitializeComponent();

            this.Width = WIDTH;
            this.Height = HEIGHT;
            this.DoubleBuffered = true;
            m = new Matrix();

            Vector v1 = new Vector();
            Console.WriteLine(v1);
            Vector v2 = new Vector(1, 2);
            Console.WriteLine(v2);
            Vector v3 = new Vector(2, 6);
            Console.WriteLine(v3);
            Vector v4 = v2 + v3;
            Console.WriteLine(v4); // 3, 8

            Matrix m1 = new Matrix();
            Console.WriteLine(m1); // 1, 0, 0, 1
            Matrix m2 = new Matrix(
                2, 4,
                -1, 3);
            Console.WriteLine(m2);
            Console.WriteLine(m1 + m2); // 3, 4, -1, 4
            Console.WriteLine(m1 - m2); // -1, -4, 1, -2
            Console.WriteLine(m2 * m2); // 0, 20, -5, 5

            Console.WriteLine(m2 * v3); // 28, 16

            // Define axes
            x_axis = new AxisX(200);
            y_axis = new AxisY(200);

            // Create object
            square = new Square(Color.Purple, 100);

            square2 = new Square(Color.Orange, 100);

            square3 = new Square(Color.Cyan, 100);

            square4 = new Square(Color.DarkBlue, 100);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw axes
            //  vb = vector buffer
            vb = ViewportTransformation(x_axis.vb).ToList();
            x_axis.Draw(e.Graphics, vb);
            vb = ViewportTransformation(y_axis.vb).ToList();
            y_axis.Draw(e.Graphics, vb);

            // Draw squares
            vb = ViewportTransformation(square.vb).ToList();
            square.Draw(e.Graphics, vb);

            vb.Clear();

            Matrix s = Matrix.ScaleMatrix(1.5f);
            vb = ViewportTransformation(TransformVectors(this.square2.vb, s)).ToList();
            square2.Draw(e.Graphics, vb);

            vb.Clear();

            Matrix r = Matrix.RotateMatrix(20);
            vb = ViewportTransformation(TransformVectors(this.square3.vb, r)).ToList();
            square3.Draw(e.Graphics, vb);

            vb.Clear();
            Matrix t = Matrix.TranslateMatrix(75, -25);

            vb = ViewportTransformation(TransformVectors(this.square4.vb, t)).ToList();
            square4.Draw(e.Graphics, vb);
        }

        private static IEnumerable<Vector> TransformVectors(IEnumerable<Vector> vectors, Matrix matrix)
        {
            foreach (Vector v in vectors) { yield return matrix * v; }
        }

        public IEnumerable<Vector> ViewingPipeline(List<Vector> vb)
        {
            List<Vector> res = new List<Vector>();
            Vector vp = new Vector();

            foreach (var v in vb)
            {
                /*
                Matrix view = Matrix.ViewMatrix(r, phi, theta);
                vp = view * v;

                Matrix proj = Matrix.ProjectionMatrix(decimal, vp.z);
                vp = proj * vp;

                res.Add(vp);
                */
            }

            return ViewportTransformation(res);
        }

        public static IEnumerable<Vector> ViewportTransformation(IEnumerable<Vector> vb)
        {
            const float cx = WIDTH  / 2;
            const float cy = HEIGHT / 2;

            foreach (Vector v in vb) { yield return new Vector(v.x + cx, cy - v.y); }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }
    }
}