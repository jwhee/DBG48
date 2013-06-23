using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    public class PlayZone : Zone
    {
        public PlayZone(GameInstance game, Vector2 position)
            : base(game, position)
        {
            this.CardList = this.game.playPile;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.cardDisplaySize > this.CardList.Count)
                this.cardDisplaySize = this.CardList.Count;

            base.Draw(spriteBatch);
        }

        // For callback
        public void IncrementDisplaySize(object obj = null)
        {
            this.cardDisplaySize++;
        }
    }
}
