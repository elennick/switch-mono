using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class FireNukes1 : IChallenge
    {
        public string GetName()
        {
            return "Radioactive";
        }

        public string GetDescription()
        {
            return "Fire one nuke. It's just that easy. Save up 100 energy, press (B)\n" +
                   "and watch the mushroom cloud rise. Every Joe Shmoe and his\n" +
                   "dog gets access to nuclear weapons these days, eh?";
        }

        public Difficulty GetDifficulty()
        {
            return new Easy();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            if (stats.numberOfNukesFired >= 1)
            {
                return true;
            }

            return false;
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfNukesFired + " / 1\nNukes Fired";
        }

        public int IsSpeedUpEnabled()
        {
            return 0;
        }

        public int StartingPower()
        {
            return 0;
        }
    }
}
