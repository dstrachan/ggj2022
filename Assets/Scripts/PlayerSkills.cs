using System;
using Jobs;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{
    // Skills
    public TextMeshProUGUI strengthValue;
    public Button strengthButton;
    public TextMeshProUGUI intelligenceValue;
    public Button intelligenceButton;
    public TextMeshProUGUI charismaValue;
    public Button charismaButton;

    // Jobs
    public Button carpetFactoryButton;
    public Button sellDrugsButton;
    public Button commitFraudButton;
    public Button mugPeopleButton;
    public Button robBankButton;
    public Button stealIdentitiesButton;

    // Time
    public Slider gameSpeedSlider;

    // Misc
    public Button resetButton;

    // Test
    public TextMeshProUGUI timeValue;
    private Job _job;

    private static GameState GameState => GameState.Instance;

    private void Awake()
    {
        _job = GetComponent<Job>();
    }

    private void Start()
    {
        strengthButton.onClick.AddListener(() => GameState.Strength.Xp++);
        intelligenceButton.onClick.AddListener(() => GameState.Intelligence.Xp++);
        charismaButton.onClick.AddListener(() => GameState.Charisma.Xp++);

        // carpetFactoryButton.onClick.AddListener(() => );
        // sellDrugsButton.onClick.AddListener(() => );
        // commitFraudButton.onClick.AddListener(() => );
        // mugPeopleButton.onClick.AddListener(() => );
        // robBankButton.onClick.AddListener(() => );
        // stealIdentitiesButton.onClick.AddListener(() => );

        gameSpeedSlider.onValueChanged.AddListener(value => GameState.Time.Factor = value);

        resetButton.onClick.AddListener(() => GameState.Reset());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _job.Attempt();
        }
    }

    private void OnGUI()
    {
        strengthValue.text = $"{GameState.Strength.Value:n}";
        intelligenceValue.text = $"{GameState.Intelligence.Value:n}";
        charismaValue.text = $"{GameState.Charisma.Value:n}";

        timeValue.text = $"{GameState.Time.Value:s}";
    }
}