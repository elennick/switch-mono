using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.GameObjects;
using Switch.GameObjects.GameDisplays;
using Switch.GameObjects.Sound;
using Microsoft.Xna.Framework.Content;
using Switch.Menus;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Switch.GameObjects.Challenges;
using Switch.HighScores;
using Switch.Utils.Difficulty;
//using Microsoft.Xna.Framework.GamerServices;

namespace Switch
{
    abstract class GameplayScreen : GameScreen
    {
        protected ContentManager content;
        private List<GameBoard> gameBoards;
        private List<GameDisplay> gameDisplays;
        private List<DetailedSpriteObject> spriteObjects;
        private int numPlayers;
        protected IChallenge challenge;
        public CountDownState currentCountDownState;
        private int timeSinceLastCountDownChange;
        public enum CountDownState { NOT_STARTED, PAUSE, READY, SET, GO, DONE };
        public enum GameMode { SCORE_ATTACK, CHALLENGE_MODE, BATTLE_MODE };
        private bool gameStarted;
        protected PlayerIndex? playerIndex1, playerIndex2;
        bool trackingFPS = false;
        int lastKnownFPS = 0;
        int numberOfFramesSinceLastUpdate = 0;
        int timeSinceLastUpdate = 0;
        private Texture2D readyImage, setImage, goImage;

        public GameplayScreen(int numPlayers)
        {
            gameBoards = new List<GameBoard>();
            gameDisplays = new List<GameDisplay>();
            spriteObjects = new List<DetailedSpriteObject>();
            this.numPlayers = numPlayers;
            this.timeSinceLastCountDownChange = 0;
            this.gameStarted = false;

            playerIndex1 = null;
            playerIndex2 = null;

            SoundManager.Instance.SetMusicPaused(true);
        }

        abstract public GameMode GetGameMode();

        public override void LoadContent()
        {
            readyImage = content.Load<Texture2D>("Sprites\\BoardComponents\\ReadySetGo\\ready");
            setImage = content.Load<Texture2D>("Sprites\\BoardComponents\\ReadySetGo\\set");
            goImage = content.Load<Texture2D>("Sprites\\BoardComponents\\ReadySetGo\\go");

            base.LoadContent();
        }

        public void addGameplayScreenObject(Object gameplayScreenObject)
        {
            if (gameplayScreenObject is GameBoard)
            {
                gameBoards.Add((GameBoard)gameplayScreenObject);
            }
            else if (gameplayScreenObject is GameDisplay)
            {
                gameDisplays.Add((GameDisplay)gameplayScreenObject);
            }
            else if (gameplayScreenObject is DetailedSpriteObject)
            {
                spriteObjects.Add((DetailedSpriteObject)gameplayScreenObject);
            }
            else
            {
                throw new ArgumentException("Object being passed in as a gameplay " + 
                    "screen object is not a GameBoard, GameDisplay or DetailedSpriteObject");
            }
        }

        public List<GameBoard> GetGameBoards()
        {
            return gameBoards;
        }

        public List<GameDisplay> GetGameDisplays()
        {
            return gameDisplays;
        }

        public List<DetailedSpriteObject> GetSpriteObjects()
        {
            return spriteObjects;
        }

        public void StartCountdown()
        {
            currentCountDownState = CountDownState.PAUSE;
            timeSinceLastCountDownChange = 0;
        }

        public void SetPlayerOne(PlayerIndex playerIndex)
        {
            playerIndex1 = playerIndex;
        }

        public void SetPlayerTwo(PlayerIndex playerIndex)
        {
            playerIndex2 = playerIndex;
        }

        public void StartGameplay()
        {
            SoundManager.Instance.SetMusicPaused(false);
            SoundManager.Instance.PlaySong("gameplay-song");

            foreach (GameBoard gameBoard in gameBoards)
            {
                gameBoard.StartGame();
            }

            this.gameStarted = true;
        }

