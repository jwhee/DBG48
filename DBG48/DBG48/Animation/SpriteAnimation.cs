using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    public class SpriteAnimation : Animation
    {
        private Texture2D texture;

        public SpriteAnimation(
            Texture2D texture,
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
            : base(originPosition, goalPosition, totalFrame, originScale, goalScale, originRotation, goalRotation, rotateClockwise, animationFinishedSFXKey) 
        {
            this.texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                this.texture,
                this.getPosition(),
                null,
                Color.White,
                this.getRotation(),
                new Vector2(this.texture.Width / 2, this.texture.Height / 2),
                this.getScale(),
                SpriteEffects.None,
                0.0f);
        }
    }
}
