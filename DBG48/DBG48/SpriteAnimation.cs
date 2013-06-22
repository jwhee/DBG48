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
    public delegate void Callback();
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

        private float originRotation;
        private float goalRotation;
        private bool rotateClockwise;

        private Color color;
        private Callback animationFinishedAction;

        public SpriteAnimation(
            Game game,
            Texture2D texture,
            Rectangle originRectangle,
            Rectangle goalRectangle,
            int totalFrame,
            float originRotation = 0.0f,
            float goalRotation = 0.0f,
            bool rotateClockwise = true,
            Color? color = null,
            Callback animationFinishedAction = null
            )
            : base(game)
        {
            this.texture = texture;
            this.totalFrame = totalFrame;
            this.originRectangle = originRectangle;
            this.goalRectangle = goalRectangle;

            this.originRotation = originRotation;
            this.goalRotation = goalRotation;
            this.rotateClockwise = rotateClockwise;

            this.animationFinishedAction = animationFinishedAction;

            if (color == null)
                this.color = Color.White;
            else
                this.color = color.Value;
            this.currentFrame = 0;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update()
        {
            if (!this.Finished())
                currentFrame++;
            
            if (animationFinishedAction != null && this.Finished())
                animationFinishedAction();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = getAnimatedCardDestinationRectangle();
            Vector2 cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, destinationRectangle, null, this.color, getRotation(), cardOrigin, SpriteEffects.None, 0.0f);
        }

        public bool Finished()
        {
            return (currentFrame < totalFrame) ? false : true;
        }

        private Rectangle getAnimatedCardDestinationRectangle()
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

        private float getRotation()
        {
            float result = originRotation;

            if (rotateClockwise)
                result += (goalRotation - originRotation) * (float)currentFrame / (float)totalFrame;
            else
                result -= (originRotation - goalRotation) * (float)currentFrame / (float)totalFrame;
            return result;
        }
    }
}
