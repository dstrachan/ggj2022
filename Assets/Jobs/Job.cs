﻿using System;
using System.Linq;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Jobs
{
    [RequireComponent(typeof(TimeWarp))]
    public class Job : MonoBehaviour
    {
        public string lockedMessage;
        public string disabledMessage;
        public string enabledMessage;
        public string successMessage;
        public string failureMessage;
        
        public string JobActionText;

        public long cost;
        public JobReward[] rewards;
        public int durationInHours;
        public int failureCooldownInHours;

        public JobRequirement[] unlockRequirements;
        public JobRequirement[] successRequirements;

        public GameObject billboard;
        
        private bool IsEnabled => GameState.Money >= cost && _disabledUntil <= GameState.Time.Value;
        private bool IsUnlocked => unlockRequirements.All(x => x.IsMet());

        private DateTime _disabledUntil;
        private GameObject _billboardInstance;
        private bool _canStartJob;

        private Button _acceptButton;
        private TextMeshPro _billboardText;

        private TimeWarp _timeWarp;
        
        private void Start()
        {
            _acceptButton = GameObject.FindGameObjectWithTag(Tags.JobAccept).GetComponent<Button>();
            _timeWarp = GetComponent<TimeWarp>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                _acceptButton.onClick.RemoveAllListeners();
                _acceptButton.onClick.AddListener(Attempt);
                _acceptButton.GetComponentInChildren<TextMeshProUGUI>().text = JobActionText;
                
                _canStartJob = IsUnlocked && IsEnabled;

                _billboardInstance = Instantiate(billboard, transform);
                _billboardInstance.transform.localPosition = new Vector3(0, 2, 0);
                _billboardText = _billboardInstance.GetComponent<TextMeshPro>();
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
            _acceptButton.gameObject.SetActive(_canStartJob);
        }

        private void UpdateBillboard()
        {
            if (IsUnlocked)
            {
                _billboardText.text = IsEnabled ? enabledMessage : disabledMessage;
            }
            else
            {
                _billboardText.text = lockedMessage;
            }
        }

        private void Attempt()
        {
            if (!IsEnabled) return; 

            GameState.Money -= cost;
            _timeWarp.SkipTimeForDuration(TimeSpan.FromHours(durationInHours));

            var success = successRequirements.All(x => x.Attempt());
            if (success)
            {
                foreach (var reward in rewards)
                {
                    _billboardText.color = Color.green;
                    _billboardText.text = successMessage;
                    reward.Give();
                }
            }
            else
            {
                _billboardText.color = Color.red;
                _billboardText.text = failureMessage;
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