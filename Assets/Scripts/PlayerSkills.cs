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
            GameState.Days.Value++;
        });
        intelligenceButton.onClick.AddListener(() =>
        {
            GameState.Intelligence.Xp += 10;
            GameState.Days.Value++;
        });
        charismaButton.onClick.AddListener(() =>
        {
            GameState.Charisma.Xp += 10;
            GameState.Days.Value++;
        });

        gameSpeedSlider.onValueChanged.AddListener(value => GameState.Time.Factor = value);

        resetButton.onClick.AddListener(() => GameState.Reset());
    }

    private void OnGUI()
    {
        strengthValue.text = $"{GameState.Strength.Value:n}";
        intelligenceValue.text = $"{GameState.Intelligence.Value:n}";
        charismaValue.text = $"{GameState.Charisma.Value:n}";
    }
}