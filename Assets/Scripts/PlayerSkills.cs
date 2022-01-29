using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{
    public TextMeshProUGUI strengthLabel;
    public Button strengthButton;
    public TextMeshProUGUI intelligenceLabel;
    public Button intelligenceButton;
    public TextMeshProUGUI charismaLabel;
    public Button charismaButton;

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
    }

    private void OnGUI()
    {
        strengthLabel.text = $"Strength: {_gameState.Strength.Value:n}";
        intelligenceLabel.text = $"Intelligence: {_gameState.Intelligence.Value:n}";
        charismaLabel.text = $"Charisma: {_gameState.Charisma.Value:n}";
    }
}