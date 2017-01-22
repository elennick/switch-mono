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
    class DestroyTiles25 : IChallenge
    {
        public string GetName()
        {
            return "Obliteration";
        }

        public string GetDescription()
        {
            return "Destroy 25 tiles. You can do this in any manner. Nuke, match,\n" +
                   "laser or cap them. You shouldn't have any problem obliterating\n" +
                   "every last one of those poor defenseless tiles. You monster.";
        }

        public Difficulty GetDifficulty()
        {
            return new Easy();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            if (stats.numberOfBlocksDestroyed >= 25)
            {
                return true;
            }

            return false;
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfBlocksDestroyed + " / 25\nTiles Destroyed";
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
