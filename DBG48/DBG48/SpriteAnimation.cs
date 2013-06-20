//using System;
//using System.Collections.Generic;
//using System.Linq;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;


namespace DBG48
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteAnimation : Microsoft.Xna.Framework.GameComponent
    {
        private Texture2D texture;
        private int totalFrame;
        private int currentFrame;

        private Rectangle originRectangle;
        private Rectangle goalRectangle;

        public SpriteAnimation(Game game, Texture2D texture, Rectangle originRectangle, Rectangle goalRectangle, int totalFrame)
            : base(game)
        {
            this.texture = texture;
            this.totalFrame = totalFrame;
            this.originRectangle = originRectangle;
            this.goalRectangle = goalRectangle;

            this.currentFrame = 0;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // NOT USED

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update()
        {
            if(!Finished())
                currentFrame++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = getAnimatedCardDestinationRectangle(originRectangle, goalRectangle);
            Vector2 cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, destinationRectangle, null, Color.White, 0.0f, cardOrigin, SpriteEffects.None, 0.0f);
        }

        public bool Finished()
        {
            return (currentFrame < totalFrame) ? false : true;
        }

        private Rectangle getAnimatedCardDestinationRectangle(
            Rectangle originRectangle, Rectangle goalRectangle)
        {
            float deltaX = goalRectangle.X - originRectangle.X;
            float deltaY = goalRectangle.Y - originRectangle.Y;
            float deltaWidth = goalRectangle.Width - originRectangle.Width;
            float deltaHeight = goalRectangle.Height - originRectangle.Height;

            return new Rectangle((int)(originRectangle.X + (deltaX * currentFrame) / totalFrame),
                                 (int)(originRectangle.Y + (deltaY * currentFrame) / totalFrame),
                                 (int)(originRectangle.Width + (deltaWidth * currentFrame) / totalFrame),
                                 (int)(originRectangle.Height + (deltaHeight * currentFrame) / totalFrame));
        }
    }
}
