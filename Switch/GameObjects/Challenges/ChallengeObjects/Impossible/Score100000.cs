using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class Score100000 : IChallenge
    {
        public string GetName()
        {
            return "Pff, Yea Right";
        }

        public string GetDescription()
        {
            return "Score 100,000 points! Use those cappers and multipliers well!\n" +
                   "Be realistic though, you'll never pull it off... you just aren't\n" +
                   "good enough...";
        }

        public Difficulty GetDifficulty()
        {
            return new Impossible();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            if (stats.score >= 100000)
            {
                return true;
            }

            return false;
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.score + " / 100000\nPoints Scored";
        }

        public int IsSpeedUpEnabled()
        {
            return 0;
        }

        public int StartingPower()
        {
            return 100;
        }
    }
}
