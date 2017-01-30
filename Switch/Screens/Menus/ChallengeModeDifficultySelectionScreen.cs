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
            this.SetSubMenuTitleText("Challenges " + ChallengeManager.Instance.GetPercentOfChallengesCompleted() + "% Completed");

            easyEntry = new MenuEntry("Easy");
            mediumEntry = new MenuEntry("Medium");
            hardEntry = new MenuEntry("Hard");
            crazyEntry = new MenuEntry("Crazy");
            impossibleEntry = new MenuEntry("Impossible");
            backMenuEntry = new ExitOrBackMenuEntry("Back To Main Menu...");

            easyEntry.Selected += EasyEntrySelected;
            mediumEntry.Selected += MediumEntrySelected;
            hardEntry.Selected += HardEntrySelected;
            crazyEntry.Selected += CrazyEntrySelected;
            impossibleEntry.Selected += ImpossibleEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(easyEntry);
            MenuEntries.Add(mediumEntry);
            MenuEntries.Add(hardEntry);
            MenuEntries.Add(crazyEntry);
            MenuEntries.Add(impossibleEntry);
            MenuEntries.Add(backMenuEntry);
        }

        void EasyEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ChallengeModeSelectionScreen(ChallengeManager.ChallengeLevel.Easy), null);
        }

        void MediumEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ChallengeModeSelectionScreen(ChallengeManager.ChallengeLevel.Medium), null);
        }

        void HardEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ChallengeModeSelectionScreen(ChallengeManager.ChallengeLevel.Hard), null);
        }

        void CrazyEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ChallengeModeSelectionScreen(ChallengeManager.ChallengeLevel.Crazy), null);
        }

        void ImpossibleEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ChallengeModeSelectionScreen(ChallengeManager.ChallengeLevel.Impossible), null);
        }
    }
}
