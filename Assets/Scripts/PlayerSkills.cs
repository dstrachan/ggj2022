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

    // Time
    public Slider gameSpeedSlider;

    // Misc
    public Button resetButton;

    private static GameState GameState => GameState.Instance;

    private void Start()
    {
        strengthButton.onClick.AddListener(() =>
        {
            GameState.Strength.Xp += 10;
        });
        intelligenceButton.onClick.AddListener(() =>
        {
            GameState.Intelligence.Xp += 10;
        });
        charismaButton.onClick.AddListener(() =>
        {
            GameState.Charisma.Xp += 10;
        });

        gameSpeedSlider.onValueChanged.AddListener(value => GameState.TimeSpeed = value);

        resetButton.onClick.AddListener(() => Model.GameState.Reset());
    }

    private void OnGUI()
    {
        strengthValue.text = $"{GameState.Strength.Value:n}";
        intelligenceValue.text = $"{GameState.Intelligence.Value:n}";
        charismaValue.text = $"{GameState.Charisma.Value:n}";
    }
}