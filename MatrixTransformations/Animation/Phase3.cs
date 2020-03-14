using System;
using MatrixTransformations.Control;

namespace MatrixTransformations.Animation
{
    public class Phase3 : Animation
    {
        private CubeController cubeController;
        private Action update;
        private const float STEP_SIZE = 1f;

        private void ApproachTarget()
        {
            this.cubeController.RotateY += STEP_SIZE;

            if (this.cubeController.RotateY >= 45)
            {
                this.cubeController.RotateY = 45;
                this.update = DepartTarget;
            }
        }

        private void DepartTarget()
        {
            this.cubeController.RotateY += -STEP_SIZE;

            if (this.cubeController.RotateY <= 0)
            {
                this.cubeController.RotateY = 0;
                InvokeFinished();
            }
        }

        public Phase3(CubeController cubeController)
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