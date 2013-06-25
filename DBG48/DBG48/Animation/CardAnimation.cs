using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    public class CardAnimation
    {
        private Card card;
        private uint totalFrame;
        private uint currentFrame;

        private Vector2 originPosition;
        private Vector2 goalPosition;

        private float originScale;
        private float goalScale;

        private float originRotation;
        private float goalRotation;
        private bool rotateClockwise;

        private Color? frameColor;

        // Call when finished
        private Callback performAnimationFinishedAction;
        private string animationFinishedSFXKey;
        private object callbackParam;

        public CardAnimation(
            Card card,
            Vector2 originPosition,
            Vector2 goalPosition,
            uint totalFrame,
            float originScale = 1.0f,
            float goalScale = 1.0f,
            float originRotation = 0.0f,
            float goalRotation = 0.0f,
            bool rotateClockwise = true,
            Color? frameColor = null,
            string animationFinishedSFXKey = null
            )
        {
            this.card = card;

            this.originPosition = originPosition;
            this.goalPosition = goalPosition;

            this.totalFrame = totalFrame;

            this.originScale = originScale;
            this.goalScale = goalScale;

            this.originRotation = originRotation;
            this.goalRotation = goalRotation;
            this.rotateClockwise = rotateClockwise;

            this.frameColor = frameColor;

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

        public void Draw(SpriteBatch spriteBatch)
        {
            this.card.Draw(
                spriteBatch,
                this.getPosition(),
                this.getScale(),
                this.getRotation(),
                this.frameColor);
        }

        public bool Finished()
        {
            return (currentFrame < totalFrame) ? false : true;
        }

        private float getRotation()
        {
            float rotation = this.originRotation;
            float progress = (float)this.currentFrame / (float)this.totalFrame;

            if (this.rotateClockwise)
                rotation += (this.goalRotation - this.originRotation) * progress;
            else
                rotation -= (this.originRotation - this.goalRotation) * progress;
            return rotation;
        }

        private float getScale()
        {
            float progress = (float)this.currentFrame / (float)this.totalFrame;
            return this.originScale + (this.goalScale - this.originScale) * progress;
        }

        private Vector2 getPosition()
        {
            float progress = (float)this.currentFrame / (float)this.totalFrame;

            float xPos = this.originPosition.X + (this.goalPosition.X - this.originPosition.X) * progress;
            float yPos = this.originPosition.Y + (this.goalPosition.Y - this.originPosition.Y) * progress;

            return new Vector2(xPos, yPos);
        }
    }
}
