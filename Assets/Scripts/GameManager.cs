using System;
using System.Linq;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Time = Model.Time;

[RequireComponent(typeof(TimeWarp))]
public class GameManager : MonoBehaviour
{
    public int endOfDayHour;
    public GameObject expensesPanel;
    public TextMeshProUGUI expensesContent;
    public Button nextDayButton;

    private TimeWarp _timeWarp;
    private DateTime _tomorrow;

    private void Start()
    {
        _timeWarp = GetComponent<TimeWarp>();
        nextDayButton.onClick.AddListener(() =>
        {
            expensesPanel.SetActive(false);
            GameState.Instance.Days.Value++;
        });
    }

    private void Update()
    {
        var tomorrow = Time.FirstDay.AddDays(GameState.Instance.Days.Value + 1);
        if (TimeWarp.TimeIsWarping)
        {
            // We may be warping due to non-end of day reasons
            if (tomorrow == _tomorrow && !expensesPanel.activeSelf)
            {
                // Show expenses window while warping
                expensesContent.text = string.Join('\n', Expenses.Expenses.GetExpenses()
                    .Select(x => string.Join('\n', $"{x.Title} = ${x.Cost:n0}", x.Description)));
                expensesPanel.SetActive(true);
            }
        }
        else if (GameState.Instance.Time.Value.Hour >= endOfDayHour)
        {
            // Start warping
            _tomorrow = Time.FirstDay.AddDays(GameState.Instance.Days.Value + 1);
            _timeWarp.SkipUntil(_tomorrow);
        }
    }
}