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
        public enum ChallengeLevel { Easy, Medium, Hard, Crazy, Impossible };
        private List<IChallenge> easyChallenges;
        private List<IChallenge> mediumChallenges;
        private List<IChallenge> hardChallenges;
        private List<IChallenge> crazyChallenges;
        private List<IChallenge> impossibleChallenges;
        public static ChallengeLevel lastKnownChallengeLevel;
        private Dictionary<string, ChallengeSaveData> challengeSaveData;

        private ChallengeManager()
        {
            challengeSaveData = new Dictionary<String, ChallengeSaveData>();

            lastKnownChallengeLevel = ChallengeLevel.Easy;

            easyChallenges = new List<IChallenge>();
            mediumChallenges = new List<IChallenge>();
            hardChallenges = new List<IChallenge>();
            crazyChallenges = new List<IChallenge>();
            impossibleChallenges = new List<IChallenge>();

            AddChallenge(new Score1000(), ChallengeLevel.Easy);
            AddChallenge(new DestroyTiles25(), ChallengeLevel.Easy);
            AddChallenge(new SurviveLevels5(), ChallengeLevel.Easy);
            AddChallenge(new FireLasers2(), ChallengeLevel.Easy);
            AddChallenge(new FireNukes1(), ChallengeLevel.Easy);

            AddChallenge(new Score3000(), ChallengeLevel.Medium);
            AddChallenge(new DestroyTiles100(), ChallengeLevel.Medium);
            AddChallenge(new SurviveLevels10(), ChallengeLevel.Medium);
            AddChallenge(new CapTiles20(), ChallengeLevel.Medium);
            AddChallenge(new FireNukes3(), ChallengeLevel.Medium);

            AddChallenge(new Score5000(), ChallengeLevel.Hard);
            AddChallenge(new DestroyTilesWithLaser50(), ChallengeLevel.Hard);
            AddChallenge(new DestroyTilesWithNuke50(), ChallengeLevel.Hard);
            AddChallenge(new CapMultipliers(), ChallengeLevel.Hard);
            AddChallenge(new CompleteCap10(), ChallengeLevel.Hard);

            AddChallenge(new Score15000(), ChallengeLevel.Crazy);
            AddChallenge(new DestroyTiles200(), ChallengeLevel.Crazy);
            AddChallenge(new SurviveLevels10Hard(), ChallengeLevel.Crazy);
            AddChallenge(new CompleteCap25(), ChallengeLevel.Crazy);
            AddChallenge(new FireNukes7(), ChallengeLevel.Crazy);

            AddChallenge(new Score100000(), ChallengeLevel.Impossible);
            AddChallenge(new DestroyTiles500(), ChallengeLevel.Impossible);
            AddChallenge(new FireNukeDuringBT(), ChallengeLevel.Impossible);
            AddChallenge(new SurviveMinutes10(), ChallengeLevel.Impossible);
            AddChallenge(new FireNukes15(), ChallengeLevel.Impossible);

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

        public void AddChallenge(IChallenge challenge, ChallengeLevel level)
        {
            if (level == ChallengeLevel.Easy)
            {
                easyChallenges.Add(challenge);
            }
            else if (level == ChallengeLevel.Medium)
            {
                mediumChallenges.Add(challenge);
            }
            else if (level == ChallengeLevel.Hard)
            {
                hardChallenges.Add(challenge);
            }
            else if (level == ChallengeLevel.Crazy)
            {
                crazyChallenges.Add(challenge);
            }
            else
            {
                impossibleChallenges.Add(challenge);
            }
        }

        public List<IChallenge> GetChallenges()
        {
            return GetAllChallengesAsList();
        }

        public List<IChallenge> GetChallenges(ChallengeLevel level)
        {
            if (level == ChallengeLevel.Easy)
            {
                return easyChallenges;
            }
            else if (level == ChallengeLevel.Medium)
            {
                return mediumChallenges;
            }
            else if (level == ChallengeLevel.Hard)
            {
                return hardChallenges;
            }
            else if (level == ChallengeLevel.Crazy)
            {
                return crazyChallenges;
            }
            else
            {
                return impossibleChallenges;
            }
        }

        public IChallenge GetChallengeByName(String name)
        {
            foreach (IChallenge challenge in GetAllChallengesAsList())
            {
                if (challenge.GetName().Equals(name))
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
            foreach (IChallenge challenge in GetAllChallengesAsList())
            {
                ChallengeSaveData data = new ChallengeSaveData(challenge.GetName(), false);
                challengeSaveDataList.Add(data);

                if (!this.challengeSaveData.ContainsKey(challenge.GetName()))
                {
                    this.challengeSaveData.Add(challenge.GetName(), data);
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

        private List<IChallenge> GetAllChallengesAsList()
        {
            List<IChallenge> allChallenges = new List<IChallenge>();

            foreach (IChallenge challenge in easyChallenges)
            {
                allChallenges.Add(challenge);
            }

            foreach (IChallenge challenge in mediumChallenges)
            {
                allChallenges.Add(challenge);
            }

            foreach (IChallenge challenge in hardChallenges)
            {
                allChallenges.Add(challenge);
            }

            foreach (IChallenge challenge in crazyChallenges)
            {
                allChallenges.Add(challenge);
            }

            foreach (IChallenge challenge in impossibleChallenges)
            {
                allChallenges.Add(challenge);
            }

            return allChallenges;
        }
    }
}
