using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CollisionExample
{
    public class CollisionExampleGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        private CoinSprite[] coins;
        private SlimeGhostSprite slimeGhost;
        private SpriteFont spriteFont;
        private int coinsLeft;

        /// <summary>
        /// A game demonstrating collision detection
        /// </summary>
        public CollisionExampleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes the game 
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            System.Random rand = new System.Random();
            coins = new CoinSprite[]
            {
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
                new CoinSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height))
            };
            coinsLeft = coins.Length;
            slimeGhost = new SlimeGhostSprite(new Vector2(200,200), .25f);

            base.Initialize();
        }

        /// <summary>
        /// Loads content for the game
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            foreach (var coin in coins) coin.LoadContent(Content);
            slimeGhost.LoadContent(Content);
            spriteFont = Content.Load<SpriteFont>("arial");
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            slimeGhost.Update(gameTime);

            slimeGhost.Color = Color.White;
            //Detect and Process collisions
            foreach (var coin in coins)
            {
                //possible to remove coin obj from game instead of hiding/ignoring it?
                if (!coin.Collected && coin.Bounds.CollidesWith(slimeGhost.Bounds))
                {
                    slimeGhost.Color = Color.Red;
                    coin.Collected = true;
                    coinsLeft--;
                }
            }
           

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game world
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var coin in coins) coin.Draw(gameTime, spriteBatch);
            slimeGhost.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(spriteFont, $"Coins left: {coinsLeft}", new Vector2(2,2), Color.Gold);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
