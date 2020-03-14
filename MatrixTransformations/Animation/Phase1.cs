using System;
using MatrixTransformations.Control;

namespace MatrixTransformations.Animation
{
    public class Phase1 : Animation
    {
        private CubeController cubeController;
        private Action update;

        private void ApproachTarget()
        {
            const float target = 1.5f;

            this.cubeController.Scale += .01f;

            if (this.cubeController.Scale >= target)
            {
                this.cubeController.Scale = target;
                this.update = DepartTarget;
            }
        }

        private void DepartTarget()
        {
            const float target = 1;
            this.cubeController.Scale += -.01f;

            if (this.cubeController.Scale <= target)
            {
                this.cubeController.Scale = target;
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
    }
}