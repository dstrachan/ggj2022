using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsSetter : MonoBehaviour
{
    public TextMeshProUGUI moneyValue;
    public TextMeshProUGUI familyValue;
    public TextMeshProUGUI strengthValue;
    public Button strengthButton;
    public TextMeshProUGUI intelligenceValue;
    public Button intelligenceButton;
    public TextMeshProUGUI charismaValue;
    public Button charismaButton;

    private void Start()
    {
        strengthButton.onClick.AddListener(() => GameState.Instance.Strength.Xp += 10);
        intelligenceButton.onClick.AddListener(() => GameState.Instance.Intelligence.Xp += 10);
        charismaButton.onClick.AddListener(() => GameState.Instance.Charisma.Xp += 10);
    }

    private void OnGUI()
    {
        moneyValue.text = $"{GameState.Instance.Money:n0}";
        familyValue.text = $"{GameState.Instance.Family:n0}";
        strengthValue.text = $"{GameState.Instance.Strength.Value:n}";
        intelligenceValue.text = $"{GameState.Instance.Intelligence.Value:n}";
        charismaValue.text = $"{GameState.Instance.Charisma.Value:n}";
    }
}