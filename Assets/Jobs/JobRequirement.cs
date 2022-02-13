using System;
using Model;
using Random = UnityEngine.Random;

namespace Jobs
{
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
}