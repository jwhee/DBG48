using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    public class MarketZone : Zone
    {
        public MarketZone(GameInstance game, Vector2 position)
            : base(game, position)
        {
            this.CardList = new List<Card>();

            // Specific MarketZone Constant
            this.cardRotation = 0.0f;
            this.cardDistance = 70;
            this.zoneWidth = 340;
        }

        public override void Update()
        {
            base.Update();
            // Right mouse click: Show CardSelectedOverlay
            if (mouse_hover_index != -1 && mouse_hover_index < this.cardDisplaySize)
            {
                // Left mouse click: Buy card
                if (game.controller.isLeftMouseButtonClicked())
                {
                    Card card = this.CardList[mouse_hover_index];
                    if (card.ResourcePointCost <= this.game.MainPlayer.ResourcePoint)
                    {
                        this.game.MainPlayer.SpendResourcePoint(card.ResourcePointCost);
                        this.PlayerBuyCard();
                    }
                    else
                    {
                        SoundEngine.Instance.PlaySoundEffect("error");
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.cardDisplaySize = this.CardList.Count;

            base.Draw(spriteBatch);
        }

        private void PlayerBuyCard()
        {
            SoundEngine.Instance.PlaySoundEffect("coin", 0.4f);

            // Animation
            Rectangle originRectangle;
            Rectangle goalRectangle;
            Vector2 position;
            SpriteAnimation anim;
            int totalFrame = 40;

            // Create card animation from hand zone to play zone
            originRectangle = this.getCardDestinationRectangle(
                                this.getHandCardPosition(mouse_hover_index), 1.07f);
            position = new Vector2(this.game.DISCARD_POSITION.X + 3, this.game.DISCARD_POSITION.Y + 3);
            goalRectangle = getCardDestinationRectangle(position, 1.07f);
            anim = new SpriteAnimation(this.game,
                                       GameInstance.squareTexture,
                                       originRectangle,
                                       goalRectangle,
                                       totalFrame,
                                       this.cardRotation,
                                       this.cardRotation,
                                       true,
                                       Color.Black);
            this.game.AnimationList.Add(anim);

            originRectangle = this.getCardDestinationRectangle(
                                this.getHandCardPosition(mouse_hover_index), 1.0f);
            goalRectangle = getCardDestinationRectangle(position, 1.0f);
            anim = new SpriteAnimation(this.game,
                                       this.CardList[mouse_hover_index].Texture,
                                       originRectangle,
                                       goalRectangle,
                                       totalFrame,
                                       this.cardRotation,
                                       this.cardRotation,
                                       true,
                                       Color.White,
                                       "cardPlace1");
            anim.RegisterCallback(MainPlayerBuyCard, this.CardList[mouse_hover_index]);
            this.game.AnimationList.Add(anim);

            // Restock card
            CardList[mouse_hover_index] = this.game.RandomlyGenerateCard();
        }

        private void MainPlayerBuyCard(object obj = null)
        {
            if (obj is Card)
            {
                this.game.MainPlayer.DiscardPile.Add(obj as Card);
            }
        }
    }
}
