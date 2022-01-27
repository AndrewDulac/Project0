using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Project0
{

    /// <summary>
    /// class representing the background
    /// </summary>
    public class BackGround
    {
        private Texture2D texture;

        private double animationTimer;

        private float scale;

        private short animationFrame = 1;

        private string image;

        /// <summary>
        /// position of the background.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Creates a new coin sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public BackGround(string background)
        {
            image = background;
        }

        /// <summary>
        /// loads bat sprite texture
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            texture = content.Load<Texture2D>(image);
            if(texture.Width - graphics.Viewport.Width < texture.Height - graphics.Viewport.Height)
            {
                scale = (float)graphics.Viewport.Width / (float)texture.Width;
            }
            else { scale = graphics.Viewport.Height / texture.Height; }
        }

        /// <summary>
        /// draws animated sprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0, new Vector2(0,0), scale, SpriteEffects.None, 0);
        }
    }
}
