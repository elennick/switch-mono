using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.GameObjects.Challenges;
//using Switch.Screens;
//using Microsoft.Xna.Framework.GamerServices;

namespace Switch.Menus
{
    class ChallengeModeDifficultySelectionScreen : MenuScreen
    {
        MenuEntry easyEntry;
        MenuEntry mediumEntry;
        MenuEntry hardEntry;
        MenuEntry crazyEntry;
        MenuEntry impossibleEntry;
        MenuEntry backMenuEntry;

        public ChallengeModeDifficultySelectionScreen()
            : base("Challenge Difficulty")
        {
            this.setSubMenuTitleText("Challenges " + ChallengeManager.Instance.GetPercentOfChallengesCompleted() + "% Completed");

            easyEntry = new MenuEntry("Easy");
            mediumEntry = new MenuEntry("Medium");
            hardEntry = new MenuEntry("Hard");
            crazyEntry = new MenuEntry("Crazy");
            impossibleEntry = new MenuEntry("Impossible");
            backMenuEntry = new ExitOrBackMenuEntry("Back To Main Menu...");

            easyEntry.Selected += easyEntrySelected;
            mediumEntry.Selected += mediumEntrySelected;
            hardEntry.Selected += hardEntrySelected;
            crazyEntry.Selected += crazyEntrySelected;
            impossibleEntry.Selected += impossibleEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(easyEntry);
            MenuEntries.Add(mediumEntry);
            MenuEntries.Add(hardEntry);
            MenuEntries.Add(crazyEntry);
            MenuEntries.Add(impossibleEntry);
            MenuEntries.Add(backMenuEntry);
        }

        void easyEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ChallengeModeSelectionScreen(ChallengeManager.ChallengeLevel.EASY), null);
        }

        void mediumEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ChallengeModeSelectionScreen(ChallengeManager.ChallengeLevel.MEDIUM), null);
        }

        void hardEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ChallengeModeSelectionScreen(ChallengeManager.ChallengeLevel.HARD), null);
        }

        void crazyEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ChallengeModeSelectionScreen(ChallengeManager.ChallengeLevel.CRAZY), null);
        }

        void impossibleEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ChallengeModeSelectionScreen(ChallengeManager.ChallengeLevel.IMPOSSIBLE), null);
        }
    }
}
