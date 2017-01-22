using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Menus;
using Switch.Utils.Difficulty.DifficultyObjects;
using Switch.Utils.Difficulty;

namespace Switch.GameObjects.Challenges.ChallengeObjects
{
    class FireNukes3 : IChallenge
    {
        public string GetName()
        {
            return "Trifecta Of Death";
        }

        public string GetDescription()
        {
            return "Fire three nuclear strikes. Save up 100 energy and let that\n" +
                   "sucker rip! Do it 3 times and you will pass this challenge\n" +
                   "of epic destruction! ";
        }

        public Difficulty GetDifficulty()
        {
            return new Hard();
        }

        public bool IsCompleted(GameboardStats stats)
        {
            if (stats.numberOfNukesFired >= 3)
            {
                return true;
            }

            return false;
        }

        public string GetStatusText(GameboardStats stats)
        {
            return "" + stats.numberOfNukesFired + " / 3\nNukes Fired";
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
