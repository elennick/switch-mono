using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switch.GameObjects.Tiles;

namespace Switch.GameObjects.Tiles
{
    class Tile : SpriteObject
    {
        private int boostY { get; set; } = 0;
        private int timeSinceSeated;
        private int idleTimer;
        private Random random;
        private int timeSinceLastDisplayedDrop = 0;

        public bool markedForDeletion { get; set; }
        public TileType type { get; }
        public int multiplier { get; set; }
        public bool seated { get; set; } //is this tile done dropping or not?
        public int age { get; set; } //measured in milliseconds, used to measure how long since tile was last moved
        public enum TileType { Normal, BottomCapper, TopCapper, Multiplier };
        public const int BaseScoreValue = 25;

        public int X { get; set; }
        public int Y { get; set; }

        public Tile(Tile tile) : base(tile.GetTexture(), tile.backgroundTexture)
        {      
            this.seated = false;
            this.age = 0;
            this.markedForDeletion = false;
            this.type = tile.type;
            this.multiplier = tile.multiplier;
            this.SetSpriteSheetDictionary(tile.GetSpriteSheetDictionary());
            ResetIdleTimer();
        }

        public Tile(Texture2D texture, TileType type) : base(texture)
        {
            this.seated = false;
            this.age = 0;
            this.markedForDeletion = false;
            this.type = type;
            ResetIdleTimer();
        }

        public Tile(Texture2D texture, Texture2D backgroundTexture, TileType type)
            : base(texture, backgroundTexture)
        {
            this.seated = false;
            this.age = 0;
            this.markedForDeletion = false;
            this.type = type;
            ResetIdleTimer();
        }

        public override void UpdateGameTime(int elapsedGameTime, int currentGameSpeed, int currentTilePixelHeight)
        {
            base.UpdateGameTime(elapsedGameTime, currentGameSpeed, currentTilePixelHeight);

            if (!this.seated)
            {
                int divisor = 4;

                this.timeSinceLastDisplayedDrop += elapsedGameTime;
                if (this.timeSinceLastDisplayedDrop >= currentGameSpeed / divisor)
                {
                    this.timeSinceLastDisplayedDrop = 0;
                    this.boostY = this.boostY += currentTilePixelHeight / divisor;
                }
            }
        }

        /**
         * Used to determine when to run an idle animation
         */ 
        public void ResetIdleTimer()
        {
            random = new Random();
            idleTimer = (random.Next(8) + 5) * 1000; //5 to 13 seconds
        }

        public void ResetAge()
        {
            this.age = 0;
        }

        public void SetY(int Y)
        {
            this.Y = Y;
            this.boostY = 0;
            this.timeSinceLastDisplayedDrop = 0;
        }

        public void BumpAge(int increment)
        {
            this.age += increment;

            if (this.seated)
            {
                this.timeSinceSeated = this.age;
            }

            this.idleTimer -= increment;

            if (this.idleTimer <= 0)
            {
                this.StartAnimation("idle", 5);
                ResetIdleTimer();
            }
        }

        public void MarkForDeletion()
        {
            this.markedForDeletion = true;
        }

        public int GetTimeSinceSeated()
        {
            return this.timeSinceSeated;
        }
    }
}
