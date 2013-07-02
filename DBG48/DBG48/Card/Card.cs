using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DBG48
{
    /// <summary>
    /// Card
    /// </summary>
    public class Card
    {
        public Texture2D Texture { get; private set; }
        public string Name { get; private set; }
        public uint Cost { get; private set; }

        public uint ResourcePoint { get; private set; }
        public uint AttackPoint { get; private set; }
        public uint ActionPoint { get; private set; }

        public Card(
            Texture2D texture, 
            string Name, 
            uint Cost = 0,
            uint ActionPoint = 0,
            uint ResourcePoint = 0,
            uint AttackPoint = 0
            )
        {
            this.Texture = texture;
            this.Name = Name;
            this.Cost = Cost;
            
            this.ActionPoint = ActionPoint;
            this.ResourcePoint = ResourcePoint;
            this.AttackPoint = AttackPoint;
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

        private static Rectangle GetDestinationRectangle(Vector2 position, float scale)
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
