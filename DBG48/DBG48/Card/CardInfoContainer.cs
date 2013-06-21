using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DBG48
{
    /// <summary>
    /// Card
    /// </summary>
    public class CardInfoContainer
    {
        public string Filepath { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }

        public CardInfoContainer(string filepath, string Name, string Text)
        {
            this.Filepath = filepath;
            this.Name = Name;
            this.Text = Text;
        }
    }
}
