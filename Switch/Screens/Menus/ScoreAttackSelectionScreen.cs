using Microsoft.Xna.Framework;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
//using Microsoft.Xna.Framework.GamerServices;
//using Switch.Screens;

namespace Switch.Menus
{
    class ScoreAttackSelectionScreen : MenuScreen
    {
        MenuEntry easyEntry;
        MenuEntry normalEntry;
        MenuEntry hardEntry;
        MenuEntry impossibleEntry;
        MenuEntry backMenuEntry;

        public ScoreAttackSelectionScreen()
            : base("Score Attack Difficulty")
        {
            this.SetSubMenuTitleText("Go For The High Score!");

            easyEntry = new MenuEntry("Easy");
            normalEntry = new MenuEntry("Normal");
            hardEntry = new MenuEntry("Hard");
            impossibleEntry = new MenuEntry("Impossible");
            backMenuEntry = new ExitOrBackMenuEntry("Back To Main Menu...");

            easyEntry.Selected += EasyEntrySelected;
            normalEntry.Selected += NormalEntrySelected;
            hardEntry.Selected += HardEntrySelected;
            impossibleEntry.Selected += ImpossibleEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(easyEntry);
            MenuEntries.Add(normalEntry);
            MenuEntries.Add(hardEntry);
            MenuEntries.Add(impossibleEntry);
            MenuEntries.Add(backMenuEntry);
        }

        void EasyEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Easy easyDiff = new Easy();

            GameScreen[] screensToLoad = new GameScreen[2];
            screensToLoad[0] = new BackgroundScreen(true, true);
            screensToLoad[1] = new ScoreAttackScreen(easyDiff, e.PlayerIndex);

            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, screensToLoad);
        }

        void NormalEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Normal normalDiff = new Normal();

            GameScreen[] screensToLoad = new GameScreen[2];
            screensToLoad[0] = new BackgroundScreen(true, true);
            screensToLoad[1] = new ScoreAttackScreen(normalDiff, e.PlayerIndex);

            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, screensToLoad);
        }

        void HardEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Hard hardDiff = new Hard();

            GameScreen[] screensToLoad = new GameScreen[2];
            screensToLoad[0] = new BackgroundScreen(true, true);
            screensToLoad[1] = new ScoreAttackScreen(hardDiff, e.PlayerIndex);

            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, screensToLoad);
        }

        void ImpossibleEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Impossible impDiff = new Impossible();

            GameScreen[] screensToLoad = new GameScreen[2];
            screensToLoad[0] = new BackgroundScreen(true, true);
            screensToLoad[1] = new ScoreAttackScreen(impDiff, e.PlayerIndex);

            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, screensToLoad);
        }
    }
}
