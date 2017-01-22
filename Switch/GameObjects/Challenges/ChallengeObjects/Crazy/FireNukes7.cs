using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class FireNukes7 : IChallenge
    {
        public string GetName()
        {
            return "Nuclear Winter";
        }

        public string GetDescription()
        {
            return "Fire seven nuclear strikes. Save up 100 energy and let that\n" +
                   "sucker rip! Do it 7 times and you will pass this challenge!\n";
        }

        public Difficulty GetDifficulty()
        {
            return new Hard();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            if (stats.numberOfNukesFired >= 7)
            {
                return true;
            }

            return false;
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfNukesFired + " / 7\nNukes Fired";
        }

        public int IsSpeedUpEnabled()
        {
            return 30000;
        }

        public int StartingPower()
        {
            return 25;
        }
    }
}
