using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DBG48
{
    /// <summary>
    /// Card
    /// </summary>
    public class Card
    {
        public Texture2D Texture { get; private set; }
        public string Name { get; private set; }
        public string Text { get; private set; }
        public uint ResourcePointCost { get; private set; }
        public uint AttackPointCost { get; private set; }

        public Card(
            Texture2D texture, 
            string Name, 
            string Text, 
            uint ResourcePointCost = 0,
            uint AttackPointCost = 0)
        {
            this.Texture = texture;
            this.Name = Name;
            this.Text = Text;
            this.ResourcePointCost = ResourcePointCost;
            this.AttackPointCost = AttackPointCost;
        }

        public void Draw(
            SpriteBatch spriteBatch,
            Vector2 position,
            float scale = 1.0f,
            float rotation = 0.0f,
            Color? frameColor = null)
        {
            Rectangle frameRectangle = Card.GetDestinationRectangle(position, 1.07f * scale);
            Rectangle imageRectangle = Card.GetDestinationRectangle(position, 1.0f * scale);

            // Draw frame
            if (frameColor != null)
            {
                spriteBatch.Draw(
                    GameInstance.squareTexture,
                    frameRectangle,
                    null,
                    frameColor.Value,
                    rotation,
                    new Vector2(GameInstance.squareTexture.Width / 2, GameInstance.squareTexture.Height / 2),
                    SpriteEffects.None,
                    0.0f);
            }

            // Draw card image
            spriteBatch.Draw(
                this.Texture,
                imageRectangle, 
                null, 
                Color.White, 
                rotation, 
                new Vector2(this.Texture.Width / 2, this.Texture.Height / 2),
                SpriteEffects.None,
                0.0f);
        }

        public static Rectangle GetDestinationRectangle(Vector2 position, float scale)
        {
            int width = (int)(GameInstance.CARD_WIDTH * GameInstance.CARD_SCALE * scale);
            int height = (int)(GameInstance.CARD_HEIGHT * GameInstance.CARD_SCALE * scale);
            return new Rectangle((int)(position.X),
                                 (int)(position.Y),
                                 width,
                                 height);
        }
    }
}
