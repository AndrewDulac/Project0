using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Project0.Collisions;


namespace Project0
{

    public class StarSprite
    {
        private const float ANIMATION_SPEED = 0.08f;

        private double animationTimer;

        private bool grow = true;

        public float maxScale, minScale, scale;

        private Vector2 position;

        private Texture2D texture;

        private Vector2 rotatePoint;

        private BoundingCircle bounds;
        public bool Collected { get; set; } = false;

        /// <summary>
        /// bounding volume of the sprite
        /// </summary>
        public BoundingCircle Bounds => bounds;
        //lambda allows get, not set.


        /// <summary>
        /// Creates a new coin sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public StarSprite(Random rand, Rectangle validArea)
        {
            maxScale = (float)rand.NextDouble() * (.08f - .05f) + .05f;
            minScale = maxScale - .03f;

            this.rotatePoint = new Vector2(
                (float)rand.NextDouble() * (validArea.Right - validArea.Left) + validArea.Left, 
                (float)rand.NextDouble() * (validArea.Bottom - validArea.Top) + validArea.Top);
            this.position = this.rotatePoint + new Vector2((float)rand.NextDouble(),(float)rand.NextDouble())*5;
            this.bounds = new BoundingCircle(
                position,
                (float)rand.NextDouble() * (maxScale - minScale) + minScale);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("star");
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            double cosTheta = Math.Cos(.1);
            double sinTheta = Math.Sin(.1);
            position = new Vector2(
                (float)(cosTheta * (position.X - rotatePoint.X) -
                sinTheta * (position.Y - rotatePoint.Y) + rotatePoint.X),

                (float)(sinTheta * (position.X - rotatePoint.X) +
                cosTheta * (position.Y - rotatePoint.Y) + rotatePoint.Y)
            );
            // Update the bounds
            bounds.Center = position;

        }

        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Collected) { return; }
            SpriteEffects spriteEffects = SpriteEffects.None;

            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if(animationTimer > ANIMATION_SPEED)
            {
                if (grow && scale <= maxScale) scale += .01f;
                if (scale > maxScale) grow = !grow;
                if (!grow && scale >= minScale) scale -= .01f;
                if (scale < minScale) grow = !grow;
                
                animationTimer -= ANIMATION_SPEED;
            }
            spriteBatch.Draw(
                texture,
                position,
                null,
                Color.White,
                0,
                new Vector2(256,256),
                scale,
                spriteEffects,
                0
            );
        }
    }
}
