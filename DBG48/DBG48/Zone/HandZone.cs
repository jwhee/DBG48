using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    public class HandZone : Zone
    {
        Player mainPlayer;

        public HandZone(GameInstance game, Vector2 position)
            : base(game, position)
        {
            this.cardList = this.game.mainPlayer.Hand;
            this.mainPlayer = this.game.mainPlayer;
            this.cardDisplaySize = GameInstance.MAX_HAND_DISPLAY_SIZE;
        }

        public override void Update()
        {
            base.Update();
            // Right mouse click: Show CardSelectedOverlay
            if (mouse_hover_index != -1)
            {
                // Left mouse click: Play card
                if (game.controller.isLeftMouseButtonClicked())
                {
                    Rectangle originRectangle;
                    Rectangle goalRectangle;
                    SpriteAnimation anim;
                    int totalFrame = 15;
                    // Create card animation from hand zone to play zone
                    originRectangle = this.game.handZone.getCardDestinationRectangle(
                                        this.game.handZone.getHandCardPosition(mouse_hover_index), 1.07f);
                    goalRectangle = this.game.playZone.getCardDestinationRectangle(
                                        this.game.playZone.getHandCardPosition(this.game.playZone.cardList.Count), 1.07f);
                    anim = new SpriteAnimation(this.game,
                                               GameInstance.squareTexture,
                                               originRectangle,
                                               goalRectangle,
                                               totalFrame,
                                               GameInstance.CARD_ROTATION,
                                               GameInstance.CARD_ROTATION,
                                               true,
                                               Color.Black);
                    this.game.animationList.Add(anim);

                    originRectangle = this.game.handZone.getCardDestinationRectangle(
                                        this.game.handZone.getHandCardPosition(mouse_hover_index), 1.0f);
                    goalRectangle = this.game.playZone.getCardDestinationRectangle(
                                        this.game.playZone.getHandCardPosition(this.game.playZone.cardList.Count), 1.0f);
                    anim = new SpriteAnimation(this.game,
                                               this.mainPlayer.Hand[mouse_hover_index].Texture,
                                               originRectangle,
                                               goalRectangle,
                                               totalFrame,
                                               GameInstance.CARD_ROTATION,
                                               GameInstance.CARD_ROTATION,
                                               true,
                                               Color.White,
                                               this.game.playZone.IncrementDisplaySize);
                    this.game.animationList.Add(anim);

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

        protected override Color getHoverFrameColor()
        {
            return Color.HotPink;
        }
    }
}
