using System.Collections.Generic;

namespace MatrixTransformations.Animation
{
    public abstract class CompositeAnimation : Animation
    {
        public readonly Queue<Animation> AnimationQueue;
        private Animation currentAnimation;

        public CompositeAnimation() : this(new Queue<Animation>()) { }

        public CompositeAnimation(Queue<Animation> animationQueue)
        {
            this.AnimationQueue = animationQueue;
        }

        public override void Update()
        {
            if (this.currentAnimation == null) { return; }
            
            this.currentAnimation.Finished += animation =>
            {
                if (this.AnimationQueue.Count > 0) { this.currentAnimation = this.AnimationQueue.Dequeue(); }
                else
                {
                    this.currentAnimation.Finished += InvokeFinished;
                }
            };
            
            if (this.AnimationQueue.Count == 0) { }
            
            this.currentAnimation.Update();
        }

        private void InvokeFinished(Animation source)
        {
            base.InvokeFinished();
        }
    }
}