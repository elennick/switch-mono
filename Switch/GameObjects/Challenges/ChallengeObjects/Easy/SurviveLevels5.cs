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
    class SurviveLevels5 : IChallenge
    {
        public string GetName()
        {
            return "Surival Of The Fittest";
        }

        public string GetDescription()
        {
            return "Survive for 5 levels! As the clock counts down, your level goes\n" +
                   "up and the tiles start coming down faster! Can you handle the\n" +
                   "pressure that comes with colorful, smiley blocks moving down\n" +
                   "the screen slightly faster than normal? We'll soon find out...";
        }

        public Difficulty GetDifficulty()
        {
            return new Easy();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            return (stats.level >= 5);
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.level + " / 5\nLevels Survived";
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
