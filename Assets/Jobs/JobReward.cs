using System;
using Model;

namespace Jobs
{
    [Serializable]
    public class JobReward
    {
        public RewardType type;
        public int value;

        private static GameState GameState => GameState.Instance;

        public void Give()
        {
            switch (type)
            {
                case RewardType.Money:
                    GameState.Money += value;
                    break;
                case RewardType.Family:
                    GameState.Family += value;
                    break;
                case RewardType.StrengthXp:
                    GameState.Strength.Xp += value;
                    break;
                case RewardType.IntelligenceXp:
                    GameState.Intelligence.Xp += value;
                    break;
                case RewardType.CharismaXp:
                    GameState.Charisma.Xp += value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}