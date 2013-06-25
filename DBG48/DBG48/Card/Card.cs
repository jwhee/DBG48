using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    /// <summary>
    /// Card
    /// </summary>
    public class Card
    {
        public Texture2D Texture { get; private set; }
        public string Name { get; private set; }
        public string Text { get; private set; }
        public uint ResourcePointCost { get; private set; }
        public uint AttackPointCost { get; private set; }

        public Card(
            Texture2D texture, 
            string Name, 
            string Text, 
            uint ResourcePointCost = 0,
            uint AttackPointCost = 0)
        {
            this.Texture = texture;
            this.Name = Name;
            this.Text = Text;
            this.ResourcePointCost = ResourcePointCost;
            this.AttackPointCost = AttackPointCost;
        }
    }
}
