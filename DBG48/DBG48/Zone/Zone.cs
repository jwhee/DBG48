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
        public List<Card> CardList;

        protected int zoneWidth = 480;
        protected int zoneHeight = 120;
        protected int cardDisplaySize = 5;
        protected int maxCardDisplaySize = 5;
        protected int cardDisplayStartIndex = 0;
        protected int maxDisplayStartIndex = 0;
        protected Color hoverFrameColor = Color.Black;
        protected float cardRotation = GameInstance.CARD_ROTATION;
        protected int cardDistance = 50;
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
            return new Vector2(30 + position.X + cardDistance * index + (GameInstance.CARD_WIDTH * GameInstance.CARD_SCALE / 2),
                               15 + position.Y + (GameInstance.CARD_HEIGHT * GameInstance.CARD_SCALE / 2));
        }

        public virtual void Update()
        {
            mouse_hover_index = -1;
            for (int i = 0; i < this.CardList.Count; i++)
            {
                if (i < this.cardDisplaySize)
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
                    int cardIndex = mouse_hover_index + this.cardDisplayStartIndex;
                    // Create overlay
                    game.currentGameState = GameState.OVERLAY;
                    Vector2 originPosition = getHandCardPosition(mouse_hover_index);
                    Vector2 goalPosition = new Vector2(game.GraphicsDevice.PresentationParameters.Bounds.Width / 2 - 200,
                                                       game.GraphicsDevice.PresentationParameters.Bounds.Height / 2);
                    game.cardSelectedOverlay = new CardSelectedOverlay(
                        this.game,
                        this.CardList[cardIndex],
                        originPosition,
                        goalPosition,
                        this.cardRotation,
                        0.0f);
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Debug
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, zoneWidth, zoneHeight);
            //spriteBatch.Draw(GameInstance.squareTexture, destinationRectangle, new Color(0, 255, 0, 50));

            // Draw hand
            for (int i = 0; i < this.cardDisplaySize; i++)
            {
                int cardIndex = i + this.cardDisplayStartIndex;

                if (mouse_hover_index != i && cardIndex < this.CardList.Count)
                {
                    this.CardList[cardIndex].Draw(
                        spriteBatch,
                        getHandCardPosition(i),
                        1.0f,
                        this.cardRotation,
                        Color.Black);
                }
            }

            // Draw hover
            if (game.currentGameState == GameState.PLAYABLE)
            {
                int cardIndex = mouse_hover_index + this.cardDisplayStartIndex;

                if (mouse_hover_index != -1
                    && mouse_hover_index < this.cardDisplaySize
                    && cardIndex < this.CardList.Count)
                {
                    Card card = this.CardList[cardIndex];
                    card.Draw(
                        spriteBatch,
                        getHandCardPosition(mouse_hover_index),
                        1.2f,
                        this.cardRotation,
                        this.hoverFrameColor);

                    this.DrawInfoBox(spriteBatch, card);
                }
            }
        }

        private void DrawInfoBox(SpriteBatch spriteBatch, Card card)
        {
            // Draw box
            Vector2 boxPos = this.getHandCardPosition(mouse_hover_index);
            spriteBatch.Draw(
                GameInstance.squareTexture,
                new Rectangle((int)boxPos.X - 75, (int)boxPos.Y, 50, 50),
                null,
                Color.OrangeRed);

            spriteBatch.Draw(
                GameInstance.squareTexture,
                new Rectangle((int)boxPos.X - 72, (int)boxPos.Y + 3, 44, 44),
                null,
                Color.LightGoldenrodYellow);

            // Draw Stats
            Vector2 textPos = new Vector2(boxPos.X - 72, boxPos.Y + 2);
            GameInstance.DrawText(
                spriteBatch,
                card.ActionPoint.ToString(),
                textPos,
                Color.Green,
                null,
                null,
                0.0f,
                0.7f);

            textPos.Y += 13;
            GameInstance.DrawText(
                spriteBatch,
                card.ResourcePoint.ToString(),
                textPos,
                Color.Orange,
                null,
                null,
                0.0f,
                0.7f);

            textPos.Y += 13;
            GameInstance.DrawText(
                spriteBatch,
                card.AttackPoint.ToString(),
                textPos,
                Color.DarkMagenta,
                null,
                null,
                0.0f,
                0.7f);
        }
    }
}
