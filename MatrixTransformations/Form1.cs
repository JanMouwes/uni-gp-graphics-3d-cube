using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MatrixTransformations.Animation;
using MatrixTransformations.Control;

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

        private List<Vector> vb; // Vector buffer

        private CameraState cameraState;

        private readonly KeyboardState keyboardState;

        private Cube cube;
        private CubeController cubeController;
        private readonly AnimationEngine animationEngine;

        public Form1()
        {
            InitializeComponent();

            this.timer1.Interval = 50;
            this.timer1.Enabled = true;

            this.Width = WIDTH;
            this.Height = HEIGHT;
            this.DoubleBuffered = true;

            this.cameraState = CameraState.Default;

            this.keyboardState = new KeyboardState();

            // Define axes
            x_axis = new AxisX(3);
            y_axis = new AxisY(3);
            z_axis = new AxisZ(3);

            this.cube = new Cube(Color.Purple);

            this.cubeController = new CubeController();

            this.animationEngine = new AnimationEngine(this.cameraState, this.cubeController);
            this.animationEngine.Finished += ResetAnimation;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DrawAxes(e);

            IEnumerable<Vector> transformedVectors = this.cubeController.TransformVectors(this.cube.vertexbuffer);
            this.vb = ViewingPipeline(transformedVectors).ToList();

            cube.Draw(e.Graphics, this.vb);

            PaintGui(e);
        }

        private void DrawAxes(PaintEventArgs e)
        {
            // Draw axes
            x_axis.Draw(e.Graphics, ViewingPipeline(x_axis.vb).ToList());
            y_axis.Draw(e.Graphics, ViewingPipeline(y_axis.vb).ToList());
            z_axis.Draw(e.Graphics, ViewingPipeline(z_axis.vb).ToList());
        }

        private IEnumerable<string> GuiItems()
        {
            yield return "D: "      + this.cameraState.D;
            yield return "Radius: " + this.cameraState.Radius;
            yield return "Phi: "    + this.cameraState.Phi;
            yield return "Theta: "  + this.cameraState.Theta;

            yield return "\nTransformations: \n" + this.cubeController;
            // yield return "Cube: \n"              + this.cube;

            if (this.animationEngine.Enabled)
            {
                yield return "Animation enabled";
                yield return this.animationEngine.ToString();
            }
        }

        private void PaintGui(PaintEventArgs e)
        {
            Font font = new Font("Arial", 12, FontStyle.Bold);

            StringBuilder stringBuilder = new StringBuilder();
            PointF p = new PointF(10, 16);

            foreach (string guiItem in GuiItems()) { stringBuilder.Append(guiItem + '\n'); }

            e.Graphics.DrawString(stringBuilder.ToString(), font, Brushes.Black, p);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            this.keyboardState.SetIsKeyPressed(e.KeyCode, true);

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            this.keyboardState.SetIsKeyPressed(e.KeyCode, false);

            if (e.KeyCode  == Keys.A) { this.animationEngine.Enabled = !this.animationEngine.Enabled; }

            if (e.KeyCode== Keys.C) { this.Reset(); }

            base.OnKeyUp(e);
        }

        private void Reset()
        {
            this.cameraState = CameraState.Default;
            this.cubeController = CubeController.Default;
            this.animationEngine.Enabled = false;
            ResetAnimation();
        }

        public IEnumerable<Vector> ViewingPipeline(IEnumerable<Vector> vectorBuffer)
        {
            List<Vector> res = new List<Vector>();

            const float radianConverter = (float) Math.PI / 180f;

            float phiRads = this.cameraState.Phi     * radianConverter;
            float thetaRads = this.cameraState.Theta * radianConverter;

            foreach (Vector v in vectorBuffer)
            {
                Matrix view = Matrix.ViewMatrix(this.cameraState.Radius, phiRads, thetaRads);
                Vector currentVector = view * v;

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
            if (this.animationEngine.Enabled) { this.animationEngine.Update(); }
            
            //    The requirements did not specify input should be disabled during animation
            this.cubeController.Update(this.keyboardState);

            this.cameraState.Update(this.keyboardState);

            this.Refresh();
        }

        private void ResetAnimation()
        {
            this.animationEngine.Reset(this.cubeController, this.cameraState);
        }
    }
}