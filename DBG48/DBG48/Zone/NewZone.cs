using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DBG48
{
    public class NewZone
    {
        protected GameInstance game;
        protected Vector2 position;

        protected int mouse_hover_index = -1;
        public List<Card> CardList;

        protected int zoneWidth = 280;
        protected int zoneHeight = 120;
        //protected int cardDisplaySize = 5;
        protected uint maxCardDisplaySize = 5;
        protected int cardDisplayStartIndex = 0;
        protected uint maxDisplayStartIndex = 0;
        protected Color hoverFrameColor = Color.Black;
        protected float cardRotation = GameInstance.CARD_ROTATION;
        protected uint cardDistance = 50;

        private float velocity = 2.0f;

        private Vector2[] positionList;
        private float[] scaleList;

        private float firstCardXPos;
        private float lastCardXPos;

        public NewZone(GameInstance game, List<Card> cardList, Vector2 position)
        {
            this.game = game;
            this.position = position;
            this.CardList = cardList;

            this.positionList = new Vector2[maxCardDisplaySize];
            this.scaleList = new float[maxCardDisplaySize];
            this.firstCardXPos = 30 + this.position.X + + 
                             (GameInstance.CARD_WIDTH * GameInstance.CARD_SCALE / 2);
            this.lastCardXPos = this.firstCardXPos + this.cardDistance * (this.maxCardDisplaySize - 1);
            for (int i = 0; i < this.maxCardDisplaySize; i++)
            {
                float xPos = 30 + this.position.X + this.cardDistance * i + 
                             (GameInstance.CARD_WIDTH * GameInstance.CARD_SCALE / 2);
                float yPos = 5 + this.position.Y + 
                             (GameInstance.CARD_HEIGHT * GameInstance.CARD_SCALE / 2);
                positionList[i] = new Vector2(xPos, yPos);

                scaleList[i] = 1.0f;
            }
        }

        private Rectangle getCardAreaRectangle(Vector2 position, float scale)
        {
            int width = (int)(GameInstance.CARD_WIDTH * GameInstance.CARD_SCALE * scale);
            int height = (int)(GameInstance.CARD_HEIGHT * GameInstance.CARD_SCALE * scale);
            return new Rectangle((int)(position.X),
                                 (int)(position.Y),
                                 width,
                                 height);
        }

        private void moveCard(bool isMovingRight)
        {
            if (isMovingRight)
            {
                for (int i = 0; i < positionList.Length; i++)
                {
                    this.positionList[i].X += this.velocity;

                    if (this.positionList[i].X > this.lastCardXPos)
                        this.positionList[i].X = this.lastCardXPos;
                }

                if (this.positionList.Length - 1 >= 0
                    && this.positionList.Length - 2 >= 0)
                {
                    float x1 = this.positionList[this.positionList.Length - 1].X;
                    float x2 = this.positionList[this.positionList.Length - 2].X;

                    if (Math.Abs(x1 - x2) < 0.0001f)
                    {
                        this.cardDisplayStartIndex--;
                        for (int i = 0; i < this.maxCardDisplaySize; i++)
                        {
                            float xPos = 30 + this.position.X + this.cardDistance * i +
                                         (GameInstance.CARD_WIDTH * GameInstance.CARD_SCALE / 2);
                            float yPos = 5 + this.position.Y +
                                         (GameInstance.CARD_HEIGHT * GameInstance.CARD_SCALE / 2);
                            positionList[i] = new Vector2(xPos, yPos);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < positionList.Length; i++)
                {
                    this.positionList[i].X -= this.velocity;

                    if (this.positionList[i].X < this.firstCardXPos)
                        this.positionList[i].X = this.firstCardXPos;
                }

                if (this.positionList.Length > 1)
                {
                    float x1 = this.positionList[0].X;
                    float x2 = this.positionList[1].X;

                    if (Math.Abs(x1 - x2) < 0.0001f)
                    {
                        this.cardDisplayStartIndex++;
                        for (int i = 0; i < this.maxCardDisplaySize; i++)
                        {
                            float xPos = 30 + this.position.X + this.cardDistance * i +
                                         (GameInstance.CARD_WIDTH * GameInstance.CARD_SCALE / 2);
                            float yPos = 5 + this.position.Y +
                                         (GameInstance.CARD_HEIGHT * GameInstance.CARD_SCALE / 2);
                            positionList[i] = new Vector2(xPos, yPos);
                        }
                    }
                }
            }
        }

        public virtual void Update()
        {
            mouse_hover_index = -1;
            for (int i = 0; i < this.maxCardDisplaySize; i++)
            {
                if (game.controller.isMouseInRegion(getCardAreaRectangle(positionList[i], 1.0f)))
                {
                    mouse_hover_index = i;
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
                    Vector2 originPosition = this.positionList[mouse_hover_index];
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

            if(this.game.controller.isMouseInRegion(getCardAreaRectangle(position, 2.0f)))
            {
                if(this.cardDisplayStartIndex < this.CardList.Count - this.maxCardDisplaySize)
                    this.moveCard(false);
            }
            else if (this.game.controller.isMouseInRegion(getCardAreaRectangle(new Vector2(position.X + zoneWidth, position.Y), 2.0f)))
            {
                if (this.cardDisplayStartIndex > 0)
                    this.moveCard(true);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Debug
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, zoneWidth, zoneHeight);
            spriteBatch.Draw(GameInstance.squareTexture, destinationRectangle, new Color(0, 255, 0, 50));

            // Draw hand
            for (int i = 0; i < this.maxCardDisplaySize; i++)
            {
                int cardIndex = i + this.cardDisplayStartIndex;

                if (mouse_hover_index != i && cardIndex < this.CardList.Count)
                {
                    this.CardList[cardIndex].Draw(
                        spriteBatch,
                        positionList[i],
                        scaleList[i],
                        this.cardRotation,
                        Color.Black);
                }
            }

            // Draw hover
            if (game.currentGameState == GameState.PLAYABLE)
            {
                int cardIndex = mouse_hover_index + this.cardDisplayStartIndex;

                if (mouse_hover_index != -1
                    && mouse_hover_index < this.maxCardDisplaySize
                    && cardIndex < this.CardList.Count)
                {
                    this.CardList[cardIndex].Draw(
                        spriteBatch,
                        positionList[mouse_hover_index],
                        1.2f * scaleList[mouse_hover_index],
                        this.cardRotation,
                        this.hoverFrameColor);
                }
            }
        }
    }
}
