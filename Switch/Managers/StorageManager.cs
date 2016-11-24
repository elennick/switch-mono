using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.HighScores;
using Switch.GameObjects.Challenges;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;
using Switch.GameObjects;

namespace Switch
{
    class StorageManager
    {
        public static string SWITCH_CONTAINER_NAME = "Switch";
        public static string HIGH_SCORES_FILE_NAME = "Switch High Scores";
        public static string CHALLENGE_STATUSES_FILE_NAME = "Switch Challenges";
        public static string STATS_FILE_NAME = "Switch Game Stats";
        private static StorageManager instance;
        private bool saveDeviceWasSelected;
        private string playerAccountName;
        private string challengeSaveFileName;
        private GameboardStats gameStats;

        public static StorageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StorageManager();
                }
                return instance;
            }
        }

        private StorageManager()
        {
            gameStats = new GameboardStats();
            saveDeviceWasSelected = false;
            playerAccountName = "Guest";
            SetChallengeSaveFileName();
        }

        private void SetChallengeSaveFileName()
        {
            challengeSaveFileName = CHALLENGE_STATUSES_FILE_NAME + "" + playerAccountName; 
        }

        public GameboardStats GetGameStats()
        {
            return gameStats;
        }

        public void AddStatsData(GameboardStats stats)
        {
            gameStats.score += stats.score;
            gameStats.numberOfBlocksDestroyed += stats.numberOfBlocksDestroyed;
            gameStats.numberOfBlocksDestroyedByLaser += stats.numberOfBlocksDestroyedByLaser;
            gameStats.numberOfBlocksDestroyedByNuke += stats.numberOfBlocksDestroyedByNuke;
            gameStats.numberOfBulletTimesFired += stats.numberOfBulletTimesFired;
            gameStats.numberOfLasersFired += stats.numberOfLasersFired;
            gameStats.numberOfNukesFired += stats.numberOfNukesFired;
            gameStats.numberOfCapsCompleted += stats.numberOfCapsCompleted;
            gameStats.numberOfMultipliersCapped += stats.numberOfMultipliersCapped;

            SaveStatsData(gameStats);
        }

        public void SaveStatsData(GameboardStats stats)
        {

        }

        public void SaveChallengeStatuses(List<ChallengeSaveData> challengeSaveData)
        {

        }

    }
}
