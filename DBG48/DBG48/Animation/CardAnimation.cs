using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    public class CardAnimation : Animation
    {
        private Card card;
        private Color? frameColor;

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
            ) : base(originPosition, goalPosition, totalFrame, originScale, goalScale, originRotation, goalRotation, rotateClockwise, animationFinishedSFXKey) 
        {
            this.card = card;
            this.frameColor = frameColor;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.card.Draw(
                spriteBatch,
                this.getPosition(),
                this.getScale(),
                this.getRotation(),
                this.frameColor);
        }
    }
}