        public override void UnloadContent()
        {
            SoundManager.Instance.StopSong();
            AnimationManager.Instance.ClearAllAnimations();
            VibrationManager.Instance.CancelAllVibrations();

            try
            {
                content.Unload();
            }
            catch (NullReferenceException nre)
            {
                //this gets called more than once sometimes for some reason and im a bad and lazy
                //programmer so this is how i am going to deal with it
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (trackingFPS)
            {
                timeSinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastUpdate >= 1000)
                {
                    lastKnownFPS = numberOfFramesSinceLastUpdate;
                    timeSinceLastUpdate = 0;
                    numberOfFramesSinceLastUpdate = 0;
                    System.Diagnostics.Debug.WriteLine("FPS = " + lastKnownFPS);
                }
            }

            if (currentCountDownState == CountDownState.NOT_STARTED)
            {
                this.StartCountdown();
                return;
            }

            if (currentCountDownState != CountDownState.DONE)
            {
                timeSinceLastCountDownChange += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastCountDownChange >= 1000)
                {
                    timeSinceLastCountDownChange = 0;
                    currentCountDownState++;
                    
                    if(currentCountDownState == CountDownState.READY || currentCountDownState == CountDownState.SET) {
                        SoundManager.Instance.PlaySound("readySet");
                    }
                    else if (currentCountDownState == CountDownState.GO)
                    {
                        SoundManager.Instance.PlaySound("go");
                    }
                }
            }

