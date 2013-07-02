using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        PREGAME, PLAYABLE, ANIMATION, OVERLAY
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
        private Vector2 HANDZONE_POSITION = new Vector2(120, 360);
        private Vector2 PLAYZONE_POSITION = new Vector2(180, 240);
        private Vector2 MARKET_POSITION = new Vector2(170, 10);
        private Vector2 MARKET2_POSITION = new Vector2(185, 120);
        public Vector2 DISCARD_POSITION = new Vector2(120, 300);
        public const string RESOURCE_POINT_TEXT_1 = "Recruit";
        public const string RESOURCE_POINT_TEXT_2 = "Point";
        public const string ATTACK_POINT_TEXT_1 = "Production";
        public const string ATTACK_POINT_TEXT_2 = "Point";
        public const string ACTION_POINT_TEXT_1 = "Encore";
        public const string ACTION_POINT_TEXT_2 = "Point";
        private const string MARKET_NAME = "BACKSTAGE";
        public const float CARD_SCALE = 0.20f;
        public const int MAX_HAND_DISPLAY_SIZE = 8;
        private const int START_HAND_SIZE = 5;
        public const int START_DECK_SIZE = 15;
        public const int CARD_WIDTH = 320;
        public const int CARD_HEIGHT = 450;
        public const float CARD_ROTATION = 0.1f;

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
        private float elapsedTime = 0.0f;

        public Controller controller;
        public GameState currentGameState = GameState.PREGAME;
        private int pregameFrameLeft = 20;

        public CardSelectedOverlay cardSelectedOverlay;
        public HandZone HandZone;
        public PlayZone PlayZone;
        public MarketZone MarketZone;
        public MarketZone Market2Zone;

        public Player MainPlayer;
        public List<CardAnimation> AnimationList;

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
            MainPlayer = new Player(this);
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
            List<CardInfoContainer> cardInfoList = this.GenerateCardInfoList();

            // Initialize deck
            Queue<Card> deck = new Queue<Card>();
            for (int i = 0; i < START_DECK_SIZE; i++)
            {
                Card card = this.RandomlyGenerateCard(cardInfoList);
                deck.Enqueue(card);
            }
            this.MainPlayer.InitializeDeck(deck);

            // Initialize market deck
            MarketZone = new MarketZone(this, MARKET_POSITION);
            for (int i = 0; i < 4; i++)
            {
                Card card = this.RandomlyGenerateCard(cardInfoList);
                MarketZone.CardList.Add(card);
            }

            // Initialize market deck
            Market2Zone = new MarketZone(this, MARKET2_POSITION);
            for (int i = 0; i < 4; i++)
            {
                Card card = this.RandomlyGenerateCard(cardInfoList);
                Market2Zone.CardList.Add(card);
            }

            // initialize starting hand
            HandZone = new HandZone(this, HANDZONE_POSITION);
            PlayZone = new PlayZone(this, PLAYZONE_POSITION);
            
            // initialize animation list
            AnimationList = new List<CardAnimation>();

            // initialize sound engine
            if (Directory.Exists(@"Content\SFX\"))
            {
                string[] filePaths = Directory.GetFiles(@"Content\SFX\", "*");
                foreach(string path in filePaths)
                {
                    Match match = Regex.Match(path, @"Content\\SFX\\(.*)\.xnb", RegexOptions.None);
                    if (match.Success)
                    {
                        string key = match.Groups[1].Value;
                        SoundEngine.Instance.RegisterSoundEffect(key, Content.Load<SoundEffect>(@"SFX\"+key));
                    }
                }
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

                foreach (CardAnimation animation in AnimationList)
                {
                    animation.Update();
                }

                // Check GameState
                if (currentGameState == GameState.PREGAME)
                {
                    this.pregameFrameLeft--;

                    // Draw starting hand
                    if (pregameFrameLeft <= 0)
                    {
                        pregameFrameLeft = 20;
                        if (this.MainPlayer.Hand.Count < 5)
                        {
                            this.MainPlayer.DrawCard();
                        }
                        else
                        {
                            this.currentGameState = GameState.PLAYABLE;
                        }
                    }
                }
                else if (currentGameState == GameState.OVERLAY)
                {
                    // Update CardSelectedOverlay
                    cardSelectedOverlay.Update();
                }
                else
                {
                    // Update CardZones
                    HandZone.Update();
                    PlayZone.Update();
                    MarketZone.Update();
                    Market2Zone.Update();

                    // DEBUG: Drawing card from a deck
                    if(controller.isMouseInRegion(getCardDestinationRectangle(DECK_POSITION, 1.0f)))
                    {
                        if(controller.isLeftMouseButtonClicked())
                        {
                            this.MainPlayer.DrawCard();
                        }
                    }

                    // Check if we ending turn
                    if (controller.isMouseInRegion(getCardDestinationRectangle(DISCARD_POSITION, 1.0f)))
                    {
                        if (controller.isLeftMouseButtonClicked())
                        {
                            this.MainPlayer.EndTurn();
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

            // Draw Market
            texture = GameInstance.squareTexture;
            destinationRectangle = new Rectangle((int)MARKET_POSITION.X, (int)MARKET_POSITION.Y, 350, 230);
            spriteBatch.Draw(texture, destinationRectangle, new Color(0, 0, 50, 20));

            Vector2 marketTextPosition = new Vector2(font.MeasureString(MARKET_NAME).X + 10, 2);
            spriteBatch.DrawString(
                GameInstance.font,
                MARKET_NAME,
                new Vector2(MARKET_POSITION.X, MARKET_POSITION.Y),
                Color.MidnightBlue,
                0.0f - (float)Math.PI / 2,
                new Vector2(marketTextPosition.X + 2, marketTextPosition.Y + 1),
                1.0f,
                SpriteEffects.None,
                0.0f);
            spriteBatch.DrawString(
                GameInstance.font,
                MARKET_NAME,
                new Vector2(MARKET_POSITION.X, MARKET_POSITION.Y),
                Color.White,
                0.0f - (float)Math.PI/2,
                new Vector2(marketTextPosition.X, marketTextPosition.Y),
                1.0f,
                SpriteEffects.None,
                0.0f);

            MarketZone.Draw(spriteBatch);
            Market2Zone.Draw(spriteBatch);

            // Draw Card zones
            PlayZone.Draw(spriteBatch);
            HandZone.Draw(spriteBatch);

            #region Draw Player Deck
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
            }

            string text = this.MainPlayer.Deck.Count.ToString();
            spriteBatch.DrawString(
                GameInstance.font,
                text,
                new Vector2(DECK_POSITION.X, DECK_POSITION.Y + 25),
                Color.White,
                0.0f,
                new Vector2(font.MeasureString(text).X / 2 - drawDeckSize, 0),
                1.0f,
                SpriteEffects.None,
                0.0f);
            #endregion

            #region Draw Player Discard Pile
            texture = GameInstance.squareTexture;
            position = new Vector2(DISCARD_POSITION.X, DISCARD_POSITION.Y);
            destinationRectangle = getCardDestinationRectangle(position, 1.07f);
            cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, destinationRectangle, null, Color.Black, 0.0f, cardOrigin, SpriteEffects.None, 0.0f);

            texture = cardbackTexture;
            position = new Vector2(DISCARD_POSITION.X, DISCARD_POSITION.Y);
            destinationRectangle = getCardDestinationRectangle(position, 1.0f);
            cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, destinationRectangle, null, Color.White, 0.0f, cardOrigin, SpriteEffects.None, 0.0f);

            int count = Math.Min(this.MainPlayer.DiscardPile.Count, 5);
            if (count > 0)
            {
                int i;
                for (i = 0; i < count; i++)
                {
                    texture = GameInstance.squareTexture;
                    position = new Vector2(DISCARD_POSITION.X + i, DISCARD_POSITION.Y + i);
                    destinationRectangle = getCardDestinationRectangle(position, 1.07f);
                    cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                    spriteBatch.Draw(texture, destinationRectangle, null, Color.Black, 0.0f, cardOrigin, SpriteEffects.None, 0.0f);
                }

                texture = GameInstance.squareTexture;
                position = new Vector2(DISCARD_POSITION.X + i, DISCARD_POSITION.Y + i);
                destinationRectangle = getCardDestinationRectangle(position, 1.07f);
                cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                spriteBatch.Draw(texture, destinationRectangle, null, Color.Black, 0.0f, cardOrigin, SpriteEffects.None, 0.0f);

                texture = this.MainPlayer.DiscardPile[this.MainPlayer.DiscardPile.Count - 1].Texture;
                position = new Vector2(DISCARD_POSITION.X + i, DISCARD_POSITION.Y + i);
                destinationRectangle = getCardDestinationRectangle(position, 1.0f);
                cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                spriteBatch.Draw(texture, destinationRectangle, null, Color.White, 0.0f, cardOrigin, SpriteEffects.None, 0.0f);

                texture = GameInstance.squareTexture;
                position = new Vector2(DISCARD_POSITION.X + i, DISCARD_POSITION.Y + i);
                destinationRectangle = getCardDestinationRectangle(position, 1.0f);
                cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                spriteBatch.Draw(texture, destinationRectangle, null, new Color(50, 100, 100, 50), 0.0f, cardOrigin, SpriteEffects.None, 0.0f);
            }
            else
            {
                texture = GameInstance.squareTexture;
                position = new Vector2(DISCARD_POSITION.X, DISCARD_POSITION.Y);
                destinationRectangle = getCardDestinationRectangle(position, 1.0f);
                cardOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                spriteBatch.Draw(texture, destinationRectangle, null, new Color(50, 100, 100, 50), 0.0f, cardOrigin, SpriteEffects.None, 0.0f);
            }

            string discardPileText = "OFF";
            Vector2 discardPileTextPosition = new Vector2(DISCARD_POSITION.X - 80, DISCARD_POSITION.Y + 50);
            Vector2 offset = new Vector2(2, -1);
            GameInstance.DrawText(spriteBatch, discardPileText, discardPileTextPosition, Color.Black, null, offset, (float)(0 - Math.PI / 2));

            discardPileText = "STAGE";
            discardPileTextPosition = new Vector2(discardPileTextPosition.X + 18, discardPileTextPosition.Y);
            GameInstance.DrawText(spriteBatch, discardPileText, discardPileTextPosition, Color.Black, null, offset, (float)(0 - Math.PI / 2));
            #endregion // Draw Player Discard Pile

            #region Player Status
            // Background
            destinationRectangle = new Rectangle(20, 50, 130, 165);
            //destinationRectangle = new Rectangle(500, 350, 130, 115);
            spriteBatch.Draw(GameInstance.squareTexture, destinationRectangle, new Color(10, 10, 10, 30));

            // Resource Point
            Vector2 actionPointTextPosition = new Vector2(destinationRectangle.X + 10, destinationRectangle.Y + 10);
            GameInstance.DrawText(spriteBatch, ACTION_POINT_TEXT_1, actionPointTextPosition, Color.White, Color.Green);
            Vector2 actionPointText2Position = new Vector2(actionPointTextPosition.X, actionPointTextPosition.Y + 18);
            GameInstance.DrawText(spriteBatch, ACTION_POINT_TEXT_2, actionPointText2Position, Color.White, Color.Green);
            Vector2 actionPointValuePosition = new Vector2(actionPointTextPosition.X + font.MeasureString(RESOURCE_POINT_TEXT_2).X + 30, actionPointText2Position.Y);
            GameInstance.DrawText(spriteBatch, this.MainPlayer.ActionPoint.ToString(), actionPointValuePosition, Color.White, Color.Green);

            // Resource Point
            Vector2 resourcePointTextPosition = new Vector2(actionPointTextPosition.X, actionPointTextPosition.Y + 50);
            GameInstance.DrawText(spriteBatch, RESOURCE_POINT_TEXT_1, resourcePointTextPosition, Color.White, Color.Orange);
            Vector2 resourcePointText2Position = new Vector2(resourcePointTextPosition.X, resourcePointTextPosition.Y + 18);
            GameInstance.DrawText(spriteBatch, RESOURCE_POINT_TEXT_2, resourcePointText2Position, Color.White, Color.Orange);
            Vector2 resourcePointValuePosition = new Vector2(resourcePointTextPosition.X + font.MeasureString(RESOURCE_POINT_TEXT_2).X + 30, resourcePointText2Position.Y);
            GameInstance.DrawText(spriteBatch, this.MainPlayer.ResourcePoint.ToString(), resourcePointValuePosition, Color.White, Color.Orange);
            
            // Attack Point
            Vector2 attackPointTextPosition = new Vector2(resourcePointTextPosition.X, resourcePointTextPosition.Y + 50);
            GameInstance.DrawText(spriteBatch, ATTACK_POINT_TEXT_1, attackPointTextPosition, Color.LightYellow, Color.DarkMagenta);
            Vector2 attackPointText2Position = new Vector2(attackPointTextPosition.X, attackPointTextPosition.Y + 18);
            GameInstance.DrawText(spriteBatch, ATTACK_POINT_TEXT_2, attackPointText2Position, Color.LightYellow, Color.DarkMagenta);
            Vector2 attackPointValuePosition = new Vector2(resourcePointTextPosition.X + font.MeasureString(ATTACK_POINT_TEXT_2).X + 30, attackPointText2Position.Y);
            GameInstance.DrawText(spriteBatch, this.MainPlayer.AttackPoint.ToString(), attackPointValuePosition, Color.LightYellow, Color.DarkMagenta);
            
            #endregion // Player Status

            // Draw animation
            List<CardAnimation> newAnimationList = new List<CardAnimation>();
            foreach (CardAnimation animation in AnimationList)
            {
                animation.Draw(spriteBatch);
                if (!animation.Finished())
                {
                    newAnimationList.Add(animation);
                }
            }
            AnimationList = newAnimationList;

            // Draw card selected overlay
            if (currentGameState == GameState.OVERLAY)
            {
                cardSelectedOverlay.Draw(spriteBatch);
            }

            // END draw
            spriteBatch.End();

            base.Draw(gameTime);
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
            //HandZone.resetMouseHoverIndex();
            currentGameState = GameState.PLAYABLE;
        }

        private List<CardInfoContainer> GenerateCardInfoList()
        {
            List<CardInfoContainer> CardInfoList = new List<CardInfoContainer>();
            if (Directory.Exists(@"Content\Img\Card\"))
            {
                string[] directories = Directory.GetDirectories(@"Content\Img\Card\");
                foreach (string directory in directories)
                {
                    string name = Path.GetFileName(directory);
                    string[] filePaths = Directory.GetFiles(directory, "*.jpg");
                    foreach (string path in filePaths)
                    {
                        uint actionPoint = (uint)(randGen.Next(10) < 4 ? 1 : 0);
                        uint resourcePoint = (uint)randGen.Next(3) + 1;
                        uint attackPoint = (uint)randGen.Next(2) + 1;
                        uint cost = actionPoint*2 + resourcePoint + attackPoint;
                        CardInfoList.Add(new CardInfoContainer(path, name, cost, actionPoint, resourcePoint, attackPoint));
                    }
                }
            }
            return CardInfoList;
        }

        public Card RandomlyGenerateCard(List<CardInfoContainer> CardInfoList = null)
        {
            List<CardInfoContainer> cardInfoList = CardInfoList;
            if (cardInfoList == null)
            {
                cardInfoList = this.GenerateCardInfoList();
            }

            if (cardInfoList.Count > 0)
            {
                int index = randGen.Next(cardInfoList.Count);
                CardInfoContainer cardInfo = cardInfoList[index];
                FileStream stream = File.OpenRead(cardInfo.Filepath);
                Texture2D cardTexture = Texture2D.FromStream(GraphicsDevice, stream);
                stream.Close();

                return new Card(
                    cardTexture, 
                    cardInfo.Name, 
                    cardInfo.Cost,
                    cardInfo.ActionPoint,
                    cardInfo.ResourcePoint,
                    cardInfo.AttackPoint);
            }
            return new Card(squareTexture, "Empty Card");
        }

        public static void DrawText(
            SpriteBatch spriteBatch, 
            string text,
            Vector2 textPosition,
            Color? frontColor = null,
            Color? backColor = null,
            Vector2? offset = null,
            float rotation = 0.0f,
            float scale = 1.0f)
        {
            if (frontColor == null)
            {
                frontColor = Color.White;
            }

            Vector2 offsetPos = new Vector2(2, 1);
            if (offset != null)
            {
                offsetPos.X = offset.Value.X;
                offsetPos.Y = offset.Value.Y;
            }

            if (backColor != null)
            {
                spriteBatch.DrawString(
                    GameInstance.font,
                    text,
                    textPosition,
                    backColor.Value,
                    rotation,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0.0f);
            }

            spriteBatch.DrawString(
                GameInstance.font,
                text,
                new Vector2(textPosition.X + offsetPos.X, textPosition.Y + offsetPos.Y),
                frontColor.Value,
                rotation,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0.0f);
        }
    }
}
