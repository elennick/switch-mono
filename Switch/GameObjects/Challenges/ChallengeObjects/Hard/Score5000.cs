using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class Score5000 : IChallenge
    {
        public string GetName()
        {
            return "Score Attack";
        }

        public string GetDescription()
        {
            return "Score 5000 points! I don't have any other funny things to say!";
        }

        public Difficulty GetDifficulty()
        {
            return new Hard();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            if (stats.score >= 5000)
            {
                return true;
            }

            return false;
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.score + " / 5000\nPoints Scored";
        }

        public int IsSpeedUpEnabled()
        {
            return 0;
        }

        public int StartingPower()
        {
            return 50;
        }
    }
}
