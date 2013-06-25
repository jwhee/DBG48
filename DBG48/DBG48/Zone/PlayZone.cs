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

            if (this.CardList.Count > this.maxCardDisplaySize)
            {
                if (game.controller.isLeftMouseButtonClicked())
                {
                    // Left Arrow
                    Vector2 tempPosition = getHandCardPosition(0);
                    Rectangle hoverRectangle = new Rectangle((int)tempPosition.X - 50, (int)tempPosition.Y, 30, 30);
                    if(this.game.controller.isMouseInRegion(hoverRectangle))
                    {
                        if (this.cardDisplayStartIndex > 0)
                            this.cardDisplayStartIndex--;
                    }

                    // Right Arrow
                    hoverRectangle = new Rectangle((int)tempPosition.X + 245, (int)tempPosition.Y, 30, 30);
                    if (this.game.controller.isMouseInRegion(hoverRectangle))
                    {
                        if (this.cardDisplayStartIndex + this.maxCardDisplaySize < this.CardList.Count
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
            Vector2 offset = new Vector2(2, 1);
            GameInstance.DrawText(spriteBatch, playAreaText, playAreaTextPosition, Color.White, Color.DarkRed, offset);

            base.Draw(spriteBatch);

            // Arrow buttons

            if (this.CardList.Count > this.maxCardDisplaySize)
            {
                Vector2 tempPosition = getHandCardPosition(0);

                // Arrow
                if (this.cardDisplayStartIndex > 0)
                {
                    texture = GameInstance.uiTexture;
                    destinationRectangle = new Rectangle((int)tempPosition.X - 50, (int)tempPosition.Y, 24, 24);
                    Rectangle hoverRectangle = new Rectangle(destinationRectangle.X, destinationRectangle.Y, 30, 30);
                    Vector2 uiOrigin = new Vector2(8, 8);

                    // hover
                    if (this.game.controller.isMouseInRegion(hoverRectangle)
                        && !this.game.controller.isLeftMouseButtonPressed())
                    {
                        spriteBatch.Draw(texture, hoverRectangle, new Rectangle(16 * 6, 16 * 0, 16, 16), Color.Black, 0.0f, uiOrigin, SpriteEffects.None, 0.0f);
                    }
                    else
                    {
                        spriteBatch.Draw(texture, destinationRectangle, new Rectangle(16 * 6, 16 * 0, 16, 16), Color.Black, 0.0f, uiOrigin, SpriteEffects.None, 0.0f);
                    }
                }

                if (this.cardDisplayStartIndex + this.maxCardDisplaySize < this.CardList.Count
                    && this.cardDisplayStartIndex < this.maxDisplayStartIndex)
                {
                    texture = GameInstance.uiTexture;
                    destinationRectangle = new Rectangle((int)tempPosition.X + 245, (int)tempPosition.Y, 24, 24);
                    Rectangle hoverRectangle = new Rectangle(destinationRectangle.X, destinationRectangle.Y, 30, 30);
                    Vector2 uiOrigin = new Vector2(8, 8);

                    // hover
                    if (this.game.controller.isMouseInRegion(hoverRectangle)
                        && !this.game.controller.isLeftMouseButtonPressed())
                    {
                        spriteBatch.Draw(texture, hoverRectangle, new Rectangle(16 * 2, 16 * 0, 16, 16), Color.Black, 0.0f, uiOrigin, SpriteEffects.None, 0.0f);
                    }
                    else
                    {
                        spriteBatch.Draw(texture, destinationRectangle, new Rectangle(16 * 2, 16 * 0, 16, 16), Color.Black, 0.0f, uiOrigin, SpriteEffects.None, 0.0f);
                    }
                }
            }
        }

        // For callback
        public void IncrementDisplaySize(object obj = null)
        {
            if (this.cardDisplaySize >= this.maxCardDisplaySize)
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
