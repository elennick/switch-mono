using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class CompleteCap25 : IChallenge
    {
        public string GetName()
        {
            return "Cap It";
        }

        public string GetDescription()
        {
            return "Complete twenty-five caps. All you have to do is drop a bottom\n" +
                   "cap anywhere on the board and then place a top cap anywhere\n" +
                   "above it. It can be directly above, or five tiles above. Either\n" +
                   "way works!";
        }

        public Difficulty GetDifficulty()
        {
            return new Hard();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            return stats.numberOfCapsCompleted >= 25;
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfCapsCompleted + " / 25\nCaps Completed";
        }

        public int IsSpeedUpEnabled()
        {
            return 30000;
        }

        public int StartingPower()
        {
            return 25;
        }
    }
}
