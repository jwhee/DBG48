using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    /// <summary>
    /// Cared
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Card image
        /// </summary>
        public Texture2D Texture { get; set; }

        public string Name { get; set; }
        public string Text { get; set; }

        public Card(Texture2D texture, string Name, string Text)
        {
            // TODO: Construct any child components here
            this.Texture = texture;
            this.Name = Name;
            this.Text = Text;
        }
    }
}
