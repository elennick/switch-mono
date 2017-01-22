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
    class SurviveMinutes10 : IChallenge
    {
        public string GetName()
        {
            return "Time Is Of The Essence";
        }

        public string GetDescription()
        {
            return "Survive for ten minutes without losing the game. An ultimate\n" +
                   "test of endurance awaits...";
        }

        public Difficulty GetDifficulty()
        {
            return new Impossible();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            return (stats.timeElapsed >= (60000 * 10));
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + (stats.timeElapsed / (1000 * 60))+ " / 10\nMinutes Survived";
        }

        public int IsSpeedUpEnabled()
        {
            return 60000;
        }

        public int StartingPower()
        {
            return 100;
        }
    }
}
