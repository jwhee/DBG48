using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DBG48
{
    public class HandZone : Zone
    {
        Player mainPlayer;

        public HandZone(GameInstance game, Vector2 position)
            : base(game, position)
        {
            this.CardList = this.game.MainPlayer.Hand;
            this.mainPlayer = this.game.MainPlayer;
            this.cardDisplaySize = GameInstance.MAX_HAND_DISPLAY_SIZE;
            this.hoverFrameColor = Color.HotPink;
        }

        public override void Update()
        {
            base.Update();
            // Right mouse click: Show CardSelectedOverlay
            if (mouse_hover_index != -1 && mouse_hover_index < this.cardDisplaySize)
            {
                // Left mouse click: Play card
                if (game.controller.isLeftMouseButtonClicked())
                {
                    Rectangle originRectangle;
                    Rectangle goalRectangle;
                    SpriteAnimation anim;
                    int totalFrame = 20;

                    // Create card animation from hand zone to play zone
                    int playZoneIndex = Math.Min(this.game.PlayZone.CardList.Count, 4);
                    originRectangle = this.getCardDestinationRectangle(
                                        this.getHandCardPosition(mouse_hover_index), 1.07f);
                    goalRectangle = this.game.PlayZone.getCardDestinationRectangle(
                                        this.game.PlayZone.getHandCardPosition(playZoneIndex), 1.07f);
                    anim = new SpriteAnimation(this.game,
                                               GameInstance.squareTexture,
                                               originRectangle,
                                               goalRectangle,
                                               totalFrame,
                                               GameInstance.CARD_ROTATION,
                                               GameInstance.CARD_ROTATION,
                                               true,
                                               Color.Black);
                    this.game.AnimationList.Add(anim);

                    originRectangle = this.getCardDestinationRectangle(
                                        this.getHandCardPosition(mouse_hover_index), 1.0f);
                    goalRectangle = this.game.PlayZone.getCardDestinationRectangle(
                                        this.game.PlayZone.getHandCardPosition(playZoneIndex), 1.0f);
                    anim = new SpriteAnimation(this.game,
                                               this.mainPlayer.Hand[mouse_hover_index].Texture,
                                               originRectangle,
                                               goalRectangle,
                                               totalFrame,
                                               GameInstance.CARD_ROTATION,
                                               GameInstance.CARD_ROTATION,
                                               true,
                                               Color.White,
                                               "cardSlide3");
                    anim.RegisterCallback(this.game.PlayZone.IncrementDisplaySize);
                    this.game.AnimationList.Add(anim);

                    // Play Card
                    this.mainPlayer.PlayCard(this.mainPlayer.Hand[mouse_hover_index]);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Texture2D texture;
            Rectangle destinationRectangle;
            Vector2 cardOrigin;

            // Arrow buttons
            if (this.CardList.Count > this.cardDisplaySize)
            {
                texture = GameInstance.uiTexture;
                Vector2 tempPosition = getHandCardPosition(0);
                destinationRectangle = new Rectangle((int)tempPosition.X - 50, (int)tempPosition.Y, 24, 24);
                cardOrigin = new Vector2(8, 8);
                spriteBatch.Draw(texture, destinationRectangle, new Rectangle(16 * 6, 16 * 0, 16, 16), Color.Black, 0.0f, cardOrigin, SpriteEffects.None, 0.0f);

                texture = GameInstance.uiTexture;
                tempPosition = getHandCardPosition(0);
                destinationRectangle = new Rectangle((int)tempPosition.X + 400, (int)tempPosition.Y, 24, 24);
                cardOrigin = new Vector2(8, 8);
                spriteBatch.Draw(texture, destinationRectangle, new Rectangle(16 * 2, 16 * 0, 16, 16), Color.Black, 0.0f, cardOrigin, SpriteEffects.None, 0.0f);
            }
        }
    }
}
