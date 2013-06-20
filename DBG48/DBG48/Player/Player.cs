using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DBG48
{
    /// <summary>
    /// Player
    /// </summary>
    public class Player
    {
        public List<Card> Hand { get; private set; }
        public Queue<Card> Deck { get; private set; }
        public List<Card> DiscardPile { get; private set; }
        private GameInstance game;

        public Player(GameInstance game)
        {
            this.game = game;
            this.Hand = new List<Card>();
            this.Deck = new Queue<Card>();
            this.DiscardPile = new List<Card>();
        }

        public void InitializeDeck(List<Texture2D> cardImageList)
        {
            for (int i = 0; i < GameInstance.START_DECK_SIZE; i++)
            {
                Texture2D cardTexture = GameInstance.cardbackTexture;
                int count = cardImageList.Count;
                if (cardImageList.Count > 0)
                {
                    int index = GameInstance.randGen.Next(count);
                    cardTexture = cardImageList[index];
                }

                this.Deck.Enqueue(
                    new Card(
                      cardTexture, 
                      "", 
                      ""));
            }
        }

        public void DrawCard()
        {
            if (this.Deck.Count <= 0)
            {
                if (DiscardPile.Count > 0)
                    this.ShuffleDeck();
                else
                    return;
            }

            Hand.Add(this.Deck.Dequeue());
        }

        public void ShuffleDeck()
        {
            int n = this.DiscardPile.Count;
            while (n > 1)
            {
                n--;
                int k = GameInstance.randGen.Next(n + 1);
                Card temp = this.DiscardPile[k];
                this.DiscardPile[k] = this.DiscardPile[n];
                this.DiscardPile[n] = temp;
            }
            this.Deck = new Queue<Card>(this.DiscardPile);
            this.DiscardPile = new List<Card>();
        }

        public void PlayCard(Card card)
        {
            this.Hand.Remove(card);
            this.DiscardPile.Add(card);
        }
    }
}
