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
            this.zoneWidth = 320;
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
        }

        // For callback
        public void IncrementDisplaySize(object obj = null)
        {
            this.cardDisplaySize++;
        }
    }
}
