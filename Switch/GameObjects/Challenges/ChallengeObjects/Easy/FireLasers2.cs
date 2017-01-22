using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class FireLasers2 : IChallenge
    {
        public string GetName()
        {
            return "Fire Da Laser!";
        }

        public string GetDescription()
        {
            return "Fire the laser twice! Simply save up 50 energy and fire by\n" +
                   "pressing (Y). Two times the lasering, two times the fun! That\n" +
                   "joke was really lame!";
        }

        public Difficulty GetDifficulty()
        {
            return new Easy();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            if (stats.numberOfLasersFired >= 2)
            {
                return true;
            }

            return false;
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfLasersFired + " / 2\nLasers Fired";
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
