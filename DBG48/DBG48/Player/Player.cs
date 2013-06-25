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
        public uint ResourcePoint { get; private set; }
        public uint AttackPoint { get; private set; }

        private GameInstance game;

        public Player(GameInstance game)
        {
            this.game = game;
            this.Hand = new List<Card>();
            this.Deck = new Queue<Card>();
            this.DiscardPile = new List<Card>();

            // Debug
            this.ResourcePoint = 99;
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

            SoundEngine.Instance.PlaySoundEffect("cardSlide4");
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

            // Play Shuffle Sound
            SoundEngine.Instance.PlaySoundEffect("cardTakeOutPackage2");
        }

        public void PlayCard(Card card)
        {
            this.Hand.Remove(card);
            this.game.PlayZone.CardList.Add(card);
        }

        public void EndTurn()
        {
            // Cards in play pile enters discard pile
            foreach (Card card in this.game.PlayZone.CardList)
            {
                this.DiscardPile.Add(card);
            }

            foreach (Card card in this.Hand)
            {
                this.DiscardPile.Add(card);
            }

            // Reset play pile and hand
            this.game.PlayZone.Reset();
            this.Hand.Clear();
            this.ResourcePoint = 0;
            this.AttackPoint = 0;

            // End turn
            this.game.currentGameState = GameState.PREGAME;
        }

        public void SpendResourcePoint(uint cost)
        {
            this.ResourcePoint -= cost;
        }

        public void SpendAttackPoint(uint cost)
        {
            this.AttackPoint -= cost;
        }
    }
}
