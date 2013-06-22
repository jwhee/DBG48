using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace DBG48
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
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

        public void IncrementDisplaySize()
        {
            this.cardDisplaySize++;
        }
    }
}
