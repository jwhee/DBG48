using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    /// <summary>
    /// Zone
    /// </summary>
    public class Zone
    {
        protected GameInstance game;
        protected Vector2 position;

        protected int mouse_hover_index = -1;
        public List<Card> cardList;

        protected int zoneWidth = 515;
        protected int zoneHeight = 120;
        protected int cardDisplaySize;

        public Zone(GameInstance game, Vector2 position)
        {
            this.game = game;
            this.position = position;
        }

        public Rectangle getCardDestinationRectangle(Vector2 position, float scale)
        {
            int width = (int)(GameInstance.CARD_WIDTH * GameInstance.CARD_SCALE * scale);
            int height = (int)(GameInstance.CARD_HEIGHT * GameInstance.CARD_SCALE * scale);
            return new Rectangle((int)(position.X),
                                 (int)(position.Y),
                                 width,
                                 height);
        }

        public Vector2 getHandCardPosition(int index)
        {
            return new Vector2(25 + position.X + 50 * index + (GameInstance.CARD_WIDTH * GameInstance.CARD_SCALE / 2),
                               15 + position.Y + (GameInstance.CARD_HEIGHT * GameInstance.CARD_SCALE / 2));
        }

        public virtual void Update()
        {
            mouse_hover_index = -1;
            for (int i = 0; i < this.cardList.Count; i++)
            {
                if (i < this.cardList.Count)
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
                        this.cardList[mouse_hover_index],
                        originRectangle,
                        goalRectangle,
                        GameInstance.CARD_ROTATION,
                        0.0f);
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture;
            Rectangle destinationRectangle;
            Vector2 cardOrigin;

            // Debug
            texture = GameInstance.squareTexture;
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, zoneWidth, zoneHeight);
            //spriteBatch.Draw(texture, destinationRectangle, new Color(0, 255, 0, 50));

            // Draw hand
            for (int i = 0; i < this.cardDisplaySize; i++)
            {
                if (mouse_hover_index != i && i < this.cardList.Count)
                {
                    // Draw frame
                    texture = GameInstance.squareTexture;
                    destinationRectangle = getCardDestinationRectangle(getHandCardPosition(i), 1.07f);
                    cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, destinationRectangle, null, Color.Black, GameInstance.CARD_ROTATION, cardOrigin, SpriteEffects.None, 0.0f);

                    // Draw other cards
                    texture = this.cardList[i].Texture;
                    destinationRectangle = getCardDestinationRectangle(getHandCardPosition(i), 1.0f);
                    cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, destinationRectangle, null, Color.White, GameInstance.CARD_ROTATION, cardOrigin, SpriteEffects.None, 0.0f);
                }
            }

            // Draw hover
            if (game.currentGameState == GameState.PLAYABLE)
            {
                if (mouse_hover_index != -1
                    && mouse_hover_index < this.cardDisplaySize
                    && mouse_hover_index < this.cardList.Count)
                {
                    // Draw highlight
                    texture = GameInstance.squareTexture;
                    destinationRectangle = getCardDestinationRectangle(getHandCardPosition(mouse_hover_index), 1.3f);
                    cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, destinationRectangle, null, this.getHoverFrameColor(), GameInstance.CARD_ROTATION, cardOrigin, SpriteEffects.None, 0.0f);

                    // Draw hovered card
                    texture = this.cardList[mouse_hover_index].Texture;
                    destinationRectangle = getCardDestinationRectangle(getHandCardPosition(mouse_hover_index), 1.2f);
                    cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, destinationRectangle, null, Color.White, GameInstance.CARD_ROTATION, cardOrigin, SpriteEffects.None, 0.0f);
                }
            }
        }

        public void resetMouseHoverIndex()
        {
            this.mouse_hover_index = -1;
        }

        protected virtual Color getHoverFrameColor()
        {
            return Color.Black;
        }
    }
}
