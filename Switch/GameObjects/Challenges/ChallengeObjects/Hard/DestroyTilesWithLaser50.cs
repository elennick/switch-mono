using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.GameObjects.Challenges;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class DestroyTilesWithLaser50 : IChallenge
    {
        public string GetName()
        {
            return "Gone In A Flash";
        }

        public string GetDescription()
        {
            return "Destroy 50 tiles... using the laser! You must demolish a total\n" +
                   "of fifty tiles but only ones removed by using the laser weapon\n" +
                   "will count towards the total! Demonstrate your mastery of this\n" +
                   "precision tool by pressing (Y)!\n";
        }

        public Difficulty GetDifficulty()
        {
            return new Hard();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            if (stats.numberOfBlocksDestroyedByLaser >= 50)
            {
                return true;
            }

            return false;
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfBlocksDestroyedByLaser + " / 50\nTiles Destroyed";
        }

        public int IsSpeedUpEnabled()
        {
            return 0;
        }

        public int StartingPower()
        {
            return 25;
        }
    }
}

