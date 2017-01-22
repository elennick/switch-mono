using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class Score15000 : IChallenge
    {
        public string GetName()
        {
            return "Score!";
        }

        public string GetDescription()
        {
            return "Score 15000 points! Show the crowd what you're made of!";
        }

        public Difficulty GetDifficulty()
        {
            return new Hard();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            if (stats.score >= 15000)
            {
                return true;
            }

            return false;
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.score + " / 15000\nPoints Scored";
        }

        public int IsSpeedUpEnabled()
        {
            return 60000;
        }

        public int StartingPower()
        {
            return 25;
        }
    }
}
