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
    public TextMeshProUGUI newTimeValue;

    private GameState _gameState;

    private void Awake()
    {
        _gameState = GameState.Load();
    }

    private void Start()
    {
        strengthButton.onClick.AddListener(() => _gameState.Strength.Xp++);
        intelligenceButton.onClick.AddListener(() => _gameState.Intelligence.Xp++);
        charismaButton.onClick.AddListener(() => _gameState.Charisma.Xp++);

        // carpetFactoryButton.onClick.AddListener(() => );
        // sellDrugsButton.onClick.AddListener(() => );
        // commitFraudButton.onClick.AddListener(() => );
        // mugPeopleButton.onClick.AddListener(() => );
        // robBankButton.onClick.AddListener(() => );
        // stealIdentitiesButton.onClick.AddListener(() => );

        gameSpeedSlider.onValueChanged.AddListener(value => _gameState.Time.Factor = value);

        resetButton.onClick.AddListener(() => _gameState = GameState.Reset());
    }

    private void OnGUI()
    {
        strengthValue.text = $"{_gameState.Strength.Value:n}";
        intelligenceValue.text = $"{_gameState.Intelligence.Value:n}";
        charismaValue.text = $"{_gameState.Charisma.Value:n}";

        newTimeValue.text = $"{_gameState.Time.Value:s}";
    }
}