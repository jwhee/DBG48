using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DBG48
{
    public class Animation
    {
        private uint totalFrame;
        private uint currentFrame;

        private Vector2 originPosition;
        private Vector2 goalPosition;

        private float originScale;
        private float goalScale;

        private float originRotation;
        private float goalRotation;
        private bool rotateClockwise;

        // Call when finished
        private Callback performAnimationFinishedAction;
        private string animationFinishedSFXKey;
        private object callbackParam;

        public Animation(
            Vector2 originPosition,
            Vector2 goalPosition,
            uint totalFrame,
            float originScale = 1.0f,
            float goalScale = 1.0f,
            float originRotation = 0.0f,
            float goalRotation = 0.0f,
            bool rotateClockwise = true,
            string animationFinishedSFXKey = null
            )
        {
            this.originPosition = originPosition;
            this.goalPosition = goalPosition;

            this.totalFrame = totalFrame;

            this.originScale = originScale;
            this.goalScale = goalScale;

            this.originRotation = originRotation;
            this.goalRotation = goalRotation;
            this.rotateClockwise = rotateClockwise;

            this.animationFinishedSFXKey = animationFinishedSFXKey;

            this.currentFrame = 0;
        }

        public void RegisterCallback(Callback cb, object obj = null)
        {
            this.performAnimationFinishedAction = cb;
            this.callbackParam = obj;
        }

        public void Update()
        {
            if (!this.Finished())
                currentFrame++;

            if (this.Finished())
            {
                if(performAnimationFinishedAction != null)
                    performAnimationFinishedAction(this.callbackParam);
                if(!string.IsNullOrWhiteSpace(this.animationFinishedSFXKey))
                    SoundEngine.Instance.PlaySoundEffect(this.animationFinishedSFXKey);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public bool Finished()
        {
            return (currentFrame < totalFrame) ? false : true;
        }

        protected float getRotation()
        {
            float rotation = this.originRotation;
            float progress = (float)this.currentFrame / (float)this.totalFrame;

            if (this.rotateClockwise)
                rotation += (this.goalRotation - this.originRotation) * progress;
            else
                rotation -= (this.originRotation - this.goalRotation) * progress;
            return rotation;
        }

        protected float getScale()
        {
            float progress = (float)this.currentFrame / (float)this.totalFrame;
            return this.originScale + (this.goalScale - this.originScale) * progress;
        }

        protected Vector2 getPosition()
        {
            float progress = (float)this.currentFrame / (float)this.totalFrame;

            float xPos = this.originPosition.X + (this.goalPosition.X - this.originPosition.X) * progress;
            float yPos = this.originPosition.Y + (this.goalPosition.Y - this.originPosition.Y) * progress;

            return new Vector2(xPos, yPos);
        }
    }
}
