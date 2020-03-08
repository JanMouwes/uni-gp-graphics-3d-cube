using System;
using MatrixTransformations.Control;

namespace MatrixTransformations.Animation
{
    public class ResetPhase : Animation
    {
        private readonly CameraState cameraState;
        private Action update;
        private bool finished;
        private const float STEP_SIZE = 1f;

        private void ApproachTarget()
        {
            const float phiTarget = -10;
            const float thetaTarget = -100;

            bool phiReached = this.cameraState.Phi     >= phiTarget;
            bool thetaReached = this.cameraState.Theta <= thetaTarget;

            this.cameraState.Phi += -STEP_SIZE;
            this.cameraState.Theta += STEP_SIZE;

            if (phiReached) { this.cameraState.Phi = phiTarget; }

            if (thetaReached) { this.cameraState.Theta = thetaTarget; }

            if (phiReached && thetaReached) { InvokeFinished(); }
        }

        public ResetPhase(CameraState cameraState)
        {
            this.cameraState = cameraState;
            this.update = ApproachTarget;
        }

        public override void Update()
        {
            this.update();
        }

        public override void Reset()
        {
            this.finished = false;
        }
    }
}