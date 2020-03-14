namespace MatrixTransformations.Animation
{
    public delegate void AnimationFinishedHandler(Animation animation);

    public abstract class Animation
    {
        public abstract void Update();

        public event AnimationFinishedHandler Finished;
        
        protected void InvokeFinished()
        {
            this.Finished?.Invoke(this);
        }
    }
}