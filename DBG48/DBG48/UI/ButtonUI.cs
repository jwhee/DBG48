using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ButtonUI
    {
        private GameInstance game;
        private Rectangle destinationRectangle;
        private int buttonType;
        private string text;

        private Rectangle hoverRectangle;
        private bool isHover = false;
        private bool isButtonClicked = false;
        private bool isButtonPressed = false;

        private Vector2 stringSize;

        public ButtonUI(GameInstance game, Rectangle destinationRectangle, int buttonType, string text)
        {
            this.game = game;
            this.destinationRectangle = destinationRectangle;
            this.buttonType = buttonType;
            this.text = text;

            this.hoverRectangle = new Rectangle(destinationRectangle.X, destinationRectangle.Y,
                destinationRectangle.Width - 10, destinationRectangle.Height - 20);

            this.stringSize = GameInstance.font.MeasureString(this.text);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update()
        {
            isHover = false;
            isButtonClicked = false;
            isButtonPressed = false;
            if (game.controller.isMouseInRegion(hoverRectangle))
            {
                isHover = true;
                if (game.controller.isLeftMouseButtonClicked())
                    isButtonClicked = true;
                if (game.controller.isLeftMouseButtonPressed())
                    isButtonPressed = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture;
            texture = GameInstance.buttonTexture;
            Rectangle sourceRectangle;
            Vector2 origin;

            // Draw button
            if (isHover && !isButtonPressed)
            {
                sourceRectangle = new Rectangle(0, 70 * buttonType, texture.Width, 70);
            }
            else
                sourceRectangle = new Rectangle(0, 70 * (buttonType + 5), texture.Width, 70);
            origin = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, origin, SpriteEffects.None, 0.0f);

            // Draw text
            origin.X = stringSize.X / 2;
            origin.Y = stringSize.Y / 2;
            spriteBatch.DrawString(GameInstance.font, this.text, new Vector2(destinationRectangle.X, destinationRectangle.Y), Color.Black, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }

        public bool isClicked()
        {
            return isButtonClicked;
        }
    }
}
