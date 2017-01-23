using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.Utils.Difficulty;
using Switch.Utils.Difficulty.DifficultyObjects;

namespace Switch.HighScores
{
    class HighScoreManager
    {
        private static HighScoreManager instance;
        private List<HighScore> highScores { get; set; }
        public static int MaxNumberOfHighScores = 9;

        private HighScoreManager() 
        {
            highScores = new List<HighScore>();
        }

        public static HighScoreManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HighScoreManager();
                }
                return instance;
            }
        }

        public List<HighScore> GetHighScores(String difficulty) 
        {
            List<HighScore> scoresOfSpecificDifficulty = new List<HighScore>();

            foreach (HighScore highScore in highScores)
            {
                if (highScore.GetDifficultyAsString().Equals(difficulty))
                {
                    scoresOfSpecificDifficulty.Add(highScore);
                }
            }

            return scoresOfSpecificDifficulty;
        }

        public void AddHighScore(HighScore highScore)
        {
            this.highScores.Add(highScore);
            TrimHighScoreList(highScore.GetDifficultyAsString());
            //StorageManager.Instance.saveHighScores();
        }

        private void TrimHighScoreList(String difficulty)
        {
            //sort the existing high scores
            List<HighScore> highScoresOfThisDifficulty = GetHighScores(difficulty);
            highScoresOfThisDifficulty.Sort();

            //remove any from the list that arent in the top max
            if (highScoresOfThisDifficulty.Count > MaxNumberOfHighScores)
            {
                for (int i = 0; i < highScoresOfThisDifficulty.Count; i++)
                {
                    if (i >= MaxNumberOfHighScores)
                    {
                        highScores.Remove(highScoresOfThisDifficulty[i]);
                    }
                }
            }

            //sort the master list
            highScores.Sort();
        }
    }
}
