using System;
using System.Linq;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Jobs
{
    [RequireComponent(typeof(TimeWarp))]
    [RequireComponent(typeof(Collider))]
    public class Job : MonoBehaviour
    {
        public string JobTitle;
        
        [TextArea(4,10)]
        public string JobDescription;
        
        [TextArea(4,10)]
        public string DisabledDescription;
        
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
        private bool IsTooExpensive => GameState.Money >= cost;
        private bool IsDisabled => _disabledUntil > GameState.Time.Value;
        
        private bool IsUnlocked => unlockRequirements.All(x => x.IsMet());

        private DateTime _disabledUntil;
        private GameObject _billboardInstance;
        private bool _canStartJob;
        private bool _inTrigger;

        private Button _acceptButton;
        private GameObject _acceptPanel;
        private TextMeshProUGUI _jobContentMesh;
        private TextMeshProUGUI _jobTitleMesh;
        private TextMeshProUGUI _jobDurationMesh;
        private TextMeshProUGUI _jobRequiresMesh;
        private TextMeshProUGUI _jobRewardMesh;
        private TextMeshProUGUI _jobAcceptTextMesh;
        private TextMeshPro _billboardText;

        private TimeWarp _timeWarp;
        private DateTime _jobStartTime = DateTime.MaxValue;
        private bool _jobStarted = false;
        
        private void Start()
        {
            _acceptButton = GameObject.FindGameObjectWithTag(Tags.JobAccept).GetComponent<Button>();
            _acceptPanel = GameObject.FindGameObjectWithTag(Tags.JobPanel);
            _jobContentMesh = GameObject.FindGameObjectWithTag(Tags.JobContent).GetComponent<TextMeshProUGUI>();
            _jobTitleMesh = GameObject.FindGameObjectWithTag(Tags.JobTitle).GetComponent<TextMeshProUGUI>();
            _jobDurationMesh = GameObject.FindGameObjectWithTag(Tags.JobDuration).GetComponent<TextMeshProUGUI>();
            _jobRequiresMesh = GameObject.FindGameObjectWithTag(Tags.JobRequires).GetComponent<TextMeshProUGUI>();
            _jobRewardMesh = GameObject.FindGameObjectWithTag(Tags.JobReward).GetComponent<TextMeshProUGUI>();
            _jobAcceptTextMesh = GameObject.FindGameObjectWithTag(Tags.JobAcceptText).GetComponent<TextMeshProUGUI>();
            
            _timeWarp = GetComponent<TimeWarp>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                _inTrigger = true;
                _canStartJob = IsUnlocked && IsEnabled;

                UpdateJobBoard();

                _billboardInstance = Instantiate(billboard, transform);
                _billboardInstance.transform.localPosition = new Vector3(0, 2, 0);
                _billboardText = _billboardInstance.GetComponent<TextMeshPro>();
                UpdateBillboard();
            }
        }

        private void UpdateJobBoard()
        {
            _acceptButton.onClick.RemoveAllListeners();  
            _acceptButton.onClick.AddListener(Attempt);
            _acceptButton.GetComponentInChildren<TextMeshProUGUI>().text = JobActionText;
            
            print(IsDisabled);
            if (IsDisabled)
            {
                _jobContentMesh.text = DisabledDescription;
                var lockedTime = (_disabledUntil - GameState.Time.Value);
                _jobAcceptTextMesh.text =
                    $"<color=#d43131><b>Job available in: {lockedTime.TotalHours.ToString("F1")} hours</b></color>";
            }
            else 
            {
                _jobContentMesh.text = JobDescription;
            }
        
            _jobTitleMesh.text = JobTitle;

            _jobDurationMesh.text = $"Takes:\n <color=orange><b>{durationInHours}</b></color> hours";

            _jobRequiresMesh.text = "Requires:\n";
            
            bool lackRequirement = false;
            foreach (var require in unlockRequirements)
            {
                if (require.IsMet())
                {
                    _jobRequiresMesh.text += $"<color=green><b>{require.value}</b></color> {require.skill}\n";
                }
                else
                {
                    lackRequirement = true;
                    _jobRequiresMesh.text += $"<color=red><b>{require.value}</b></color> {require.skill}\n";
                }
            }

            if (lackRequirement && !IsDisabled)
            {
                _jobAcceptTextMesh.text =
                    $"<color=#d43131><b>You lack the skills for this job!</b></color>";
            }

            _jobRewardMesh.text = "Reward:\n";
            foreach (var reward in rewards)
            {
                _jobRewardMesh.text += $"<color=green><b>{reward.value}</b></color> {reward.type}\n";
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                _inTrigger = false;
                _canStartJob = false;
                Destroy(_billboardInstance);
            }
        }

        private void Update()
        {
            _acceptPanel.gameObject.SetActive(_inTrigger && !TimeWarp.TimeIsWarping);
            _acceptButton.gameObject.SetActive(_canStartJob && !IsDisabled && !TimeWarp.TimeIsWarping);
            
            _jobAcceptTextMesh.gameObject.SetActive(IsDisabled || !_canStartJob && !TimeWarp.TimeIsWarping);

            if (_billboardText != null && _jobStarted && _canStartJob && _jobStartTime + TimeSpan.FromHours(durationInHours) < GameState.Instance.Time.Value)
            {
                _jobStarted = false;
                UpdateBillboard();
                UpdateJobBoard();
            }
            
            if (Input.GetButton("JoyJump"))
            {
                Attempt();
            }
        }

        private void UpdateBillboard()
        {
            if (IsUnlocked)
            {
                _billboardText.color = Color.white;
                _billboardText.text = IsEnabled ? enabledMessage : disabledMessage;
            }
            else
            {
                _billboardText.color = Color.gray;
                _billboardText.text = lockedMessage;
            }
        }

        private void Attempt()
        {
            if (TimeWarp.TimeIsWarping) return;
            
            if (!IsEnabled) return; 

            GameState.Money -= cost;

            _jobStarted = true;
            _jobStartTime = GameState.Instance.Time.Value;
            
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