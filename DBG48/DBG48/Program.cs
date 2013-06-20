using System;

namespace DBG48
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameInstance game = new GameInstance())
            {
                game.Run();
            }
        }
    }
#endif
}

