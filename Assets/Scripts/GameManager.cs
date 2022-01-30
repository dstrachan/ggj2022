using System;
using Model;
using UnityEngine;
using Time = Model.Time;

[RequireComponent(typeof(TimeWarp))]
public class GameManager : MonoBehaviour
{
    public int endOfDayHour;

    private TimeWarp _timeWarp;
    private DateTime _tomorrow;
    private bool _showExpenses;

    private void Start()
    {
        _timeWarp = GetComponent<TimeWarp>();
    }

    private void Update()
    {
        var tomorrow = Time.FirstDay.AddDays(GameState.Instance.Days.Value + 1);
        if (TimeWarp.TimeIsWarping)
        {
            // We may be warping due to non-end of day reasons
            if (tomorrow == _tomorrow)
            {
                // Show expenses window while warping
                _showExpenses = true;
            }
        }
        else if (GameState.Instance.Time.Value.Hour >= endOfDayHour)
        {
            // Start warping
            _tomorrow = Time.FirstDay.AddDays(GameState.Instance.Days.Value + 1);
            _timeWarp.SkipUntil(_tomorrow);
        }
        else if (tomorrow == _tomorrow && !_showExpenses)
        {
            // Finished warping and looking at expenses
            GameState.Instance.Days.Value++;
        }
    }

    private void OnGUI()
    {
        if (_showExpenses)
        {
        }
    }
}