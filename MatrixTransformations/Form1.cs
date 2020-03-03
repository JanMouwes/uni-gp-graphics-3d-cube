using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MatrixTransformations
{
    public partial class Form1 : Form
    {
        // Window dimensions
        const int WIDTH = 800;
        const int HEIGHT = 600;

        // Axes
        readonly AxisX x_axis;
        readonly AxisY y_axis;
        readonly AxisZ z_axis;

        private Cube cube;
        private List<Vector> vb; // Vector buffer

        private CameraState cameraState;

        public float TurnSpeed = 5;

        public Vector CubePosition;
        private readonly Dictionary<Keys, bool> keyStates;

        public Form1()
        {
            InitializeComponent();

            this.timer1.Interval = 1;
            this.timer1.Enabled = true;

            this.Width = WIDTH;
            this.Height = HEIGHT;
            this.DoubleBuffered = true;

            this.CubePosition = new Vector(0, 0, 0);

            this.cameraState = CameraState.Default;

            this.keyStates = new Dictionary<Keys, bool>();

            // Define axes
            x_axis = new AxisX(3);
            y_axis = new AxisY(3);
            z_axis = new AxisZ(3);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DrawAxes(e);

            this.cube = new Cube(Color.Purple);

            this.vb = cube.vertexbuffer;
            this.vb = ViewingPipeline(this.vb).ToList();

            cube.Draw(e.Graphics, this.vb);

            PaintGui(e);
        }

        private void DrawAxes(PaintEventArgs e)
        {
            // Draw axes
            //  vb = vector buffer
            vb = ViewingPipeline(x_axis.vb).ToList();
            x_axis.Draw(e.Graphics, vb);
            vb = ViewingPipeline(y_axis.vb).ToList();
            y_axis.Draw(e.Graphics, vb);
            vb = ViewingPipeline(z_axis.vb).ToList();
            z_axis.Draw(e.Graphics, vb);
        }

        private IEnumerable<string> GuiItems()
        {
            yield return "D: "      + this.cameraState.D;
            yield return "Radius: " + this.cameraState.Radius;
            yield return "Phi: "    + this.cameraState.Phi;
            yield return "Theta: "  + this.cameraState.Theta;

            yield return "CubePosition: " + this.CubePosition;
        }

        private void PaintGui(PaintEventArgs e)
        {
            Font font = new Font("Arial", 12, FontStyle.Bold);

            int i = 0;

            foreach (string guiItem in GuiItems())
            {
                const int x = 10;
                int y = i * 16;

                PointF p = new PointF(x, y);
                e.Graphics.DrawString(guiItem, font, Brushes.Black, p);
                i++;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            float diff = this.TurnSpeed;

            if (this.keyStates.ContainsKey(e.KeyCode)) { this.keyStates[e.KeyCode] = true; }
            else { this.keyStates.Add(e.KeyCode, true); }

            switch (e.KeyCode)
            {
                case Keys.C:
                    this.Reset();

                    break;
                case Keys.Right:
                    this.cameraState.ThetaVelocity = diff;

                    break;
                case Keys.Left:
                    this.cameraState.ThetaVelocity = -diff;

                    break;
                case Keys.Up:
                    this.cameraState.PhiVelocity = diff;

                    break;
                case Keys.Down:
                    this.cameraState.PhiVelocity = -diff;

                    break;
                case Keys.Oemplus:
                case Keys.PageUp:
                    this.CubePosition.z += 1;

                    break;
                case Keys.OemMinus:
                case Keys.PageDown:
                    this.CubePosition.z -= 1;

                    break;
            }

            base.OnKeyDown(e);
        }

        private void Reset()
        {
            this.cameraState = CameraState.Default;
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);

            if (se.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                float change;

                if (se.NewValue > se.OldValue) { change = 1; }
                else { change = -1; }

                this.CubePosition.z += change;
            }
        }

        public IEnumerable<Vector> ViewingPipeline(IEnumerable<Vector> vectorBuffer)
        {
            List<Vector> res = new List<Vector>();
            Vector currentVector = new Vector();

            const float radianConverter = (float) Math.PI / 180f;

            float phiRads = this.cameraState.Phi     * radianConverter;
            float thetaRads = this.cameraState.Theta * radianConverter;

            foreach (Vector v in vectorBuffer)
            {
                Matrix view = Matrix.ViewMatrix(1, phiRads, thetaRads);
                currentVector = view * v;

                Matrix proj = Matrix.ProjectionMatrix(this.cameraState.D, currentVector.z);
                currentVector = proj * currentVector;

                res.Add(currentVector);
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

        private void Form1_Load(object sender, EventArgs e) { }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.cameraState.Update();

            this.Refresh();
        }
    }
}