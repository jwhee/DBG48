using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DBG48
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Controller : Microsoft.Xna.Framework.GameComponent
    {
        MouseState ms;
        private bool isLeftMouseClicked;
        private bool isRightMouseClicked;

        public Controller(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update()
        {
            MouseState next_ms = Mouse.GetState();

            this.isLeftMouseClicked = false;
            if (ms.LeftButton == ButtonState.Pressed && 
                next_ms.LeftButton == ButtonState.Released)
                isLeftMouseClicked = true;

            this.isRightMouseClicked = false;
            if (ms.RightButton == ButtonState.Pressed && 
                next_ms.RightButton == ButtonState.Released)
                isRightMouseClicked = true;

            ms = next_ms;
        }

        public bool isLeftMouseButtonClicked()
        {
            return this.isLeftMouseClicked;
        }

        public bool isLeftMouseButtonPressed()
        {
            return (ms.LeftButton == ButtonState.Pressed) ? true : false;
        }

        public bool isRightMouseButtonClicked()
        {
            return this.isRightMouseClicked;
        }

        public bool isRightMouseButtonPressed()
        {
            return (ms.RightButton == ButtonState.Pressed) ? true : false;
        }

        public bool isMouseInRegion(Rectangle region)
        {
            Rectangle actualRegion = new Rectangle(
                (int)(region.X - region.Width / 2),
                (int)(region.Y - region.Height / 2),
                region.Width,
                region.Height);
            if (ms.X > actualRegion.Left && 
                ms.X < actualRegion.Right && 
                ms.Y > actualRegion.Top && 
                ms.Y < actualRegion.Bottom)
                return true;
            return false;
        }
    }
}
