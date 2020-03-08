using System;
using System.Collections.Generic;

namespace MatrixTransformations.Animation
{
    public delegate void PhaseEndedHandler(string phaseKey);

    public class AnimationEngine
    {
        public bool Enabled { get; set; }

        public event PhaseEndedHandler PhaseFinished;

        private readonly LinkedList<AnimationPhase> animationQueue;
        private LinkedListNode<AnimationPhase> currentPhaseNode;

        public AnimationPhase CurrentPhase => this.currentPhaseNode.Value;

        public AnimationEngine()
        {
            this.animationQueue = new LinkedList<AnimationPhase>();
        }

        public void AddPhase(string title, Animation animation)
        {
            this.animationQueue.AddLast(new AnimationPhase(title, animation));

            if (this.currentPhaseNode == null) { this.currentPhaseNode = this.animationQueue.First; }
        }

        public void Update()
        {
            if (!Enabled) { return; }

            this.CurrentPhase.Animation.Update();

            Console.WriteLine(CurrentPhase.Title);

            this.CurrentPhase.Animation.Finished += OnAnimationFinished;
        }

        private void OnAnimationFinished(Animation animation)
        {
            this.CurrentPhase.Animation.Finished -= OnAnimationFinished;

            PhaseFinished?.Invoke(CurrentPhase.Title);
            
            if (this.currentPhaseNode.Next != null)
            {
                this.currentPhaseNode = this.currentPhaseNode.Next;
            }
            else
            {
                this.Enabled = false;
                this.currentPhaseNode = null;
            }
        }


        public struct AnimationPhase
        {
            public string Title { get; }

            public Animation Animation { get; }

            public AnimationPhase(string title, Animation animation)
            {
                this.Title = title;
                this.Animation = animation;
            }
        }
    }
}