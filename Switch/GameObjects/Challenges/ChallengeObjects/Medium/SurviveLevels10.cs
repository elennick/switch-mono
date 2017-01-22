using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class SurviveLevels10 : IChallenge
    {
        public string GetName()
        {
            return "Still Alive";
        }

        public string GetDescription()
        {
            return "Survive for 10 levels worth of time without losing the game. Use\n" +
                   "all the tools at your disposal to survive until the time limit is\n" +
                   "up! IT'S SO VERY INTENSE AHHHHHH!!!";
        }

        public Difficulty GetDifficulty()
        {
            return new Normal();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            return (stats.level >= 10);
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.level + " / 10\nLevels Survived";
        }

        public int IsSpeedUpEnabled()
        {
            return 10000;
        }

        public int StartingPower()
        {
            return 25;
        }
    }
}
