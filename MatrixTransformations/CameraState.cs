using System.Windows.Forms;
using MatrixTransformations.Control;

namespace MatrixTransformations
{
    public class CameraState
    {
        private float theta = -100f;
        private float phi = -10f;

        /// <summary>
        /// Distance from the viewer's eye and the view pane.
        /// Used to determine scaling of objects.
        /// </summary>
        public float D { get; set; } = 800;

        ///<summary>
        /// Distance from camera's position to (0,0,0)
        /// </summary>
        public float Radius { get; set; } = 10;

        /// <summary>
        /// Rotation around the z-axis in degrees.
        /// Is always a number between 0 and 360.
        /// </summary>
        public float Theta
        {
            get => this.theta;
            set => this.theta = value % 360;
        }

        /// <summary>
        /// Rotation around the x-axis in degrees
        /// Is always a number between 0 and 360.
        /// </summary>
        public float Phi
        {
            get => this.phi;
            set => this.phi = value % 360;
        }

        public void Update(KeyboardState keyboardState)
        {
            bool isShiftPressed = keyboardState.IsKeyPressed(Keys.ShiftKey);

            if (keyboardState.IsKeyPressed(Keys.R))
            {
                const float stepSize = .2f;
                Radius += isShiftPressed ? stepSize : -stepSize;
            }
            if (keyboardState.IsKeyPressed(Keys.D))
            {
                const float stepSize = 5f;
                D += isShiftPressed ? stepSize : -stepSize;
            }
            if (keyboardState.IsKeyPressed(Keys.T))
            {
                const float stepSize = 1;
                Theta += isShiftPressed ? stepSize : -stepSize;
            }
        }

        public static CameraState Default =>
            new CameraState
            {
                D = 800,
                Radius = 10,
                Theta = -100,
                Phi = -10
            };
    }
}