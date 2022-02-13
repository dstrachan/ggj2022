using System;
using System.Linq;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jobs
{
    [RequireComponent(typeof(Collider))]
    public class Job : MonoBehaviour
    {
        private static GameState GameState => GameState.Instance;
        
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

        public GameManager _gameManager;
        
        private bool IsEnabled => GameState.Money >= cost && _disabledUntil <= GameState.Time;
        private bool IsTooExpensive => GameState.Money < cost;
        private bool IsDisabled => _disabledUntil > GameState.Time;
        
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

        private bool _activeJob = false;
        
        private DateTime _jobStartTime = DateTime.MaxValue;
        private bool _jobStarted = false;
        private bool firstTime = true;
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
            _gameManager = GameObject.FindGameObjectWithTag(Tags.GameManager).GetComponent<GameManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                _activeJob = true;
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
            _acceptButton.GetComponentInChildren<TextMeshProUGUI>().text = $"(A) {JobActionText}";

            _jobContentMesh.text = JobDescription;
            if (IsDisabled)
            {
                if (!string.IsNullOrEmpty(DisabledDescription))
                {
                    _jobContentMesh.text = DisabledDescription;
                }

                var lockedTime = (_disabledUntil - GameState.Time);
                _jobAcceptTextMesh.text =
                    $"<color=#d43131><b>Job available in: {lockedTime.TotalHours.ToString("F1")} hours</b></color>";
            }
            else if (IsTooExpensive)
            {
                _jobAcceptTextMesh.text =
                    $"<color=#d43131><b>You can't afford to do this</b></color>";
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

            if (cost > 0)
            {
                if (GameState.Instance.Money >= cost)
                {
                    _jobRequiresMesh.text += $"<color=green>Costs ${cost}</color>";
                }
                else
                {
                    _jobRequiresMesh.text += $"<color=red>Costs ${cost}</color>";

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
                _activeJob = false;
                _acceptPanel.gameObject.SetActive(false);
                _inTrigger = false;
                _canStartJob = false;
                Destroy(_billboardInstance);
            }
        }

        private void Update()
        {
            if (firstTime)
            {
                firstTime = false;
                _acceptPanel.gameObject.SetActive(false);
            }
            
            if (_activeJob)
            {
                _acceptPanel.gameObject.SetActive(_inTrigger && !_gameManager.TimeIsWarping);
                _acceptButton.gameObject.SetActive(_canStartJob && !IsDisabled && !_gameManager.TimeIsWarping);
                _jobAcceptTextMesh.gameObject.SetActive(IsDisabled || !_canStartJob && !_gameManager.TimeIsWarping);

                if (_billboardText != null && _jobStarted && _canStartJob &&
                    _jobStartTime + TimeSpan.FromHours(durationInHours) < GameState.Instance.Time)
                {
                    _jobStarted = false;
                    UpdateBillboard();
                    UpdateJobBoard();
                }

                if (!_jobStarted && _canStartJob && Input.GetButtonDown("Action"))
                {
                    Attempt();
                }
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
                _billboardText.color = Color.red;
                _billboardText.text = lockedMessage;
            }
        }

        private void Attempt()
        {
            if (_gameManager.TimeIsWarping) return;
            
            if (!IsEnabled) return; 

            GameState.Money -= cost;

            _jobStarted = true;
            _jobStartTime = GameState.Instance.Time;
            
            _gameManager.WarpTo(TimeSpan.FromHours(durationInHours));

            var success = successRequirements.All(x => x.Attempt());
            if (success)
            {
                if (rewards.Any())
                {
                    foreach (var reward in rewards)
                    {
                        _billboardText.color = Color.green;
                        _billboardText.text = successMessage;
                        reward.Give();
                    }
                
                    // Rewards billboard
                    var rewardBillboard = Instantiate(billboard, _billboardInstance.transform.position, Quaternion.identity);
                    rewardBillboard.AddComponent<RewardBillboard>();
                    var text = rewardBillboard.GetComponent<TextMeshPro>();
                    text.text = "<color=#66FF66>TODO reward amount</color>";
                }
            }
            else
            {
                _billboardText.color = Color.red;
                _billboardText.text = failureMessage;
                _disabledUntil = GameState.Time.Add(TimeSpan.FromHours(failureCooldownInHours));
            }
        }
    }
}