﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Project0.Collisions;

namespace Project0
{

    public enum Direction
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft,
    }

    /// <summary>
    /// A class representing a slime ghost
    /// </summary>
    public class PersonSprite
    {
        private GamePadState gamePadState;

        private float scale;

        private const int height = 32;

        private const int width = 32;

        private KeyboardState keyboardState;

        private Texture2D idleTexture;

        private bool running = false;

        private Texture2D runTexture;

        private Vector2 position;

        private BoundingRectangle bounds;

        private double animationTimer;

        private short animationFrame = 1;
        /// <summary>
        /// direction of the bat
        /// </summary>
        public Direction Direction;

        /// <summary>
        /// The color blend with the ghost;
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        public PersonSprite(Vector2 position, float scale)
        {
            this.position = position;
            this.scale = scale;
            this.bounds = new BoundingRectangle(
                position - new Vector2(scale * width/2, scale * height/2),
                (scale * width / 2),
                scale * height );
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            idleTexture = content.Load<Texture2D>("Player/IdleSheet");
            runTexture = content.Load<Texture2D>("Player/RunSheet");
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            gamePadState = GamePad.GetState(0);
            keyboardState = Keyboard.GetState();

            // Apply the gamepad movement with inverted Y axis
            position += gamePadState.ThumbSticks.Left * new Vector2(100 * (float)gameTime.ElapsedGameTime.TotalSeconds, -100 * (float)gameTime.ElapsedGameTime.TotalSeconds);
            running = false;

            // Apply keyboard movement
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) 
            { 
                position += new Vector2(0, -100 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                Direction = Direction.Up;
                running = true;
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                position += new Vector2(0, 100 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                Direction = Direction.Down;
                running = true;
            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                position += new Vector2(-100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                Direction = Direction.Left;
                running = true;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                position += new Vector2(100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                Direction = Direction.Right;
                running = true;
            }

            // Update the bounds
            bounds.X = position.X - ((width * scale) / 4);
            bounds.Y = position.Y - ((height * scale) / 2);
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;

            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer > 0.1)
            {
                animationFrame++;
                if (animationFrame > 7) animationFrame = 1;
                animationTimer -= 0.1;
            }
            var source = new Rectangle(animationFrame * 32, (int)Direction * 32 + 1, 32, 32);
            if (running) 
            {
                spriteBatch.Draw(
                    runTexture,
                    position,
                    source,
                    Color.White,
                    0,
                    new Vector2(width/2, height/2),
                    scale,
                    spriteEffects,
                    0
                );
            }
            else
            {
                spriteBatch.Draw(
                    idleTexture,
                    position,
                    source,
                    Color.White,
                    0,
                    new Vector2(width/2, height/2),
                    scale,
                    spriteEffects,
                    0
                );
            }
        }
    }
}
