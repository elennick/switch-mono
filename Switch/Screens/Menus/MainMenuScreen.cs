using Microsoft.Xna.Framework;
using Switch.GameObjects.Sound;
using Switch.Menus;
//using Microsoft.Xna.Framework.GamerServices;
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
//using Switch.Screens;

namespace Switch.Menus
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
            : base("awesome switch logo goes here !")
        {
            this.SetShowBackgroundDecoration(false);

            // Create our menu entries.
            MenuEntry scoreAttack = new MenuEntry("1-Player Score Attack");
            MenuEntry challengeMode = new MenuEntry("1-Player Challenges");
            MenuEntry battleMode = new MenuEntry("2-Player Battle");
            MenuEntry highScores = new MenuEntry("High Scores");
            MenuEntry options = new MenuEntry("Help & Options");
            MenuEntry exit = new ExitOrBackMenuEntry("Exit");

            // Hook up menu event handlers.
            scoreAttack.Selected += ScoreAttackSelected;
            challengeMode.Selected += ChallengeModeSelected;
            battleMode.Selected += BattleModeSelected;
            highScores.Selected += HighScoresSelected;
            options.Selected += OptionsSelected;
            exit.Selected += ExitSelected;

            // Add entries to the menu.
            MenuEntries.Add(scoreAttack);
            MenuEntries.Add(challengeMode);
            MenuEntries.Add(battleMode);
            MenuEntries.Add(highScores);
            MenuEntries.Add(options);
#if XBOX
            if (Guide.IsTrialMode)
            {
                gameIsInTrialMode = Guide.IsTrialMode;
                MenuEntries.Add(purchase);
            }
#endif
            MenuEntries.Add(exit);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            this.SetMenuTitleImage(content.Load<Texture2D>("Sprites\\Title\\switch_logo"));
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        void ScoreAttackSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ScoreAttackSelectionScreen(), null);
        }

        void BattleModeSelected(object sender, PlayerIndexEventArgs e)
        {
            //ScreenManager.AddScreen(new BattleModeControllerSelectScreen(), null);
        }

        void ChallengeModeSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ChallengeModeDifficultySelectionScreen(), null);
        }

        void HighScoresSelected(object sender, PlayerIndexEventArgs e)
        {
            //ScreenManager.AddScreen(new HighScoreScreen(), null);
        }

        void OptionsSelected(object sender, PlayerIndexEventArgs e)
        {
           ScreenManager.AddScreen(new OptionsMenuScreen(), null);
        }

        void ExitSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            //override the default method so that nothing happens when B is pressed
            //from the top level main menu here
        }
    }
}
