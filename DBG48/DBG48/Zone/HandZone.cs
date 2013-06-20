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
    public class HandZone : Zone
    {
        private const int ZONE_WIDTH = 515;
        private const int ZONE_HEIGHT = 120;

        private int mouse_hover_index = -1;
        private Player mainPlayer;

        public HandZone(GameInstance game, Vector2 position)
            : base(game, position)
        {
            this.mainPlayer = this.game.mainPlayer;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update()
        {
            mouse_hover_index = -1;
            for (int i = 0; i < GameInstance.MAX_HAND_DISPLAY_SIZE; i++)
            {
                if (i < this.mainPlayer.Hand.Count)
                {
                    if (game.controller.isMouseInRegion(getCardDestinationRectangle(getHandCardPosition(i), 1.0f)))
                    {
                        mouse_hover_index = i;
                    }
                }
            }

            // Right mouse click: Show CardSelectedOverlay
            if (mouse_hover_index != -1)
            {
                if (game.controller.isRightMouseButtonClicked())
                {
                    // Create overlay
                    game.currentGameState = GameState.OVERLAY;
                    Rectangle originRectangle = getCardDestinationRectangle(getHandCardPosition(mouse_hover_index), 1.2f);
                    Rectangle goalRectangle = getCardDestinationRectangle(
                        new Vector2(game.GraphicsDevice.PresentationParameters.Bounds.Width / 2 - 200, 
                            game.GraphicsDevice.PresentationParameters.Bounds.Height / 2),
                        5.0f);
                    game.cardSelectedOverlay = new CardSelectedOverlay(
                        this.game,
                        this.mainPlayer.Hand[mouse_hover_index], 
                        originRectangle, 
                        goalRectangle);
                }

                // Left mouse click: Play card
                if (game.controller.isLeftMouseButtonClicked())
                {
                    this.mainPlayer.PlayCard(this.mainPlayer.Hand[mouse_hover_index]);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture;
            Rectangle destinationRectangle;
            Vector2 cardOrigin;

            // Debug
            texture = GameInstance.squareTexture;
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, ZONE_WIDTH, ZONE_HEIGHT);
            //spriteBatch.Draw(texture, destinationRectangle, new Color(0, 255, 0, 50));

            // Draw hand
            for (int i = 0; i < GameInstance.MAX_HAND_DISPLAY_SIZE; i++)
            {
                if (mouse_hover_index != i && i < this.mainPlayer.Hand.Count)
                {
                    // Draw frame
                    texture = GameInstance.squareTexture;
                    destinationRectangle = getCardDestinationRectangle(getHandCardPosition(i), 1.07f);
                    cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, destinationRectangle, null, Color.Black, 0.1f, cardOrigin, SpriteEffects.None, 0.0f);

                    // Draw other cards
                    texture = this.mainPlayer.Hand[i].Texture;
                    destinationRectangle = getCardDestinationRectangle(getHandCardPosition(i), 1.0f);
                    cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, destinationRectangle, null, Color.White, 0.1f, cardOrigin, SpriteEffects.None, 0.0f);
                }
            }

            // Draw hover
            if (game.currentGameState == GameState.PLAYABLE)
            {
                if (mouse_hover_index != -1 && mouse_hover_index < this.mainPlayer.Hand.Count)
                {
                    // Draw highlight
                    texture = GameInstance.squareTexture;
                    destinationRectangle = getCardDestinationRectangle(getHandCardPosition(mouse_hover_index), 1.3f);
                    cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, destinationRectangle, null, Color.HotPink, 0.0f, cardOrigin, SpriteEffects.None, 0.0f);

                    // Draw hovered card
                    texture = this.mainPlayer.Hand[mouse_hover_index].Texture;
                    destinationRectangle = getCardDestinationRectangle(getHandCardPosition(mouse_hover_index), 1.2f);
                    cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, destinationRectangle, null, Color.White, 0.0f, cardOrigin, SpriteEffects.None, 0.0f);
                }
            }

            // TEST: arrow buttons
            if (this.mainPlayer.Hand.Count > GameInstance.MAX_HAND_DISPLAY_SIZE)
            {
                texture = GameInstance.uiTexture;
                Vector2 tempPosition = getHandCardPosition(0);
                destinationRectangle = new Rectangle((int)tempPosition.X - 45, (int)tempPosition.Y, 24, 24);
                cardOrigin = new Vector2(8, 8);
                spriteBatch.Draw(texture, destinationRectangle, new Rectangle(16 * 6, 16 * 0, 16, 16), Color.Black, 0.0f, cardOrigin, SpriteEffects.None, 0.0f);

                texture = GameInstance.uiTexture;
                tempPosition = getHandCardPosition(0);
                destinationRectangle = new Rectangle((int)tempPosition.X + 445, (int)tempPosition.Y, 24, 24);
                cardOrigin = new Vector2(8, 8);
                spriteBatch.Draw(texture, destinationRectangle, new Rectangle(16 * 2, 16 * 0, 16, 16), Color.Black, 0.0f, cardOrigin, SpriteEffects.None, 0.0f);
            }
        }

        private Vector2 getHandCardPosition(int index)
        {
            return new Vector2(25 + position.X + 50 * index + (GameInstance.CARD_WIDTH * GameInstance.CARD_SCALE / 2),
                               15 + position.Y + (GameInstance.CARD_HEIGHT * GameInstance.CARD_SCALE / 2));
        }

        public void resetMouseHoverIndex()
        {
            this.mouse_hover_index = -1;
        }
    }
}
