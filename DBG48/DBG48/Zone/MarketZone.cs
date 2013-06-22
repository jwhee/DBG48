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

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.cardDisplaySize = this.CardList.Count;

            base.Draw(spriteBatch);
        }
    }
}
