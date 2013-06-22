using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    public delegate void Callback();

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteAnimation
    {
        private Texture2D texture;
        private int totalFrame;
        private int currentFrame;

        private Rectangle originRectangle;
        private Rectangle goalRectangle;

        private float originRotation;
        private float goalRotation;
        private bool rotateClockwise;

        private Color color;

        // Call when finished
        private Callback performAnimationFinishedAction;
        private string animationFinishedSFXKey;

        public SpriteAnimation(
            Game game,
            Texture2D texture,
            Rectangle originRectangle,
            Rectangle goalRectangle,
            int totalFrame,
            float originRotation = 0.0f,
            float goalRotation = 0.0f,
            bool rotateClockwise = true,
            Color? color = null,
            Callback animationFinishedAction = null,
            string animationFinishedSFXKey = null
            )
        {
            this.texture = texture;
            this.totalFrame = totalFrame;
            this.originRectangle = originRectangle;
            this.goalRectangle = goalRectangle;

            this.originRotation = originRotation;
            this.goalRotation = goalRotation;
            this.rotateClockwise = rotateClockwise;

            if (color == null)
                this.color = Color.White;
            else
                this.color = color.Value;

            this.performAnimationFinishedAction = animationFinishedAction;

            this.animationFinishedSFXKey = animationFinishedSFXKey;

            this.currentFrame = 0;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update()
        {
            if (!this.Finished())
                currentFrame++;

            if (this.Finished())
            {
                if(performAnimationFinishedAction != null)
                    performAnimationFinishedAction();
                if(!string.IsNullOrWhiteSpace(this.animationFinishedSFXKey))
                    SoundEngine.Instance.PlaySoundEffect(this.animationFinishedSFXKey);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = getAnimatedCardDestinationRectangle();
            Vector2 cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, destinationRectangle, null, this.color, getRotation(), cardOrigin, SpriteEffects.None, 0.0f);
        }

        public bool Finished()
        {
            return (currentFrame < totalFrame) ? false : true;
        }

        private Rectangle getAnimatedCardDestinationRectangle()
        {
            float deltaX = goalRectangle.X - originRectangle.X;
            float deltaY = goalRectangle.Y - originRectangle.Y;
            float deltaWidth = goalRectangle.Width - originRectangle.Width;
            float deltaHeight = goalRectangle.Height - originRectangle.Height;

            return new Rectangle((int)(originRectangle.X + (deltaX * currentFrame) / totalFrame),
                                 (int)(originRectangle.Y + (deltaY * currentFrame) / totalFrame),
                                 (int)(originRectangle.Width + (deltaWidth * currentFrame) / totalFrame),
                                 (int)(originRectangle.Height + (deltaHeight * currentFrame) / totalFrame));
        }

        private float getRotation()
        {
            float result = originRotation;

            if (rotateClockwise)
                result += (goalRotation - originRotation) * (float)currentFrame / (float)totalFrame;
            else
                result -= (originRotation - goalRotation) * (float)currentFrame / (float)totalFrame;
            return result;
        }
    }
}
