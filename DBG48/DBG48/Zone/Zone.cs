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
    /// Zone
    /// </summary>
    public class Zone
    {
        protected GameInstance game;
        protected Vector2 position;

        public Zone(GameInstance game, Vector2 position)
        {
            this.game = game;
            this.position = position;
        }

        protected Rectangle getCardDestinationRectangle(Vector2 position, float scale)
        {
            return new Rectangle((int)(position.X),
                                 (int)(position.Y),
                                 (int)(GameInstance.CARD_WIDTH * GameInstance.CARD_SCALE * scale),
                                 (int)(GameInstance.CARD_HEIGHT * GameInstance.CARD_SCALE * scale));
        }
    }
}
