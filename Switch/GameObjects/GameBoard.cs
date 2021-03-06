﻿using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Switch.GameObjects.Tiles;
using Switch.GameObjects;
using Switch.GameObjects.Sound;
using Switch.GameObjects.GameDisplays;
using Switch;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects
{
    class GameBoard
    {
        private Vector2 position { get; set; } //top left corner of the board, might vary if we are utilizing effects such as the board shaking
        private Vector2 originalPosition; //top left corner of the board, should always stay the same after init
        private TileSet tileSet; //a set of tiles for this gameboard to utilize
        private Rectangle gameBoardRect; //a Rectangle object representing the size of the board
        private Rotater rotater; //the object at the bottom of the board that swaps rows
        private GameMessageBoxDisplay messageBox;
        private List<SpriteObject> animatableSprites;
        private int height { get; } //height of the board in pixels
        private int width { get; } //width of the board in pixels
        private int numTilesHeight; //number of tiles high that this board has as a max
        private int numTilesWidth; //number of tiles across that this board has as a max
        private int startingSpeed; //the number of milliseconds to wait before moving tiles down initially
        private int currentSpeed; //the number of milliseconds to wait before moving tiles down currently
        private int timeBeforeSpeedUp; //the amount of milliseconds before the game increments up in speed
        private int speedUpIncrement; //the amount of time removed before each tile drop... higher means the game will speed up more each time
        private int minGameSpeed; //the point at which the game stops speeding things up
        private int timeSinceLastSpeedUp;
        private int playerIndex; //the player controlling this game board
        private int numTilesToDrop; //how many tiles to drop everytime a new set come down
        private TileList tiles; //a List of all tiles currently on the board
        private bool paused;
        private bool scaleTiles; // whether or not to scale tile graphics to fit the board size
        private bool bulletTimeActive;
        private int bulletTimeLeft;
        private bool gameOver;
        private int timeDownHeld;
        private GameboardStats stats;
        private bool speedupEnabled;
        private bool isShaking;
        private int timeLeftShaking;
        private int timeSinceLastShake;
        private DetailedSpriteObject boardBackgroundSpriteObject;
        private DetailedSpriteObject nukeSpriteObject;
        private DetailedSpriteObject blankWhiteSpritObject;
        private int maxBulletTime;
        private Random random;
        private float fadeFromWhiteAlpha;
        private Difficulty difficulty;

        /**
         * Constructor
         */
        public GameBoard(Vector2 boardPosition,
                         TileSet tileSet,
                         Difficulty difficulty,
                         int width,
                         int height,
                         int playerIndex)
        {
            //set constructor params
            this.position = boardPosition;
            this.originalPosition = boardPosition;
            this.difficulty = difficulty;
            this.height = height;
            this.width = width;
            this.numTilesHeight = difficulty.GetNumberOfTilesInTheGameboardHeight();
            this.numTilesWidth = difficulty.GetNumberOfTilesInTheGameboardWidth();
            this.startingSpeed = difficulty.GetStartingSpeed(); //milliseconds
            this.currentSpeed = startingSpeed;
            this.playerIndex = playerIndex;
            this.tileSet = tileSet;

            //init stuff
            this.random = new Random();
            this.animatableSprites = new List<SpriteObject>();
            this.timeBeforeSpeedUp = 60000; //game speeds up every 60 seconds by default
            this.speedUpIncrement = 50; //tiles drop 1/20th of a second faster every time the game speeds up
            this.minGameSpeed = difficulty.GetMaxSpeed();
            this.numTilesToDrop = difficulty.GetNumberOfTilesToDropPerRound();
            this.paused = true;
            this.scaleTiles = false;
            this.bulletTimeActive = false;
            this.bulletTimeLeft = 0;
            this.gameOver = false;
            this.timeDownHeld = 0;
            this.speedUpIncrement = 15;
            this.timeBeforeSpeedUp = 60000;
            this.speedupEnabled = true;
            this.minGameSpeed = 150;
            this.timeSinceLastSpeedUp = 0;
            this.isShaking = false;
            this.timeLeftShaking = 0;
            this.timeSinceLastShake = 0;
            this.fadeFromWhiteAlpha = 1;
            this.maxBulletTime = 15000; //bullet time is 15 seconds long by default

            this.rotater = new Rotater(this.tileSet.rotater, 0, this.numTilesWidth - 2, 1);
            this.animatableSprites.Add(this.rotater);

            int startingPower;
            if (!SwitchGame.DEBUG_MODE)
            {
                startingPower = 0;
            }
            else
            {
                startingPower = 100;
            }
            this.stats = new GameboardStats(startingPower);
            this.tiles = new TileList(numTilesWidth, numTilesHeight, stats);

            this.gameBoardRect = new Rectangle((int)boardPosition.X,
                                               (int)boardPosition.Y,
                                               (int)(boardPosition.X + width),
                                               (int)(boardPosition.Y + height));

            boardBackgroundSpriteObject = new DetailedSpriteObject(tileSet.boardBackground, new Rectangle((int)(boardPosition.X - 10), 0, width + 20, height + 111));
            nukeSpriteObject = new DetailedSpriteObject(tileSet.nukeImage, new Rectangle((int)originalPosition.X, (int)originalPosition.Y, width, height));
            blankWhiteSpritObject = new DetailedSpriteObject(tileSet.blankImage, new Rectangle((int)originalPosition.X, (int)originalPosition.Y, width, height));
        }

        public int GetPlayerIndex()
        {
            return this.playerIndex;
        }

        public Difficulty GetDifficulty()
        {
            return this.difficulty;
        }

        public void SetMessageBoxDisplay(GameMessageBoxDisplay messageBox)
        {
            this.messageBox = messageBox;
        }

        public void AddMessageBoxMessage(String message, Color color)
        {
            if (this.messageBox != null)
            {
                this.messageBox.AddMessage(message, color);
            }
        }

        public void AddMessageBoxMessage(String message)
        {
            AddMessageBoxMessage(message, new Color(217, 217, 217));
        }

        public GameboardStats GetStats()
        {
            return this.stats;
        }

        public int GetMaxBulletTime()
        {
            return this.maxBulletTime;
        }

        public Rectangle GetRect()
        {
            return this.gameBoardRect;
        }

        public void SetSpeedUpTimer(int milliseconds)
        {
            timeBeforeSpeedUp = milliseconds;
        }

        public void SetSpeedUpEnabled(bool enabled)
        {
            this.speedupEnabled = enabled;
        }

        public bool IsSpeedUpEnabled()
        {
            return this.speedupEnabled;
        }

        public TileSet GetTileSet()
        {
            return this.tileSet;
        }

        public float GetTilePixelWidth()
        {
            return this.width / this.numTilesWidth;
        }

        public float GetTilePixelHeight()
        {
            return this.height / this.numTilesHeight;
        }

        public void SetScaleTiles(bool scaleTiles)
        {
            this.scaleTiles = scaleTiles;
        }

        public bool IsScaleTiles()
        {
            return this.scaleTiles;
        }

        public void SetPaused(bool paused)
        {
            this.paused = paused;
        }

        public int GetNumTilesToDrop()
        {
            return this.numTilesToDrop;
        }

        public void SetNumTilesToDrop(int num)
        {
            this.numTilesToDrop = num;
        }

        public int GetScore()
        {
            return this.stats.score;
        }

        public int GetPower()
        {
            return this.stats.power;
        }

        public int GetBulletTimeLeft()
        {
            return this.bulletTimeLeft;
        }

        public bool IsGameOver()
        {
            return this.gameOver;
        }

        public bool IsBulletTimeOn()
        {
            return this.bulletTimeActive;
        }

        public bool IsNukeAnimationOn()
        {
            return this.isShaking;
        }

        public int GetCurrentSpeed()
        {
            int speed = this.currentSpeed;

            if (this.timeDownHeld > 200)
            {
                speed /= 4;
            }
            else if (this.bulletTimeActive)
            {
                speed *= 3;
            }

            return speed;
        }

        public Rectangle GetTileRectangle(Tile tile)
        {
            int thisTilePosX = (int)(this.position.X + (this.GetTilePixelWidth() * tile.X));
            int thisTilePosY = (int)(this.position.Y + (this.GetTilePixelHeight() * tile.Y));
            Vector2 thisTilesPosition = new Vector2(thisTilePosX, thisTilePosY);

            return new Rectangle(thisTilePosX, thisTilePosY, (int)GetTilePixelWidth(), (int)GetTilePixelHeight());
        }

        //this method is really ineffecient, needs to be refactored badly
        public void DropNewTileSet(int numTiles)
        {
            if (numTiles >= this.numTilesWidth)
            {
                numTiles = this.numTilesWidth - 1;
            }

            //get random tile types to drop
            Tile[] tilesToDrop = this.GetRandomTiles(numTiles);

            //drop them into different columns
            List<int> columnsToDropIn = new List<int>();

            while (true)
            {
                int columnToDropIn = random.Next(numTilesWidth);

                if (!columnsToDropIn.Contains(columnToDropIn))
                {
                    columnsToDropIn.Add(columnToDropIn);
                    if (columnsToDropIn.Count >= numTiles)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            //check to see if any of the columns we dropped into are already full
            for (int i = 0; i < columnsToDropIn.Count; i++)
            {
                if (tiles.IsColumnFull(columnsToDropIn[i]))
                {
                    this.paused = true;
                    this.gameOver = true;
                }
            }

            //update the tile grid with the new tiles
            for (int i = 0; i < columnsToDropIn.Count; i++)
            {
                tilesToDrop[i].X = columnsToDropIn[i];
                tilesToDrop[i].SetY(0);
                tiles.Add(tilesToDrop[i], columnsToDropIn[i]);
            }
        }

        public void ShakeBoard(int millisToShakeFor)
        {
            if (!this.isShaking)
            {
                this.isShaking = true;
                this.timeLeftShaking = millisToShakeFor;
                this.timeSinceLastShake = 0;
                this.SetPaused(true);
                this.fadeFromWhiteAlpha = 1;
            }
        }

        public void LevelUp()
        {
            this.currentSpeed = (int)MathHelper.Clamp(this.currentSpeed - this.speedUpIncrement, this.minGameSpeed, this.startingSpeed);
            this.timeSinceLastSpeedUp = 0;
            AddMessageBoxMessage("Level Up!", Color.Silver);
            this.stats.level++;
            SoundManager.Instance.PlaySound("levelup");
        }

        /**
         * Called by the screen that is utilizing this board during every Update() cycle
         */
        public void Update(GameTime gameTime)
        {
            int elapsedTime = gameTime.ElapsedGameTime.Milliseconds;

            stats.timeElapsed += elapsedTime;
            timeDownHeld += elapsedTime;
            this.timeSinceLastSpeedUp += elapsedTime;

            //let all sprite objects know the current elapsed game time in case they need to update a frame
            foreach (SpriteObject sprite in animatableSprites)
            {
                sprite.UpdateGameTime(elapsedTime);
            }

            foreach (Tile sprite in tiles.GetTilesAsList())
            {
                sprite.UpdateGameTime(elapsedTime, this.GetCurrentSpeed(), (int)this.GetTilePixelHeight());
            }

            AnimationManager.Instance.UpdateGameTime(elapsedTime);
            VibrationManager.Instance.Update(elapsedTime);

            //make sure the power level isnt over max
            this.stats.power = (int)MathHelper.Clamp(this.stats.power, 0, 100);

            //shake the board if shaking is enabled
            if (this.isShaking)
            {
                VibrationManager.Instance.VibrateController((PlayerIndex)this.playerIndex, 200);

                if (this.timeLeftShaking <= 0)
                {
                    this.isShaking = false;
                    SetPaused(false);
                    this.position = this.originalPosition;
                }
                else if (this.timeSinceLastShake >= 75)
                {
                    this.timeSinceLastShake = 0;
                    int xVariation = random.Next(20) - 10;
                    int YVariation = random.Next(20) - 10;

                    this.position = new Vector2(this.originalPosition.X + xVariation,
                                                     this.originalPosition.Y + YVariation);
                }

                this.fadeFromWhiteAlpha -= 0.005f;
                this.timeLeftShaking -= elapsedTime;
                this.timeSinceLastShake += elapsedTime;
            }

            //if bullet time is currently engaged, check to see if it is time to turn it off
            if (this.bulletTimeActive)
            {
                this.bulletTimeLeft -= elapsedTime;
                if (this.bulletTimeLeft <= 0)
                {
                    this.StopBulletTime();
                }
            }

            //this is the core logic that defines gameplay
            if (!this.paused)
            {
                if (tiles.AreAllTilesSeated())
                {
                    DropNewTileSet(this.numTilesToDrop);
                }
                else
                {
                    if (tiles.WillTilesGetDropped(GetCurrentSpeed()))
                    {
                        tiles.SeatTilesOnTopOfOtherTiles();
                        tiles.SeatTilesAtFloor();

                        if (this.timeSinceLastSpeedUp >= this.timeBeforeSpeedUp
                            && this.speedupEnabled)
                        {
                            LevelUp();
                        }

                        tiles.DropTilesOlderThanAge(GetCurrentSpeed());

                        int scoreBeforeTileDeletion = stats.score;
                        stats = tiles.MarkTilesForDeletion();
                        PointsGained(stats.score - scoreBeforeTileDeletion);
                    }

                    tiles.DeleteTilesMarkedForDeletion(this);
                }

                tiles.AgeTiles(elapsedTime);
            }
        }

        public void StartGame()
        {
            AddMessageBoxMessage("Game Start!", Color.GreenYellow);
            SetPaused(false);
            DropNewTileSet(this.numTilesToDrop);
            StartRotaterIdleAnimation();
        }

        public void StartRotaterIdleAnimation()
        {
            if (!this.rotater.IsAnimating())
            {
                this.rotater.StartAnimation("idle", 2, true);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //draw the board background
            spriteBatch.Draw(boardBackgroundSpriteObject.GetTexture(), boardBackgroundSpriteObject.destRect, Color.White);

            //draw the nuke image if the board is currently shaking due to a nuke
            if (this.isShaking)
            {
                spriteBatch.Draw(nukeSpriteObject.GetTexture(), nukeSpriteObject.destRect, Color.White);
            }

            //if scaling is turned on, figure out the scale size
            Vector2 scale = new Vector2(1, 1);
            Vector2 centerImageScale = new Vector2(1, 1);
            Tile referenceTile = this.GetTileSet().GetRefTile();

            if (this.IsScaleTiles())
            {
                try
                {
                    float scaleX = this.GetTilePixelWidth() / referenceTile.GetTexture().Width;
                    float scaleY = this.GetTilePixelHeight() / referenceTile.GetTexture().Height;
                    scale = new Vector2(scaleX, scaleY);

                    if (scaleX <= scaleY)
                    {
                        centerImageScale = new Vector2(scaleX, scaleX);
                    }
                    else
                    {
                        centerImageScale = new Vector2(scaleY, scaleY);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                }
            }

            //draw the tiles
            List<Tile> tilesToDraw = tiles.GetTilesAsList();
            foreach (Tile tile in tilesToDraw)
            {
                //figure out where on the screen to draw the tile background
                int thisTilePosX = (int)(this.position.X + (this.GetTilePixelWidth() * tile.X));
                int thisTilePosY = (int)(this.position.Y + (this.GetTilePixelHeight() * tile.Y));
                //int thisTilePosY = (int)(this.getPosition().Y + (this.getTilePixelHeight() * tile.getGridY()) + tile.getBoostY());

                Vector2 thisTilesPosition = new Vector2(thisTilePosX, thisTilePosY);

                //draw the background if necessary
                if (tile.backgroundTexture != null)
                {
                    //draw the background sprite
                    spriteBatch.Draw(tile.backgroundTexture, thisTilesPosition, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }

                //figure out where on screen to draw the tile foreground image
                Vector2 thisTilesCenterOrigin = new Vector2(tile.GetTexture().Width / 2, tile.GetTexture().Height / 2);
                Vector2 thisTilesCenterPosition = new Vector2(thisTilePosX + (this.GetTilePixelWidth() / 2), thisTilePosY + (this.GetTilePixelHeight() / 2));
                Vector2 thisTilesCenterOriginDuringIdleAnimation = new Vector2(tile.GetCurrentCelRect().Width / 2, tile.GetCurrentCelRect().Height / 2);

                //finally, draw the foreground of the tile
                if (tile.IsAnimating() && tile.GetCurrentAnimation() == "idle")
                {
                    spriteBatch.Draw(tile.GetTexture(), thisTilesCenterPosition, tile.GetCurrentCelRect(), Color.White, 0,
                                     thisTilesCenterOriginDuringIdleAnimation, centerImageScale, SpriteEffects.None, 0);
                }
                else if (tile.IsAnimating())
                {
                    spriteBatch.Draw(tile.GetTexture(), thisTilesPosition, tile.GetCurrentCelRect(), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(tile.GetTexture(), thisTilesCenterPosition, tile.GetCurrentCelRect(), Color.White, 0, thisTilesCenterOrigin, centerImageScale, SpriteEffects.None, 0);
                }
            }

            //draw the nuke white overlay image if the board is currently shaking due to a nuke
            if (this.isShaking)
            {
                Color color = new Color(255, 255, 255) * (float)MathHelper.Clamp(fadeFromWhiteAlpha, 0f, 1f);
                spriteBatch.Draw(blankWhiteSpritObject.GetTexture(), blankWhiteSpritObject.destRect, color);
            }

            //draw the rotater
            StartRotaterIdleAnimation();

            int rotaterYOffset = 12;
            int rotaterPosX = (int)(this.position.X + (this.GetTilePixelWidth() * rotater.horizontalPosition));
            int rotaterPosY = (int)(this.position.Y + this.height + rotaterYOffset);

            Vector2 rotaterPosition = new Vector2(rotaterPosX, rotaterPosY);
            Vector2 rotaterScale = new Vector2(scale.X * 2, 1.15f);

            spriteBatch.Draw(rotater.GetTexture(), rotaterPosition, rotater.GetCurrentCelRect(), Color.White, 0, Vector2.Zero, rotaterScale, SpriteEffects.None, 0);

            //draw any animations that are going
            AnimationManager.Instance.DrawAnimations(spriteBatch);
        }

        public void HandleInput(InputState input)
        {
            //game controller stuff
            GamePadState gamePadState = input.CurrentGamePadStates[this.playerIndex];
            GamePadState previousGamePadState = input.LastGamePadStates[this.playerIndex];

            if (gamePadState.ThumbSticks.Left.X < -0.3 && previousGamePadState.ThumbSticks.Left.X >= -0.3)
            {
                rotater.MoveLeft();
            }
            if (gamePadState.ThumbSticks.Left.X > 0.3 && previousGamePadState.ThumbSticks.Left.X <= 0.3)
            {
                rotater.MoveRight();
            }

            if (gamePadState.DPad.Left == ButtonState.Pressed && previousGamePadState.DPad.Left == ButtonState.Released)
            {
                rotater.MoveLeft();
            }
            if (gamePadState.DPad.Right == ButtonState.Pressed && previousGamePadState.DPad.Right == ButtonState.Released)
            {
                rotater.MoveRight();
            }

            if (gamePadState.IsButtonDown(Buttons.A) && previousGamePadState.IsButtonUp(Buttons.A))
            {
                SwapColumns();
            }
            if (gamePadState.IsButtonDown(Buttons.X) && previousGamePadState.IsButtonUp(Buttons.X))
            {
                EngageBulletTime();
            }
            if (gamePadState.IsButtonDown(Buttons.Y) && previousGamePadState.IsButtonUp(Buttons.Y))
            {
                FireTheLasers(rotater.horizontalPosition, rotater.horizontalPosition + 1);
            }
            if (gamePadState.IsButtonDown(Buttons.B) && previousGamePadState.IsButtonUp(Buttons.B))
            {
                FireNuke();
            }

            //keyboard stuff
            KeyboardState keyboardState = input.CurrentKeyboardStates[this.playerIndex];
            KeyboardState previousKeyboardState = input.LastKeyboardStates[this.playerIndex];

            if (keyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left))
            {
                rotater.MoveLeft();
            }
            if (keyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right))
            {
                rotater.MoveRight();
            }

            if (keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
            {
                SwapColumns();
            }
            if (keyboardState.IsKeyDown(Keys.X) && previousKeyboardState.IsKeyUp(Keys.X))
            {
                EngageBulletTime();
            }
            if (keyboardState.IsKeyDown(Keys.Y) && previousKeyboardState.IsKeyUp(Keys.Y))
            {
                FireTheLasers(rotater.horizontalPosition, rotater.horizontalPosition + 1);
            }
            if (keyboardState.IsKeyDown(Keys.B) && previousKeyboardState.IsKeyUp(Keys.B))
            {
                FireNuke();
            }

            //easy shortcut to quit the entire game if we're running in dev/debug mode
            if (keyboardState.IsKeyDown(Keys.LeftShift) && previousKeyboardState.IsKeyUp(Keys.Escape) && SwitchGame.DEBUG_MODE)
            {
                SwitchGame.Instance.screenManager.Game.Exit();
            }

            //down held stuff
            if (gamePadState.ThumbSticks.Left.Y >= -0.3 && gamePadState.DPad.Down != ButtonState.Pressed && !keyboardState.IsKeyDown(Keys.Down))
            {
                timeDownHeld = 0;
            }
        }

        public List<DetailedSpriteObject> GetSpritesToBlur()
        {
            List<DetailedSpriteObject> tilesToBlur = new List<DetailedSpriteObject>();

            foreach (Tile tile in tiles.GetUnseatedTiles())
            {
                tilesToBlur.Add(new DetailedSpriteObject(tile.backgroundTexture,
                                                         this.GetTileRectangle(tile)));
            }

            return tilesToBlur;
        }

        private void SwapColumns()
        {
            rotater.StartAnimation("rotate", 80);
            SoundManager.Instance.PlaySound("flip");
            tiles.SwapColumns(rotater.horizontalPosition, rotater.horizontalPosition + 1);
        }

        private void EngageBulletTime()
        {
            if (!this.bulletTimeActive && this.stats.power >= 25)
            {
                AddMessageBoxMessage("Bullet Time Engaged!", Color.Blue);
                SoundManager.Instance.PlaySound("bullettime");

                this.bulletTimeActive = true;
                this.bulletTimeLeft += 15000;
                this.maxBulletTime = this.bulletTimeLeft;
                this.stats.numberOfBulletTimesFired++;
                this.stats.score += 25;

                if (!SwitchGame.DEBUG_MODE)
                {
                    this.stats.power -= 25;
                }
            }
        }

        private void StopBulletTime()
        {
            if (this.bulletTimeActive)
            {
                AddMessageBoxMessage("Bullet Time Over!", Color.Blue);

                this.bulletTimeActive = false;
                this.bulletTimeLeft = 0;
            }
        }

        private void FireTheLasers(int leftMostColumnToBlast, int rightMostColumnToBlast)
        {
            if (this.stats.power >= 50)
            {
                for (int i = leftMostColumnToBlast; i <= rightMostColumnToBlast; i++)
                {
                    int numTilesDestroyed = tiles.ClearColumn(i); //todo - update this to give points for stuff destroyed
                    stats.numberOfBlocksDestroyed += numTilesDestroyed;
                    stats.numberOfBlocksDestroyedByLaser += numTilesDestroyed;
                }

                this.stats.numberOfLasersFired++;
                this.stats.score += 50;
                AddMessageBoxMessage("Laser Fired!", Color.Yellow);
                SoundManager.Instance.PlaySound("laser");

                Rectangle animationRect = new Rectangle((int)(position.X + (leftMostColumnToBlast * GetTilePixelWidth())),
                                                        (int)position.Y,
                                                        (int)(GetTilePixelWidth() * 2),
                                                        height);
                AnimationManager.Instance.StartAnimation("laser", 50, animationRect);
                VibrationManager.Instance.VibrateController((PlayerIndex)this.playerIndex, 300);

                if (!SwitchGame.DEBUG_MODE)
                {
                    this.stats.power -= 50;
                }
            }
        }

        private void FireNuke()
        {
            if (this.stats.power >= 100)
            {
                int numTilesDestroyed = tiles.ClearBoard(); //todo - update this to give points for stuff destroyed
                stats.numberOfBlocksDestroyed += numTilesDestroyed;
                stats.numberOfBlocksDestroyedByNuke += numTilesDestroyed;
                stats.score += 100;

                if (this.bulletTimeActive)
                {
                    stats.numberOfNukesFiredDuringActiveBulletTime++;
                }

                this.stats.numberOfNukesFired++;
                AddMessageBoxMessage("Nuke Dropped!!!", Color.Red);
                SoundManager.Instance.PlaySound("nuke-explode");

                Rectangle animationRect = new Rectangle((int)position.X,
                                                        (int)position.Y,
                                                        width,
                                                        height);

                this.ShakeBoard(1250);

                if (!SwitchGame.DEBUG_MODE)
                {
                    this.stats.power -= 100;
                }
            }
        }

        /**
         * Returns an array of tiles, randomly chosen from the TileSet. Duplicates are a possibility.
         **/
        private Tile[] GetRandomTiles(int numOfTiles)
        {
            Tile[] allTiles = tileSet.ToArray();

            List<Tile> tilePool = new List<Tile>();
            foreach (Tile tile in allTiles)
            {
                int probabilityCounter;

                if (tile.type == Tile.TileType.Normal)
                {
                    probabilityCounter = 6;
                }
                else if (tile.type == Tile.TileType.BottomCapper || tile.type == Tile.TileType.TopCapper)
                {
                    probabilityCounter = 6;
                }
                else
                {
                    probabilityCounter = 1;
                }

                for (int i = 0; i < probabilityCounter; i++)
                {
                    tilePool.Add(tile);
                }
            }

            Tile[] randomTileArray = new Tile[numOfTiles];

            for (int i = 0; i < numOfTiles; i++)
            {
                int randomTilePosition = random.Next(tilePool.Count);
                randomTileArray[i] = new Tile(tilePool[randomTilePosition]);
            }

            return randomTileArray;
        }

        private void PointsGained(int pointsGained)
        {
            if (pointsGained > 0 && pointsGained <= 100)
            {
                AddMessageBoxMessage("Good! +" + pointsGained + " Points!");
            }
            else if (pointsGained > 100 && pointsGained <= 250)
            {
                AddMessageBoxMessage("Nice One! +" + pointsGained + " Points!", Color.Gold);
            }
            else if (pointsGained > 250 && pointsGained <= 500)
            {
                AddMessageBoxMessage("Stupendous! +" + pointsGained + " Points!", Color.Gold);
            }
            else if (pointsGained > 500 && pointsGained <= 1000)
            {
                AddMessageBoxMessage("Spectacular! +" + pointsGained + " Points!", Color.Gold);
            }
            else if (pointsGained > 1000)
            {
                AddMessageBoxMessage("Incredible! +" + pointsGained + " Points!", Color.Gold);
            }
        }
    }
}