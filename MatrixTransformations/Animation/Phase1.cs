using System;
using MatrixTransformations.Control;

namespace MatrixTransformations.Animation
{
    public class Phase1 : Animation
    {
        private CubeController cubeController;
        private Action update;
        private bool finished;

        private void ApproachTarget()
        {
            const float target = 1.1f;

            this.cubeController.Scale += .01f;

            if (this.cubeController.Scale >= target)
            {
                this.cubeController.Scale = target;
                this.update = DepartTarget;
            }
        }

        private void DepartTarget()
        {
            this.cubeController.Scale += -.01f;

            if (this.cubeController.Scale <= 1)
            {
                this.cubeController.Scale = 1;
                InvokeFinished();
            }
        }

        public Phase1(CubeController cubeController)
        {
            this.cubeController = cubeController;
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