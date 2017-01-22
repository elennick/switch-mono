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
    class DestroyTiles500 : IChallenge
    {
        public string GetName()
        {
            return "Destroy Tiles";
        }

        public string GetDescription()
        {
            return "Destroy 500 tiles. You may use any method to do so including\n" + 
                   "matching, capping, lasering and nuking.";
        }

        public Difficulty GetDifficulty()
        {
            return new Hard();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            if (stats.numberOfBlocksDestroyed >= 500)
            {
                return true;
            }

            return false;
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfBlocksDestroyed + " / 500\nTiles Destroyed";
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
