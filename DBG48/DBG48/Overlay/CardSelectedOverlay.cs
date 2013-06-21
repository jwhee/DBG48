using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CardSelectedOverlay
    {
        private enum OverlayState
        {
            FLY_IN,
            NORMAL,
            FLY_OUT
        }
        
        GameInstance game;
        OverlayState state = OverlayState.FLY_IN;
        Card card;
        SpriteAnimation flyInAnimation;
        SpriteAnimation flyOutAnimation;
        int currentFrame;
        Rectangle cardRectangle { get; set; }
        private Rectangle viewPort;

        ButtonUI playButton;
        //ButtonUI resourceButton;

        public CardSelectedOverlay(GameInstance game, Card card, Rectangle originRectangle, Rectangle goalRectangle)
        {
            this.game = game;
            this.card = card;
            this.flyInAnimation = new SpriteAnimation(game, card.Texture, originRectangle, goalRectangle, 10);
            this.flyOutAnimation = new SpriteAnimation(game, card.Texture, goalRectangle, originRectangle, 5);
            this.cardRectangle = goalRectangle;
            this.state = OverlayState.FLY_IN;
            this.currentFrame = 0;
            this.viewPort = new Rectangle(game.GraphicsDevice.PresentationParameters.Bounds.Width / 2, game.GraphicsDevice.PresentationParameters.Bounds.Height/2, game.GraphicsDevice.PresentationParameters.Bounds.Width, game.GraphicsDevice.PresentationParameters.Bounds.Height);

            this.playButton = new ButtonUI(game, new Rectangle(550, 420, 300, 100), 1, "Play!");
            //this.resourceButton = new ButtonUI(game, new Rectangle(400, 400, 205, 50), 3, "Resource");
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization code here
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update()
        {
            if (currentFrame < 10)
                currentFrame++;

            switch (this.state)
            {
                case OverlayState.FLY_IN:
                    if (!this.flyInAnimation.Finished())
                        this.flyInAnimation.Update();
                    else
                        this.state = OverlayState.NORMAL;
                    break;
                case OverlayState.FLY_OUT:
                    if (!this.flyOutAnimation.Finished())
                        this.flyOutAnimation.Update();
                    else
                    {
                        this.game.returnToPlayable();
                    }
                    break;

                default:
                    playButton.Update();
                    if (playButton.isClicked())
                    {
                        break;
                    }
                    
                    if (this.game.controller.isLeftMouseButtonClicked())
                    {
                        if (this.game.controller.isMouseInRegion(this.cardRectangle))
                        {
                            // DO SOMETHING
                        }
                        else if(this.game.controller.isMouseInRegion(this.viewPort))
                        {
                            this.currentFrame = 0;
                            this.state = OverlayState.FLY_OUT;
                        }
                    }

                    if (this.game.controller.isRightMouseButtonClicked())
                    {
                        if (this.game.controller.isMouseInRegion(this.viewPort))
                        {
                            this.currentFrame = 0;
                            this.state = OverlayState.FLY_OUT;
                        }
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // DEBUG
            //spriteBatch.Draw(Game1.squareTexture, cardRectangle, null, new Color(0, 50, 0, 0), 0.0f, new Vector2(Game1.squareTexture.Width / 2, Game1.squareTexture.Height / 2), SpriteEffects.None, 0.0f);
            
            Texture2D texture;
            Vector2 origin;
            switch(this.state)
            {
                case OverlayState.FLY_IN:
                    // Darken background
                    texture = GameInstance.squareTexture;
                    origin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, viewPort, null, new Color(15, 0, 0, (int)(150.0f * (currentFrame) / 10.0f)), 0.0f, origin, SpriteEffects.None, 0.0f);
                    // Enlarging card image
                    this.flyInAnimation.Draw(spriteBatch);
                    break;

                case OverlayState.FLY_OUT:
                    // Whiten background
                    texture = GameInstance.squareTexture;
                    origin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, viewPort, null, new Color(15, 0, 0, (int)(150.0f - 150.0f * currentFrame / 10.0f)), 0.0f, origin, SpriteEffects.None, 0.0f);
                    // Retracting card image
                    this.flyOutAnimation.Draw(spriteBatch);
                    break;

                default:
                    // Static dark background
                    texture = GameInstance.squareTexture;
                    origin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, viewPort, null, new Color(15, 0, 0, 150), 0.0f, origin, SpriteEffects.None, 0.0f);
                    
                    // TEST: button...
                    playButton.Draw(spriteBatch);

                    // Card
                    texture = card.Texture;
                    origin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, cardRectangle, null, Color.White, 0.0f, origin, SpriteEffects.None, 0.0f);

                    // Text box
                    texture = GameInstance.squareTexture;
                    origin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, new Rectangle(580, 200, 380, 350), null, 
                        new Color(50, 50, 50, 0), 0.0f, origin, SpriteEffects.None, 0.0f);
                    spriteBatch.Draw(texture, new Rectangle(580, 200, 370, 340), null, 
                        new Color(100, 100, 100, 0), 0.0f, origin, SpriteEffects.None, 0.0f);

                    // Draw text
                    string text = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(card.Name));
                    origin.X = 0; //Game1.font.MeasureString(text).X/2;
                    origin.Y = 0; //Game1.font.MeasureString(text).Y/2;
                    spriteBatch.DrawString(GameInstance.font, text, new Vector2(420, 50), Color.Black, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);

                    text = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(card.Text));
                    spriteBatch.DrawString(GameInstance.font, text, new Vector2(420, 100), Color.Black, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
                    break;
            }
        }
    }
}
