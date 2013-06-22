using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    /// <summary>
    /// Card
    /// </summary>
    public class Card
    {
        public Texture2D Texture { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }

        public Card(Texture2D texture, string Name, string Text)
        {
            this.Texture = texture;
            this.Name = Name;
            this.Text = Text;
        }
    }
}
