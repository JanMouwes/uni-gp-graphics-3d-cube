using System;
using MatrixTransformations.Control;

namespace MatrixTransformations.Animation
{
    public class Phase2 : Animation
    {
        private CubeController cubeController;
        private Action update;
        private const float STEP_SIZE = 1f;

        private void ApproachTarget()
        {
            this.cubeController.RotateX += STEP_SIZE;

            if (this.cubeController.RotateX >= 45)
            {
                this.cubeController.RotateX = 45;
                this.update = DepartTarget;
            }
        }

        private void DepartTarget()
        {
            this.cubeController.RotateX += -STEP_SIZE;

            if (this.cubeController.RotateX <= 0)
            {
                this.cubeController.RotateX = 0;
                InvokeFinished();
            }
        }

        public Phase2(CubeController cubeController)
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