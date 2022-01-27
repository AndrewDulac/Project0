using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Project0
{
    public enum DirectionEnum
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

    class InputManager
    {
        private KeyboardState currentKeyBoardState;
        private KeyboardState priorKeyBoardState;
        private GamePadState currentGamePadState;
        private GamePadState priorGamePadState;
        MouseState currentMouseState;
        MouseState priorMouseState;

        /// <summary>
        /// The player's direction
        /// </summary>
        public Vector2 Direction { get; private set; }

        public DirectionEnum DirectionState { get; private set; }
        
        /// <summary>
        /// If the user has requested to exit
        /// </summary>
        public bool Exit { get; private set; }

        /// <summary>
        /// If the player pressed warp
        /// </summary>
        public bool Warp { get; private set; }

        /// <summary>
        /// Update the input object
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public void Update(GameTime gameTime)
        {
            #region state
            // Update input state
            priorKeyBoardState = currentKeyBoardState;
            currentKeyBoardState = Keyboard.GetState();
            priorMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            priorGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(0);
            #endregion

            #region direction
            // TODO: Assign actions based on input
            // Right thumbstick
            Direction = Vector2.Zero;
            Direction += currentGamePadState.ThumbSticks.Left * 1;

            // WASD keys:
            if (currentKeyBoardState.IsKeyDown(Keys.Left) ||
                currentKeyBoardState.IsKeyDown(Keys.A))
                Direction += new Vector2(-1, 0);
            if (currentKeyBoardState.IsKeyDown(Keys.Right) ||
                currentKeyBoardState.IsKeyDown(Keys.D))
                Direction += new Vector2(1, 0);
            if (currentKeyBoardState.IsKeyDown(Keys.Up) ||
                currentKeyBoardState.IsKeyDown(Keys.W))
                Direction += new Vector2(0, -1);
            if (currentKeyBoardState.IsKeyDown(Keys.Down) ||
                currentKeyBoardState.IsKeyDown(Keys.S))
                Direction += new Vector2(0, 1);
            #endregion

            if (Math.Abs(Direction.X) > 0 || Math.Abs(Direction.Y) > 0)
            {
                // should probably change this to if statements to capture gamepadinput.

                if(Direction.X == 0 && Direction.Y < -.01f)
                    DirectionState = DirectionEnum.Up;

                else if (Direction.X == 0 && Direction.Y > .01f)
                    DirectionState = DirectionEnum.Down;

                else if (Direction.X < -.01f && Direction.Y == 0)
                    DirectionState = DirectionEnum.Left;

                else if (Direction.X > .01f && Direction.Y == 0)
                    DirectionState = DirectionEnum.Right;

                else if (Direction.X > .01f && Direction.Y < -.01f)
                    DirectionState = DirectionEnum.UpRight;

                else if (Direction.X > .01f && Direction.Y > .01f)
                    DirectionState = DirectionEnum.DownRight;

                else if (Direction.X < -.01f && Direction.Y > .01f)
                    DirectionState = DirectionEnum.DownLeft;

                else if (Direction.X < -.01f && Direction.Y < -.01f)
                    DirectionState = DirectionEnum.UpLeft;

                else DirectionState = DirectionEnum.Down;

                Direction = Direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Math.Abs(Direction.X) + Math.Abs(Direction.Y) > 1) Direction.Normalize();
            }


            #region warp
            Warp = false;
            // reset jump
            if (currentKeyBoardState.IsKeyDown(Keys.Space) &&
                priorKeyBoardState.IsKeyUp(Keys.Space))
                Warp = true;

            if (currentMouseState.LeftButton == ButtonState.Pressed &&
                priorMouseState.LeftButton == ButtonState.Released)
                Warp = true;

            if (currentGamePadState.IsButtonDown(Buttons.A) &&
                priorGamePadState.IsButtonUp(Buttons.A))
                Warp = true;
            #endregion

            #region exit
            if (currentGamePadState.Buttons.Back == ButtonState.Pressed || currentKeyBoardState.IsKeyDown(Keys.Escape))
                Exit = true;
            #endregion
        }
    }
}
