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
    class SurviveLevels10Hard : IChallenge
    {
        public string GetName()
        {
            return "I Will Survive";
        }

        public string GetDescription()
        {
            return "Survive for ten levels of time without losing the game. It\n" +
                   "doesn't matter how you stay alive, but you must. Show the\n" +
                   "crowd how you can hold up during a marathon of puzzling!";
        }

        public Difficulty GetDifficulty()
        {
            return new Hard();
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
            return 15000;
        }

        public int StartingPower()
        {
            return 50;
        }
    }
}
