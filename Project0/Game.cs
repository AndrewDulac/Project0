using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Project0
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        InputManager inputManager;

        private List<StarSprite> stars;
        private PersonSprite player;
        private SpriteFont titleFont;
        private SpriteFont commonFont;
        private BackGround backGround;
        private int starsLeft;

        /// <summary>
        /// A game demonstrating collision detection
        /// </summary>
        public Game()
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
            inputManager = new InputManager();
            backGround = new BackGround("nightsky");
            stars = new List<StarSprite>();
            for(int i = 0; i < 15; i++)
            {
                stars.Add(new StarSprite(rand, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height / 2)));
            }
            starsLeft = stars.Count;
            player = new PersonSprite(new Vector2(30, graphics.GraphicsDevice.Viewport.Height - 50), 2f, this);

            base.Initialize();
        }

        /// <summary>
        /// Loads content for the game
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            foreach (var coin in stars) coin.LoadContent(Content);
            player.LoadContent(Content);
            backGround.LoadContent(Content, GraphicsDevice);
            titleFont = Content.Load<SpriteFont>("Britannic_Bold_Title");
            commonFont = Content.Load<SpriteFont>("Britannic_Bold_12");

        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            inputManager.Update(gameTime);
            // TODO: Add your update logic here
            player.Update(inputManager.Direction, inputManager.DirectionState, 200);

            player.Color = Color.White;
            //Detect and Process collisions
            foreach (var star in stars)
            {
                //possible to remove coin obj from game instead of hiding/ignoring it?
                if (!star.Collected && star.Bounds.CollidesWith(player.Bounds))
                {
                    player.Color = Color.Yellow;
                    star.Collected = true;
                    starsLeft--;
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
            string title = "Tasteless Game";
            Vector2 titleSize = titleFont.MeasureString(title);
            Vector2 titleLoc = new Vector2((GraphicsDevice.Viewport.Width/2 - titleSize.X/2), 20);
            string exitmsg = "Press ESC or the back button on your controller to exit the game.";
            Vector2 exitmsgSize = commonFont.MeasureString(exitmsg);
            Vector2 exitLoc = new Vector2((GraphicsDevice.Viewport.Width/2 - exitmsgSize.X/2), 30 + titleSize.Y);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            backGround.Draw(spriteBatch);
            spriteBatch.DrawString(titleFont, title, titleLoc, Color.LightSlateGray);
            spriteBatch.DrawString(commonFont, exitmsg, exitLoc, Color.LightSlateGray);
            foreach (var coin in stars) coin.Draw(gameTime, spriteBatch);
            player.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
