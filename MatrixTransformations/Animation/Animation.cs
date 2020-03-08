namespace MatrixTransformations.Animation
{
    public delegate void AnimationFinishedHandler(Animation animation);

    public abstract class Animation
    {
        public abstract void Update();

        public event AnimationFinishedHandler Finished;
        
        public abstract void Reset();

        protected void InvokeFinished()
        {
            this.Finished?.Invoke(this);
        }
    }
}