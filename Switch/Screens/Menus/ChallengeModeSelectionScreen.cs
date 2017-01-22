using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Switch.GameObjects.Challenges;
using Switch.GameObjects.Challenges.ChallengeObjects;
using Switch.Menus;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Switch.Menus
{
    class ChallengeModeSelectionScreen : MenuScreen
    {
        MenuEntry backMenuEntry;
        IChallenge currentlySelectedChallenge;
        private Texture2D uncheckedCheckbox;
        private Texture2D checkedCheckbox;

        public ChallengeModeSelectionScreen(Switch.GameObjects.Challenges.ChallengeManager.ChallengeLevel level)
            : base("Select A Challenge!")
        {
            this.setSubMenuTitleText("Challenges " + ChallengeManager.Instance.GetPercentOfChallengesCompleted() + "% Completed");

            List<IChallenge> challenges = ChallengeManager.Instance.GetChallenges(level);
            foreach (IChallenge challenge in challenges)
            {
                ChallengeModeMenuEntry menuEntry = new ChallengeModeMenuEntry(challenge.GetName(), 
                                                                              challenge.GetDescription(), 
                                                                              ChallengeManager.Instance.GetChallengeStatus(challenge.GetName()));
                menuEntry.Selected += challengeEntrySelected;
                MenuEntries.Add(menuEntry);
            }

            backMenuEntry = new ExitOrBackMenuEntry("Go Back...");
            backMenuEntry.Selected += OnCancel;
            MenuEntries.Add(backMenuEntry);
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            uncheckedCheckbox = content.Load<Texture2D>("Sprites\\checkbox");
            checkedCheckbox = content.Load<Texture2D>("Sprites\\checkbox-checked");

            foreach (MenuEntry menuEntry in MenuEntries)
            {
                //set checkbox image based on completion status
                try
                {
                    ChallengeModeMenuEntry cMenuEntry = (ChallengeModeMenuEntry)menuEntry;
                    if (cMenuEntry.isChallengeCompleted())
                    {
                        cMenuEntry.setImage(checkedCheckbox);
                    }
                    else
                    {
                        cMenuEntry.setImage(uncheckedCheckbox);
                    }
                }
                catch (InvalidCastException ice)
                {
                    //swallow this, it happens when the backMenuEntry is iterated over
                    continue;
                }
            }
        }

        void challengeEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            string challengeName = ((MenuEntry)sender).Text;
            currentlySelectedChallenge = ChallengeManager.Instance.GetChallengeByName(challengeName);
            string message = "Challenge - " + currentlySelectedChallenge.GetName() + "\n\n" + currentlySelectedChallenge.GetDescription();

            ChallengeModeMessageBoxScreen confirmExitMessageBox = new ChallengeModeMessageBoxScreen(currentlySelectedChallenge.GetName(), 
                                                                          currentlySelectedChallenge.GetDescription(), false);
            confirmExitMessageBox.Accepted += ConfirmChallengeSelected;
            ScreenManager.AddScreen(confirmExitMessageBox, ControllingPlayer);
        }

        void ConfirmChallengeSelected(object sender, PlayerIndexEventArgs e)
        {
            GameScreen[] screensToLoad = new GameScreen[2];
            screensToLoad[0] = new BackgroundScreen(true, true);
            screensToLoad[1] = new ChallengeModeScreen(currentlySelectedChallenge, e.PlayerIndex);

            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, screensToLoad);
        }
    }
}
