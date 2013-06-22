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

        public void InitializeDeck(Queue<Card> deck)
        {
            this.Deck = deck;
        }

        public void DrawCard()
        {
            if (this.Deck.Count <= 0)
            {
                if (DiscardPile.Count > 0)
                    this.ShuffleDeck();
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
            foreach (Card card in this.DiscardPile)
            {
                this.Deck.Enqueue(card);
            }
            this.DiscardPile.Clear();
        }

        public void PlayCard(Card card)
        {
            this.Hand.Remove(card);
            this.DiscardPile.Add(card);
        }
    }
}
