using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    public class CardSelectedOverlay : Overlay
    {
        private Card card;
        private CardAnimation flyInAnimation;
        private CardAnimation flyOutAnimation;
        private Vector2 goalCardPosition;

        // Format: Rectangle(center.X, center.Y, width, height)
        private Rectangle viewPort;

        ButtonUI playButton;
        //ButtonUI resourceButton;

        public CardSelectedOverlay(
            GameInstance game,
            Card card,
            Vector2 originCardPosition,
            Vector2 goalCardPosition,
            float originCardRotation = 0.0f, 
            float goalCardRotation = 0.0f)
            : base (game)
        {
            this.card = card;
            this.flyInAnimation = new CardAnimation(card,
                                                    originCardPosition,
                                                    goalCardPosition,
                                                    10,
                                                    1.0f,
                                                    5.0f,
                                                    originCardRotation,
                                                    goalCardRotation,
                                                    false);
            this.flyOutAnimation = new CardAnimation(card,
                                                    goalCardPosition,
                                                    originCardPosition,
                                                    10,
                                                    5.0f,
                                                    1.0f,
                                                    goalCardRotation,
                                                    originCardRotation,
                                                    true);
            this.goalCardPosition = goalCardPosition;
            this.viewPort = new Rectangle(game.GraphicsDevice.PresentationParameters.Bounds.Width / 2, game.GraphicsDevice.PresentationParameters.Bounds.Height/2, game.GraphicsDevice.PresentationParameters.Bounds.Width, game.GraphicsDevice.PresentationParameters.Bounds.Height);

            this.playButton = new ButtonUI(game, new Rectangle(550, 420, 300, 100), 1, "Play!");
            //this.resourceButton = new ButtonUI(game, new Rectangle(400, 400, 205, 50), 3, "Resource");
        }

        public override void Update()
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
                        if(this.game.controller.isMouseInRegion(this.viewPort))
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

        public override void Draw(SpriteBatch spriteBatch)
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
                    //playButton.Draw(spriteBatch);

                    // Card
                    card.Draw(spriteBatch, goalCardPosition, 5.0f);

                    // Text box
                    texture = GameInstance.squareTexture;
                    origin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, new Rectangle(580, 200, 380, 350), null, 
                        Color.Pink, 0.0f, origin, SpriteEffects.None, 0.0f);
                    spriteBatch.Draw(texture, new Rectangle(580, 200, 370, 340), null,
                        Color.LightYellow, 0.0f, origin, SpriteEffects.None, 0.0f);

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
