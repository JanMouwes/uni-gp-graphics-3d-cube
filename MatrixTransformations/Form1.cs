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
        AxisZ z_axis;

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

            // Define axes
            x_axis = new AxisX(3);
            y_axis = new AxisY(3);
            z_axis = new AxisZ(3);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw axes
            //  vb = vector buffer
            vb = ViewingPipeline(x_axis.vb).ToList();
            x_axis.Draw(e.Graphics, vb);
            vb = ViewingPipeline(y_axis.vb).ToList();
            y_axis.Draw(e.Graphics, vb);
            vb = ViewingPipeline(z_axis.vb).ToList();
            z_axis.Draw(e.Graphics, vb);

            Cube cube  = new Cube(Color.Purple);

            this.vb = cube.vertexbuffer;
            this.vb = ViewingPipeline(this.vb).ToList();
            
            cube.Draw(e.Graphics, this.vb);
        }

        private static IEnumerable<Vector> TransformVectors(IEnumerable<Vector> vectors, Matrix matrix)
        {
            foreach (Vector v in vectors) { yield return matrix * v; }
        }

        public IEnumerable<Vector> ViewingPipeline(IEnumerable<Vector> vb)
        {
            List<Vector> res = new List<Vector>();
            Vector vp = new Vector();

            foreach (var v in vb)
            {
                 Matrix view = Matrix.ViewMatrix(1, 1, 1);
                 vp = view * v;

                 Matrix proj = Matrix.ProjectionMatrix(100, vp.z);
                 vp = proj * vp;

                 res.Add(vp);
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}