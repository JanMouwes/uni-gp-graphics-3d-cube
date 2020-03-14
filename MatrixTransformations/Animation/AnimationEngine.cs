using System;
using System.Collections.Generic;
using MatrixTransformations.Control;

namespace MatrixTransformations.Animation
{
    public class AnimationEngine
    {
        private int PhaseCounter { get; set; }

        public bool Enabled { get; set; }
        public Animation Current { get; set; }

        private Phase1 phase1;
        private Phase2 phase2;
        private Phase3 phase3;
        private ResetPhase resetPhase;

        private CameraState cameraState;

        private event Action update;
        public event Action Finished;

        public AnimationEngine(CameraState cameraState, CubeController cubeController)
        {
            Reset(cubeController, cameraState);
        }

        public void Reset(CubeController cubeController, CameraState newCameraState)
        {
            PhaseCounter = 1;
            
            this.cameraState = newCameraState;

            this.phase1 = new Phase1(cubeController);
            this.phase2 = new Phase2(cubeController);
            this.phase3 = new Phase3(cubeController);
            this.resetPhase = new ResetPhase(newCameraState);

            Current = this.phase1;
            Current.Finished += HandleFinished;

            this.update = null;
            update += DecreaseTheta;
            update += Current.Update;
        }


        private void DecreaseTheta() => this.cameraState.Theta -= 1;

        private void IncreasePhi() => this.cameraState.Phi += 1;

        private void HandleFinished(Animation finishedAnimation)
        {
            Current.Finished -= HandleFinished;

            this.update -= Current.Update;

            switch (this.PhaseCounter)
            {
                case 1:
                    Current = this.phase2;

                    break;
                case 2:
                    Current = this.phase3;

                    update -= DecreaseTheta;
                    update += IncreasePhi;

                    break;
                case 3:
                    Current = this.resetPhase;
                    update -= IncreasePhi;

                    break;
                case 4:
                    this.Finished?.Invoke();
                    
                    return; // Stop the current animation
            }

            this.update += Current.Update;
            this.Current.Finished += HandleFinished;

            this.PhaseCounter++;
        }

        public void Update()
        {
            if (!Enabled) { return; }

            update?.Invoke();
        }

        public override string ToString()
        {
            return "Current phase: " + PhaseCounter;
        }
    }
}