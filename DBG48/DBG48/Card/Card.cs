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
            Rectangle frameRectangle,
            Rectangle imageRectangle,
            Color? frameColor = null,
            float rotation = 0.0f)
        {
            if (frameColor == null)
            {
                frameColor = Color.Black;
            }

            // Draw frame
            spriteBatch.Draw(
                GameInstance.squareTexture, 
                frameRectangle, 
                null, 
                frameColor.Value,
                rotation,
                new Vector2(GameInstance.squareTexture.Width / 2, GameInstance.squareTexture.Height / 2),
                SpriteEffects.None,
                0.0f);

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
    }
}
