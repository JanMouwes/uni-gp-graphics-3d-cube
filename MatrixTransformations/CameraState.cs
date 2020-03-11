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

        /// <summary>
        /// Change of Theta in degrees per tick.
        /// </summary>
        public float ThetaVelocity { get; set; }

        /// <summary>
        /// Change of Phi in degrees per tick.
        /// </summary>
        public float PhiVelocity { get; set; }

        public void Update()
        {
            this.Phi += PhiVelocity;
            this.Theta += ThetaVelocity;

            //    Decay
            ThetaVelocity *= .9f;
            PhiVelocity *= .9f;
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