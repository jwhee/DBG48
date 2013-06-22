using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DBG48
{
    public enum GameState
    {
        PLAYABLE, ANIMATION, OVERLAY
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameInstance : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // constant
        private Vector2 DECK_POSITION = new Vector2(60, 420);
        private Vector2 HAND_POSITION = new Vector2(150, 400);
        public const float CARD_SCALE = 0.20f;
        public const int MAX_HAND_DISPLAY_SIZE = 9;
        private const int START_HAND_SIZE = 5;
        public const int START_DECK_SIZE = 15;
        public const int CARD_WIDTH = 320;
        public const int CARD_HEIGHT = 450;

        // Texture
        public static Texture2D squareTexture;
        public static Texture2D circleTexture;
        public static Texture2D cardbackTexture;
        public static Texture2D uiTexture;
        public static Texture2D buttonTexture;
        public static Texture2D cardTexture;
        public static Texture2D backgroundTexture;
        public static SpriteFont font;

        public static Random randGen;
        float elapsedTime = 0.0f;

        public Controller controller;
        public GameState currentGameState = GameState.PLAYABLE;

        public CardSelectedOverlay cardSelectedOverlay;
        public HandZone handZone;
        //public Queue<Card> deck;
        //public List<Card> discard;
        public PlayZone playZone;

        public Player mainPlayer;

        public GameInstance()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
            //Window.AllowUserResizing = true;
            Window.Title = "DBG48";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 500;

            controller = new Controller(this);
            randGen = new Random();
            mainPlayer = new Player(this);

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // font
            font = this.Content.Load<SpriteFont>("font");

            // initialize texture
            squareTexture = this.Content.Load<Texture2D>("square");
            circleTexture = this.Content.Load<Texture2D>("circle");
            uiTexture = this.Content.Load<Texture2D>("ui-icons");
            buttonTexture = this.Content.Load<Texture2D>("button-sprite");
            
            // Try load background
            backgroundTexture = squareTexture;
            string filePath = @"Content\Img\Resource\background.jpg";
            if (File.Exists(filePath))
            {
                FileStream stream = File.OpenRead(filePath);
                backgroundTexture = Texture2D.FromStream(GraphicsDevice, stream);
                stream.Close();
            }

            // Try load card back
            cardbackTexture = squareTexture;
            filePath = @"Content\Img\Resource\cardback.png";
            if (File.Exists(filePath))
            {
                FileStream stream = File.OpenRead(filePath);
                cardbackTexture = Texture2D.FromStream(GraphicsDevice, stream);
                stream.Close();
            }

            // Dynamic load pictures
            List<CardInfoContainer> cardInfoList = new List<CardInfoContainer>();
            if (Directory.Exists(@"Content\Img\Card\"))
            {
                string[] directories = Directory.GetDirectories(@"Content\Img\Card\");
                foreach (string directory in directories)
                {
                    string name = Path.GetFileName(directory);
                    string[] filePaths = Directory.GetFiles(directory, "*.jpg");
                    foreach (string path in filePaths)
                    {
                        cardInfoList.Add(new CardInfoContainer(path, name, ""));
                    }
                }
            }

            // Initialize deck
            Queue<Card> deck = new Queue<Card>();
            for (int i = 0; i < START_DECK_SIZE; i++)
            {
                int count = cardInfoList.Count;
                if (cardInfoList.Count > 0)
                {
                    int index = randGen.Next(count);
                    CardInfoContainer cardInfo = cardInfoList[index];
                    FileStream stream = File.OpenRead(cardInfo.Filepath);
                    Texture2D cardTexture = Texture2D.FromStream(GraphicsDevice, stream);
                    stream.Close();
                    deck.Enqueue(new Card(cardTexture, cardInfo.Name, cardInfo.Text));
                }
            }
            this.mainPlayer.InitializeDeck(deck);

            // initialize starting hand
            handZone = new HandZone(this, new Vector2(100, 350));
            playZone = new PlayZone(this, new Vector2(100, 100));

            // Draw starting hand?
            for (int i = 0; i < START_HAND_SIZE; i++)
            {
                mainPlayer.DrawCard();
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // DEV USE
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedTime > 16.6f) //60 FPS update rate
            {
                elapsedTime = 0.0f;

                // Update mouse
                controller.Update();

                // Check GameState
                if (currentGameState == GameState.OVERLAY)
                {
                    // Update CardSelectedOverlay
                    cardSelectedOverlay.Update();
                }
                else
                {
                    // Update handzone
                    handZone.Update();
                    playZone.Update();

                    // Check if we are drawing card from a deck
                    if(controller.isMouseInRegion(getCardDestinationRectangle(DECK_POSITION, 1.0f)))
                    {
                        if(controller.isLeftMouseButtonClicked())
                        {
                            this.mainPlayer.DrawCard();
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // BEGIN draw
            spriteBatch.Begin();
            
            // local-global variable for drawing sprites
            Texture2D texture;
            Vector2 position;
            Rectangle destinationRectangle;
            Vector2 cardOrigin;

            // Draw background
            spriteBatch.Draw(
                backgroundTexture,
                new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight),
                Color.White);
            spriteBatch.Draw(
                squareTexture,
                new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight),
                null,
                new Color(200, 200, 250, 100),
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);

            // Draw hand
            handZone.Draw(spriteBatch);
            playZone.Draw(spriteBatch);

            // Draw full deck
            int drawDeckSize = 10;
            for (int i = 0; i < drawDeckSize; i++)
            {
                texture = GameInstance.squareTexture;
                position = new Vector2(DECK_POSITION.X + i, DECK_POSITION.Y + i);
                destinationRectangle = getCardDestinationRectangle(position, 1.07f);
                cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                spriteBatch.Draw(texture, destinationRectangle, null, Color.Black, 0.0f, cardOrigin, SpriteEffects.None, 0.0f);

                texture = cardbackTexture;
                position = new Vector2(DECK_POSITION.X + i, DECK_POSITION.Y + i);
                destinationRectangle = getCardDestinationRectangle(position, 1.0f);
                cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                spriteBatch.Draw(texture, destinationRectangle, null, Color.White, 0.0f, cardOrigin, SpriteEffects.None, 0.0f);

                string text = this.mainPlayer.Deck.Count.ToString();
                spriteBatch.DrawString(
                    GameInstance.font, 
                    text,
                    new Vector2(DECK_POSITION.X, DECK_POSITION.Y + 25), 
                    Color.White, 
                    0.0f, 
                    new Vector2(font.MeasureString(text).X/2 - drawDeckSize, 0), 
                    1.0f, 
                    SpriteEffects.None, 
                    0.0f);
            }

            // Draw card selected overlay
            if (currentGameState == GameState.OVERLAY)
            {
                cardSelectedOverlay.Draw(spriteBatch);
            }

            // END draw
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private Vector2 getHandCardPosition(int index)
        {
            return new Vector2(HAND_POSITION.X + 50 * index, HAND_POSITION.Y);
        }
        
        private Rectangle getCardDestinationRectangle(Vector2 position, float scale)
        {
            return new Rectangle((int)(position.X),
                                 (int)(position.Y),
                                 (int)(CARD_WIDTH* CARD_SCALE * scale),
                                 (int)(CARD_HEIGHT * CARD_SCALE * scale));
        }

        public void returnToPlayable()
        {
            handZone.resetMouseHoverIndex();
            currentGameState = GameState.PLAYABLE;
        }
    }
}