            if (IsActive)
            {
                foreach (GameBoard gameBoard in gameBoards)
                {
                    if (gameBoard.IsGameOver()
                        && !AnimationManager.Instance.AreAnyAnimationsActive()
                        && !GameboardIsFiringNuke())
                    {
                        VibrationManager.Instance.CancelAllVibrations();

                        if (this.GetGameMode() == GameMode.SCORE_ATTACK)
                        {
                            int score = gameBoard.GetScore();
                            Difficulty diff = gameBoard.GetDifficulty();

                            String name;
                            //try
                            //{
                                //name = SignedInGamer.SignedInGamers[(int)playerIndex1].Gamertag;
                            //}
                           // catch (Exception e)
                           // {
                                name = "Guest";
                            //}

                            HighScore highScore = new HighScore(name, score, diff);
                            HighScoreManager.Instance.AddHighScore(highScore);
                        }

                        if (this.GetGameMode() != GameMode.BATTLE_MODE)
                        {
                            SaveStats();
                            //** RE-ENABLE ME
                            //GameOverBackToMenuScreen confirmGameOverMessageBox = new GameOverBackToMenuScreen(this.getGameMode());
                            //ScreenManager.AddScreen(confirmGameOverMessageBox, null);
                        }
                        else
                        {
                            //TODO - THIS WHOLE CHUNK OF CODE IS BROKEN FIX IT
                            String playerIndex;
                            if (gameBoard.GetPlayerIndex() == 0)
                            {
                                //player 1 lost, so the winner is player 2
                                playerIndex = "2";
                            }
                            else
                            {
                                //player 2 lost, so the winner is player 1
                                playerIndex = "1";
                            }
                            //TODO - THE WHOLE CHUNK OF CODE ABOVE THIS IS BROKEN - FIX IT

                            SaveStats();
                            //** RE-ENABLE ME
                            //BattleModeGameOverScreen battleModeGameOverScreen = new BattleModeGameOverScreen("Player " + playerIndex + " Wins!", (int)playerIndex1, (int)playerIndex2);
                            //ScreenManager.AddScreen(battleModeGameOverScreen, null);
                        }
                    }

                    if (this.challenge != null 
                        && this.challenge.IsCompleted(gameBoard.GetStats())
                        && !AnimationManager.Instance.AreAnyAnimationsActive()
                        && !GameboardIsFiringNuke()
                        && !gameBoard.IsGameOver())
                    {
                        VibrationManager.Instance.CancelAllVibrations();
                        SoundManager.Instance.PlaySound("player-select");

                        ChallengeManager challengeManager = ChallengeManager.Instance;
                        if (!challengeManager.GetChallengeStatus(this.challenge.GetName()))
                        {
                            challengeManager.SetChallengeCompleteStatus(this.challenge.GetName(), true);
                            StorageManager.Instance.SaveChallengeStatuses(challengeManager.GetChallengeSaveData());
                            SaveStats();
                        }

                        ChallengeCompleteScreen challengeCompleteScreen = new ChallengeCompleteScreen("Challenge Completed!");
                        ScreenManager.AddScreen(challengeCompleteScreen, null);
                    }

                    gameBoard.Update(gameTime);
                }

                foreach (GameDisplay gameDisplay in gameDisplays)
                {
                    gameDisplay.Update(gameTime, false, false);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (trackingFPS)
            {
                numberOfFramesSinceLastUpdate++;
            }

            /*ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.White, 0, 0);*/

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            //draw background images
            foreach (DetailedSpriteObject sprite in spriteObjects)
            {
                spriteBatch.Draw(sprite.GetTexture(), sprite.destRect, Color.White);
            }

            foreach (GameBoard gameBoard in gameBoards)
            {
                //draw the gameboard (tiles, rotater, etc)
                gameBoard.Draw(spriteBatch, gameTime);

                //draw game displays (score, power meters, etc)
                foreach (GameDisplay display in gameDisplays)
                {
                    display.Draw(spriteBatch, gameTime);
                }
            }

            if (!gameStarted)
            {
                Vector2 readySetGoImagePosition = new Vector2(640, 360);
                if (currentCountDownState == CountDownState.READY)
                {
                    Vector2 readyImageOrigin = new Vector2(readyImage.Width / 2, readyImage.Height / 2);
                    spriteBatch.Draw(readyImage, readySetGoImagePosition, null, Color.White, 0, readyImageOrigin, Vector2.One, SpriteEffects.None, 0);
                }
                else if (currentCountDownState == CountDownState.SET)
                {
                    Vector2 setImageOrigin = new Vector2(setImage.Width / 2, setImage.Height / 2);
                    spriteBatch.Draw(setImage, readySetGoImagePosition, null, Color.White, 0, setImageOrigin, Vector2.One, SpriteEffects.None, 0);
                }
                else if (currentCountDownState == CountDownState.GO)
                {
                    Vector2 goImageOrigin = new Vector2(goImage.Width / 2, goImage.Height / 2);
                    spriteBatch.Draw(goImage, readySetGoImagePosition, null, Color.White, 0, goImageOrigin, Vector2.One, SpriteEffects.None, 0);
                }
                else if(currentCountDownState == CountDownState.DONE)
                {
                    this.StartGameplay();
                }
            }

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
            {
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
            }
        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            GamePadState gamePadState1 = input.CurrentGamePadStates[(int)playerIndex1];
            bool gamePadDisconnected1 = (!gamePadState1.IsConnected &&
                                         input.GamePadWasConnected[(int)playerIndex1]);

            GamePadState gamePadState2;
            bool gamePadDisconnected2 = false;
            if (playerIndex2 != null)
            {
                gamePadState2 = input.CurrentGamePadStates[(int)playerIndex2];
                gamePadDisconnected2 = (!gamePadState2.IsConnected &&
                                             input.GamePadWasConnected[(int)playerIndex2]);
            }

            if (input.IsPauseGame(playerIndex1) || gamePadDisconnected1)
            {
                PauseGame((PlayerIndex)playerIndex1);
            }
            else if (playerIndex2 != null && (input.IsPauseGame(playerIndex2) || gamePadDisconnected2))
            {
                PauseGame((PlayerIndex)playerIndex2);
            }
            else if(this.currentCountDownState == CountDownState.DONE)
            {
                foreach (GameBoard gameBoard in gameBoards)
                {
                    gameBoard.HandleInput(input);
                }
            }

        }

        private bool GameboardIsFiringNuke()
        {
            foreach (GameBoard gameBoard in gameBoards)
            {
                if (gameBoard.IsNukeAnimationOn())
                {
                    return true;
                }
            }

            return false;
        }

        private void SaveStats()
        {
            if (gameBoards.Count > 1)
            {
                GameboardStats consolidatedStats = new GameboardStats();
                foreach (GameBoard gameBoardToSaveStatsFor in gameBoards)
                {
                    consolidatedStats.AddStats(gameBoardToSaveStatsFor.GetStats());
                }

                StorageManager.Instance.AddStatsData(consolidatedStats);
            }
            else
            {
                StorageManager.Instance.AddStatsData(gameBoards[0].GetStats());
            }
        }

        private void PauseGame(PlayerIndex playerIndexThatPaused)
        {
            if (this.currentCountDownState == CountDownState.DONE)
            {
                VibrationManager.Instance.CancelAllVibrations();
                if (this.GetGameMode() == GameMode.CHALLENGE_MODE)
                {
                    ScreenManager.AddScreen(new PauseMenuScreen(this.challenge), playerIndexThatPaused);
                }
                else
                {
                    ScreenManager.AddScreen(new PauseMenuScreen(null), playerIndexThatPaused);
                }
            }
        }
    }
}
