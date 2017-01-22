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
    class DestroyTiles100 : IChallenge
    {
        public string GetName()
        {
            return "Annihilation";
        }

        public string GetDescription()
        {
            return "Destroy 100 tiles. Explode them. Melt them. Eat them. Destroy\n" +
                   "them by simply loving them too much. Just get it done.\n";
        }

        public Difficulty GetDifficulty()
        {
            return new Normal();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            if (stats.numberOfBlocksDestroyed >= 100)
            {
                return true;
            }

            return false;
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfBlocksDestroyed + " / 100\nTiles Destroyed";
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
