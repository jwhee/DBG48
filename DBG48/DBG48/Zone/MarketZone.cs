using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    public class MarketZone : Zone
    {
        public MarketZone(GameInstance game, Vector2 position)
            : base(game, position)
        {
            this.CardList = new List<Card>();

            // Specific MarketZone Constant
            this.cardRotation = 0.0f;
            this.cardDistance = 70;
            this.zoneWidth = 340;
        }

        public override void Update()
        {
            base.Update();
            // Right mouse click: Show CardSelectedOverlay
            if (mouse_hover_index != -1 && mouse_hover_index < this.cardDisplaySize)
            {
                // Left mouse click: Buy card
                if (game.controller.isLeftMouseButtonClicked())
                {
                    this.game.MainPlayer.BuyCard(this.CardList[mouse_hover_index]);
                    this.Restock(mouse_hover_index);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.cardDisplaySize = this.CardList.Count;

            base.Draw(spriteBatch);
        }

        public void Restock(int index)
        {
            CardList[index] = this.game.RandomlyGenerateCard();
        }
    }
}
