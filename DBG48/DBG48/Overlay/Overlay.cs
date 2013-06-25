using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    public class Overlay
    {
        protected enum OverlayState
        {
            FLY_IN,
            NORMAL,
            FLY_OUT
        }
        
        protected GameInstance game;
        protected OverlayState state = OverlayState.FLY_IN;
        protected int currentFrame;

        public Overlay(GameInstance game)
        {
            this.game = game;
            this.state = OverlayState.FLY_IN;
            this.currentFrame = 0;
        }

        public virtual void Update()
        {
            throw new NotImplementedException();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
