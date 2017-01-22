using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class CapTiles20 : IChallenge
    {
        public string GetName()
        {
            return "Caught In The Middle";
        }

        public string GetDescription()
        {
            return "Destroy fifteen tiles by capping them. You must remove 15\n" +
                   "tiles from the screen but only ones destroyed by being\n" +
                   "sandwiched will count towards this total! Mmmm... sandwich.";
        }

        public Difficulty GetDifficulty()
        {
            return new Normal();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            if (stats.numberOfTilesDestroyedByCapping >= 15)
            {
                return true;
            }

            return false;
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfTilesDestroyedByCapping + " / 15\nTiles Destroyed";
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
