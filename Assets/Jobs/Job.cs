using System;
using System.Linq;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jobs
{
    public class Job : MonoBehaviour
    {
        public string title;
        public string description;
        public string lockedMessage;
        public string successMessage;
        public string failureMessage;

        public JobReward reward;
        public int durationInHours;
        public int failureCooldownInHours;

        public JobRequirement[] unlockRequirements;
        public JobRequirement[] successRequirements;

        public bool IsUnlocked => unlockRequirements.All(x => x.IsMet());

        public bool Attempt()
        {
            var success = successRequirements.All(x => x.Attempt());

            // TODO: Give rewards / set cooldown
            if (success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static GameState GameState => GameState.Instance;
    }

    [Serializable]
    public class JobRequirement
    {
        public SkillEnum skill;
        public int value;

        private static GameState GameState => GameState.Instance;

        private static Skill GetSkill(SkillEnum skillEnum) => skillEnum switch
        {
            SkillEnum.Strength => GameState.Strength,
            SkillEnum.Intelligence => GameState.Intelligence,
            SkillEnum.Charisma => GameState.Charisma,
            _ => throw new ArgumentOutOfRangeException(),
        };

        public bool IsMet() => GetSkill(skill).Value >= value;
        public bool Attempt() => Random.Range(0, 100) < GetSkill(skill).Value / value * 100;
    }

    [Serializable]
    public class JobReward
    {
        public RewardType type;
        public int value;
    }
}