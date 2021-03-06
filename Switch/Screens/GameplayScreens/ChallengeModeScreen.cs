﻿using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Switch.GameObjects;
using Switch.GameObjects.Tiles;
using System.Collections.Generic;
using Switch.GameObjects.Challenges;
using Switch.GameObjects.Challenges.ChallengeObjects;
using Switch.GameObjects.Sound;
using Switch.GameObjects.GameDisplays;
using Switch.Menus;
using Switch;

namespace Switch
{
    class ChallengeModeScreen : GameplayScreen
    {
        Random random = new Random();

        public ChallengeModeScreen(IChallenge challenge, PlayerIndex playerIndex) : base(1)
        {
            this.challenge = challenge;
            base.SetPlayerOne(playerIndex);

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override GameMode GetGameMode()
        {
            return GameplayScreen.GameMode.CHALLENGE_MODE;
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            base.LoadContent();

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();

            //define and load fonts
            SpriteFont weaponDisplayFont = content.Load<SpriteFont>("Fonts\\WeaponsDisplayFont");
            SpriteFont messageBoxDisplayFont = content.Load<SpriteFont>("Fonts\\ScoreMessageBoxFont");
            SpriteFont scoreFont = content.Load<SpriteFont>("Fonts\\PhillySansFont");

            //create a new gameboard to use for 1p
            GameBoard gameBoard = new GameBoard(new Vector2(480, 55), TileSet.LoadAndGetDefaultTileset(content, challenge.GetDifficulty()),challenge.GetDifficulty(), 400, 564, (int)playerIndex1);
            gameBoard.SetScaleTiles(true);
            gameBoard.GetStats().power = this.challenge.StartingPower();
            this.addGameplayScreenObject(gameBoard);

            //check to see if this challenge wants the board to speed up or stay constant speed
            int speedUpTime = challenge.IsSpeedUpEnabled();
            if (speedUpTime > 0)
            {
                gameBoard.SetSpeedUpEnabled(true);
                gameBoard.SetSpeedUpTimer(speedUpTime);
            }
            else
            {
                gameBoard.SetSpeedUpEnabled(false);
            }
            

            //load all the background imagery
            DetailedSpriteObject infoPanels = new DetailedSpriteObject(content.Load<Texture2D>("Sprites\\BoardComponents\\info_panels_challenges"),
                                                                            new Rectangle(90, 0, 350, 675));

            this.addGameplayScreenObject(infoPanels);

            //define and load the displays
            LevelDisplay levelDisplay = new LevelDisplay(new Vector2(385, 75), scoreFont, gameBoard);
            ChallengeStatusDisplay challengeStatusDisplay = new ChallengeStatusDisplay(new Vector2(115, 180), scoreFont, gameBoard, this.challenge);
            ScoreDisplay scoreDisplay = new ScoreDisplay(new Vector2(200, 75), scoreFont, gameBoard);
            ComplexPowerMeterDisplay powerDisplay = new ComplexPowerMeterDisplay(new Vector2(920, 0), content, messageBoxDisplayFont, gameBoard, false);
            GameMessageBoxDisplay messageBoxDisplay = new GameMessageBoxDisplay(250,
                                                                                300,
                                                                                new Vector2(105, 338),
                                                                                messageBoxDisplayFont,
                                                                                gameBoard,
                                                                                12);

            this.addGameplayScreenObject(levelDisplay);
            this.addGameplayScreenObject(challengeStatusDisplay);
            this.addGameplayScreenObject(scoreDisplay);
            this.addGameplayScreenObject(powerDisplay);
            this.addGameplayScreenObject(messageBoxDisplay);
        }
    }
}
