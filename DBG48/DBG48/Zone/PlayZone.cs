using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    public class PlayZone : Zone
    {
        public PlayZone(GameInstance game, Vector2 position)
            : base(game, position)
        {
            this.CardList = new List<Card>();
            this.zoneWidth = 320;
        }

        public override void Update()
        {
            base.Update();

            if (this.CardList.Count > 5)
            {
                if (game.controller.isLeftMouseButtonClicked())
                {
                    // Left Arrow
                    Vector2 tempPosition = getHandCardPosition(0);
                    Rectangle destinationRectangle = new Rectangle((int)tempPosition.X - 50, (int)tempPosition.Y, 24, 24);
                    if(this.game.controller.isMouseInRegion(destinationRectangle))
                    {
                        if (this.cardDisplayStartIndex > 0)
                            this.cardDisplayStartIndex--;
                    }

                    // Right Arrow
                    destinationRectangle = new Rectangle((int)tempPosition.X + 245, (int)tempPosition.Y, 24, 24);
                    if (this.game.controller.isMouseInRegion(destinationRectangle))
                    {
                        if (this.cardDisplayStartIndex + 5 < this.CardList.Count
                            && this.cardDisplayStartIndex < this.maxDisplayStartIndex)
                            this.cardDisplayStartIndex++;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.cardDisplaySize > this.CardList.Count)
                this.cardDisplaySize = this.CardList.Count;

            Texture2D texture = GameInstance.squareTexture;
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y + zoneHeight / 3, zoneWidth, zoneHeight / 3);
            spriteBatch.Draw(texture, destinationRectangle, new Color(50, 20, 20, 50));

            string playAreaText = "MAINSTAGE AREA";
            Vector2 playAreaTextPosition = new Vector2(position.X + 30 , position.Y+ zoneHeight/3 - 15);
            spriteBatch.DrawString(
                GameInstance.font,
                playAreaText,
                playAreaTextPosition,
                Color.DarkRed,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);
            spriteBatch.DrawString(
                GameInstance.font,
                playAreaText,
                new Vector2(playAreaTextPosition.X + 2, playAreaTextPosition.Y + 1),
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);

            base.Draw(spriteBatch);

            // Arrow buttons
            
            if (this.CardList.Count > 5)
            {
                Vector2 tempPosition = getHandCardPosition(0);

                // Arrow
                if (this.cardDisplayStartIndex > 0)
                {
                    texture = GameInstance.uiTexture;
                    destinationRectangle = new Rectangle((int)tempPosition.X - 50, (int)tempPosition.Y, 24, 24);
                    Vector2 uiOrigin = new Vector2(8, 8);
                    spriteBatch.Draw(texture, destinationRectangle, new Rectangle(16 * 6, 16 * 0, 16, 16), Color.Black, 0.0f, uiOrigin, SpriteEffects.None, 0.0f);
                }

                if (this.cardDisplayStartIndex + 5 < this.CardList.Count
                    && this.cardDisplayStartIndex < this.maxDisplayStartIndex)
                {
                    texture = GameInstance.uiTexture;
                    destinationRectangle = new Rectangle((int)tempPosition.X + 245, (int)tempPosition.Y, 24, 24);
                    Vector2 uiOrigin = new Vector2(8, 8);
                    spriteBatch.Draw(texture, destinationRectangle, new Rectangle(16 * 2, 16 * 0, 16, 16), Color.Black, 0.0f, uiOrigin, SpriteEffects.None, 0.0f);
                }
            }
        }

        // For callback
        public void IncrementDisplaySize(object obj = null)
        {
            if (this.cardDisplaySize >= 5)
            {
                this.maxDisplayStartIndex++;
                this.cardDisplayStartIndex = this.maxDisplayStartIndex;
            }
            else
                this.cardDisplaySize++;
        }

        public void Reset()
        {
            this.CardList.Clear();
            this.cardDisplayStartIndex = 0;
            this.maxDisplayStartIndex = 0;
        }
    }
}
