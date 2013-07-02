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
                    if (card.Cost <= this.game.MainPlayer.ResourcePoint)
                    {
                        this.game.MainPlayer.SpendResourcePoint(card.Cost);
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

            // Create card animation from hand zone to play zone
            uint totalFrame = 40;
            Vector2 position = new Vector2(this.game.DISCARD_POSITION.X + 3, this.game.DISCARD_POSITION.Y + 3);
            CardAnimation anim = new CardAnimation(this.CardList[mouse_hover_index],
                                     this.getHandCardPosition(mouse_hover_index),
                                     position,
                                     totalFrame,
                                     1.0f,
                                     1.0f,
                                     this.cardRotation,
                                     this.cardRotation,
                                     true,
                                     Color.Black,
                                     "cardPlace1");
            this.game.AnimationList.Add(anim);
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
