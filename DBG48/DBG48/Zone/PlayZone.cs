using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    public class PlayZone : Zone
    {
        public PlayZone(GameInstance game, Vector2 position)
            : base(game, position)
        {
            this.cardList = this.game.mainPlayer.DiscardPile;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.cardDisplaySize > this.cardList.Count)
                this.cardDisplaySize = this.cardList.Count;

            base.Draw(spriteBatch);
        }

        // For callback
        public void IncrementDisplaySize()
        {
            this.cardDisplaySize++;
        }
    }
}
