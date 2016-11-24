using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Switch.GameObjects.Challenges.ChallengeObjects;

namespace Switch.GameObjects.Challenges
{
    class ChallengeManager
    {
        private static ChallengeManager instance;
        public enum ChallengeLevel { EASY, MEDIUM, HARD, CRAZY, IMPOSSIBLE };
        private List<Challenge> easyChallenges;
        private List<Challenge> mediumChallenges;
        private List<Challenge> hardChallenges;
        private List<Challenge> crazyChallenges;
        private List<Challenge> impossibleChallenges;
        public static ChallengeLevel lastKnownChallengeLevel;
        private Dictionary<string, ChallengeSaveData> challengeSaveData;

        private ChallengeManager()
        {
            challengeSaveData = new Dictionary<String, ChallengeSaveData>();

            lastKnownChallengeLevel = ChallengeLevel.EASY;

            easyChallenges = new List<Challenge>();
            mediumChallenges = new List<Challenge>();
            hardChallenges = new List<Challenge>();
            crazyChallenges = new List<Challenge>();
            impossibleChallenges = new List<Challenge>();

            AddChallenge(new Score1000(), ChallengeLevel.EASY);
            AddChallenge(new DestroyTiles25(), ChallengeLevel.EASY);
            AddChallenge(new SurviveLevels5(), ChallengeLevel.EASY);
            AddChallenge(new FireLasers2(), ChallengeLevel.EASY);
            AddChallenge(new FireNukes1(), ChallengeLevel.EASY);

            AddChallenge(new Score3000(), ChallengeLevel.MEDIUM);
            AddChallenge(new DestroyTiles100(), ChallengeLevel.MEDIUM);
            AddChallenge(new SurviveLevels10(), ChallengeLevel.MEDIUM);
            AddChallenge(new CapTiles20(), ChallengeLevel.MEDIUM);
            AddChallenge(new FireNukes3(), ChallengeLevel.MEDIUM);

            AddChallenge(new Score5000(), ChallengeLevel.HARD);
            AddChallenge(new DestroyTilesWithLaser50(), ChallengeLevel.HARD);
            AddChallenge(new DestroyTilesWithNuke50(), ChallengeLevel.HARD);
            AddChallenge(new CapMultipliers(), ChallengeLevel.HARD);
            AddChallenge(new CompleteCap10(), ChallengeLevel.HARD);

            AddChallenge(new Score15000(), ChallengeLevel.CRAZY);
            AddChallenge(new DestroyTiles200(), ChallengeLevel.CRAZY);
            AddChallenge(new SurviveLevels10Hard(), ChallengeLevel.CRAZY);
            AddChallenge(new CompleteCap25(), ChallengeLevel.CRAZY);
            AddChallenge(new FireNukes7(), ChallengeLevel.CRAZY);

            AddChallenge(new Score100000(), ChallengeLevel.IMPOSSIBLE);
            AddChallenge(new DestroyTiles500(), ChallengeLevel.IMPOSSIBLE);
            AddChallenge(new FireNukeDuringBT(), ChallengeLevel.IMPOSSIBLE);
            AddChallenge(new SurviveMinutes10(), ChallengeLevel.IMPOSSIBLE);
            AddChallenge(new FireNukes15(), ChallengeLevel.IMPOSSIBLE);

        }

        public static ChallengeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ChallengeManager();
                }
                return instance;
            }
        }

        public int GetPercentOfChallengesCompleted()
        {
            int percentCompleted = 0;
            int totalChallenges = 0;
            int totalChallengesCompleted = 0;

            totalChallenges = challengeSaveData.Count;
            foreach (ChallengeSaveData challenge in GetChallengeSaveData())
            {
                if (challenge.IsChallengeCompleted)
                {
                    totalChallengesCompleted++;
                }
            }

            try
            {
                percentCompleted = (int)(((float)totalChallengesCompleted / (float)totalChallenges) * 100);
            }
            catch (Exception e) 
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            if (percentCompleted < 0)
            {
                percentCompleted = 0;
            }
            
            return percentCompleted;
        }

        public void AddChallenge(Challenge challenge, ChallengeLevel level)
        {
            if (level == ChallengeLevel.EASY)
            {
                easyChallenges.Add(challenge);
            }
            else if (level == ChallengeLevel.MEDIUM)
            {
                mediumChallenges.Add(challenge);
            }
            else if (level == ChallengeLevel.HARD)
            {
                hardChallenges.Add(challenge);
            }
            else if (level == ChallengeLevel.CRAZY)
            {
                crazyChallenges.Add(challenge);
            }
            else
            {
                impossibleChallenges.Add(challenge);
            }
        }

        public List<Challenge> GetChallenges()
        {
            return GetAllChallengesAsList();
        }

        public List<Challenge> GetChallenges(ChallengeLevel level)
        {
            if (level == ChallengeLevel.EASY)
            {
                return easyChallenges;
            }
            else if (level == ChallengeLevel.MEDIUM)
            {
                return mediumChallenges;
            }
            else if (level == ChallengeLevel.HARD)
            {
                return hardChallenges;
            }
            else if (level == ChallengeLevel.CRAZY)
            {
                return crazyChallenges;
            }
            else
            {
                return impossibleChallenges;
            }
        }

        public Challenge GetChallengeByName(String name)
        {
            foreach (Challenge challenge in GetAllChallengesAsList())
            {
                if (challenge.getName().Equals(name))
                {
                    return challenge;
                }
            }

            return null;
        }

        public void SetChallengeCompleteStatus(String name, bool isComplete)
        {
            if (challengeSaveData.ContainsKey(name))
            {
                challengeSaveData[name].IsChallengeCompleted = isComplete;
            }
        }

        public List<ChallengeSaveData> CreateNewChallengeSaveData()
        {
            List<ChallengeSaveData> challengeSaveDataList = new List<ChallengeSaveData>();
            foreach (Challenge challenge in GetAllChallengesAsList())
            {
                ChallengeSaveData data = new ChallengeSaveData(challenge.getName(), false);
                challengeSaveDataList.Add(data);

                if (!this.challengeSaveData.ContainsKey(challenge.getName()))
                {
                    this.challengeSaveData.Add(challenge.getName(), data);
                }
            }

            return challengeSaveDataList;
        }

        public List<ChallengeSaveData> GetChallengeSaveData() 
        {
            return new List<ChallengeSaveData>(challengeSaveData.Values);
        }

        public bool GetChallengeStatus(String name)
        {
            if (challengeSaveData.ContainsKey(name))
            {
                return challengeSaveData[name].IsChallengeCompleted;
            }
            else
            {
                return false;
            }
        }

        private List<Challenge> GetAllChallengesAsList()
        {
            List<Challenge> allChallenges = new List<Challenge>();

            foreach (Challenge challenge in easyChallenges)
            {
                allChallenges.Add(challenge);
            }

            foreach (Challenge challenge in mediumChallenges)
            {
                allChallenges.Add(challenge);
            }

            foreach (Challenge challenge in hardChallenges)
            {
                allChallenges.Add(challenge);
            }

            foreach (Challenge challenge in crazyChallenges)
            {
                allChallenges.Add(challenge);
            }

            foreach (Challenge challenge in impossibleChallenges)
            {
                allChallenges.Add(challenge);
            }

            return allChallenges;
        }
    }
}
