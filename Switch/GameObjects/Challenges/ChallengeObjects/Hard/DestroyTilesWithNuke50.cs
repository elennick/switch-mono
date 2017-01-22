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
    class DestroyTilesWithNuke50 : IChallenge
    {
        public string GetName()
        {
            return "Duke Nuked";
        }

        public string GetDescription()
        {
            return "Destroy 50 tiles using only the nuclear strike! You must remove\n" +
                   "50 tiles from the screen to pass this challenge but only tiles\n" +
                   "destroyed using the nuke will count. Hail to the king, baby!\n";
        }

        public Difficulty GetDifficulty()
        {
            return new Hard();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            if (stats.numberOfBlocksDestroyedByNuke >= 50)
            {
                return true;
            }

            return false;
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfBlocksDestroyedByNuke + " / 50\nTiles Destroyed";
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

