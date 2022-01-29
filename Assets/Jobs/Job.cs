using System;
using System.Linq;
using Model;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jobs
{
    public class Job : MonoBehaviour
    {
        public string lockedMessage;
        public string disabledMessage;
        public string enabledMessage;
        public string successMessage;
        public string failureMessage;

        public JobReward[] rewards;
        public int durationInHours;
        public int failureCooldownInHours;

        public JobRequirement[] unlockRequirements;
        public JobRequirement[] successRequirements;

        public GameObject billboard;

        public bool IsEnabled => _disabledUntil <= GameState.Time.Value;
        public bool IsUnlocked => unlockRequirements.All(x => x.IsMet());

        private DateTime _disabledUntil;
        private GameObject _billboardInstance;
        private bool _canStartJob;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                _canStartJob = IsUnlocked && IsEnabled;

                _billboardInstance = Instantiate(billboard, transform);
                _billboardInstance.transform.localPosition = new Vector3(0, 2, 0);
                UpdateBillboard();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                UpdateBillboard();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                _canStartJob = false;
                Destroy(_billboardInstance);
            }
        }

        private void Update()
        {
            if (_canStartJob && Input.GetMouseButtonDown(0))
            {
                Attempt();
                UpdateBillboard();
            }
        }

        private void UpdateBillboard()
        {
            var billboardText = _billboardInstance.GetComponent<TextMeshPro>();
            if (IsUnlocked)
            {
                billboardText.text = IsEnabled ? enabledMessage : disabledMessage;
            }
            else
            {
                billboardText.text = lockedMessage;
            }
        }

        public void Attempt()
        {
            if (!IsEnabled) return; // TODO: Message

            GameState.SkipTimeForDuration(TimeSpan.FromHours(durationInHours));

            var success = successRequirements.All(x => x.Attempt());
            if (success)
            {
                foreach (var reward in rewards)
                {
                    reward.Give();
                }
            }
            else
            {
                _disabledUntil = GameState.Time.Value.Add(TimeSpan.FromHours(failureCooldownInHours));
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

        private static GameState GameState => GameState.Instance;

        public void Give()
        {
            switch (type)
            {
                case RewardType.Money:
                    GameState.Money += value;
                    break;
                case RewardType.Love:
                    GameState.Love += value;
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