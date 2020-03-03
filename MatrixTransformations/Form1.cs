using System;
using System.Collections.Generic;
using System.Drawing;
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
            square = new Square(Color.Purple,100);

            square2 = new Square(Color.Orange, 100);

            square3 = new Square(Color.Cyan, 100);

            square4 = new Square(Color.DarkBlue, 100);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw axes
            //  vb = vector buffer
            vb = ViewportTransformation(x_axis.vb);
            x_axis.Draw(e.Graphics, vb);
            vb = ViewportTransformation(y_axis.vb);
            y_axis.Draw(e.Graphics, vb);

            // Draw squares
            vb = ViewportTransformation(square.vb);
            square.Draw(e.Graphics, vb);
            
            vb.Clear();
            var s = Matrix.ScaleMatrix(1.5f);
            foreach (var v in square2.vb)
            {
                Vector v2 = s * v;
                vb.Add(v2);
            }
            vb = ViewportTransformation(vb);
            square2.Draw(e.Graphics, vb);
            
            vb.Clear();
            var r = Matrix.RotateMatrix(20);
            foreach (var v in square3.vb)
            {
                Vector v2 = r * v;
                vb.Add(v2);
            }
            vb = ViewportTransformation(vb);
            square3.Draw(e.Graphics, vb);

            vb.Clear();
            //var r = Matrix.RotateMatrix(20);
            foreach (var v in square4.vb)
            {
                Vector v2 = v;
                vb.Add(v2);
            }
            vb = ViewportTransformation(vb);
            square4.Draw(e.Graphics, vb);
        }

        public static List<Vector> ViewportTransformation(List<Vector> vb)
        {
            List<Vector> result = new List<Vector>();

            float cx = WIDTH / 2;
            float cy = HEIGHT / 2;

            foreach (var v in vb)
            {
                Vector v2 = new Vector(v.x + cx, cy - v.y);
                result.Add(v2);
            }

            return result;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }
    }
}
