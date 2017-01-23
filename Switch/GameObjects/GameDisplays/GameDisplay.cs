using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switch.GameObjects;

namespace Switch.GameObjects.GameDisplays
{
    abstract class GameDisplay
    {
        protected Vector2 position { get; set; }
        protected SpriteFont font { get; set; }
        protected GameBoard gameBoard;

        public GameDisplay(Vector2 position, SpriteFont font, GameBoard gameBoard)
        {
            this.position = position;
            this.font = font;
            this.gameBoard = gameBoard;
        }

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

        }
    }
}
