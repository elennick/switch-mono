using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switch.GameObjects;
using Switch.GameObjects.Challenges;

namespace Switch.GameObjects.GameDisplays
{
    class ChallengeStatusDisplay : GameDisplay
    {
        private IChallenge challenge;
        protected GameBoard gameBoard;

        public ChallengeStatusDisplay(Vector2 position, SpriteFont font, GameBoard gameBoard, IChallenge challenge)
            : base(position, font, gameBoard)
        {
            this.challenge = challenge;
            this.gameBoard = gameBoard;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(font,
                challenge.GetStatusText(gameBoard.GetStats()),
                position,
                new Color(217, 217, 217));
        }
    }
}
