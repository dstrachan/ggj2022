using System;
using Model;
using UnityEngine;
using Time = Model.Time;

[RequireComponent(typeof(TimeWarp))]
public class GameManager : MonoBehaviour
{
    public int endOfDayHour;
    private TimeWarp _timeWarp;

    void Start()
    {
        _timeWarp = GetComponent<TimeWarp>();
    }

    void Update()
    {
        if (GameState.Instance.Time.Value.Hour < endOfDayHour) return;
        
        var tomorrowMorn = Time.FirstDay.AddDays(GameState.Instance.Days.Value + 1);

        _timeWarp.SkipUntil(tomorrowMorn);
            
        GameState.Instance.Days.Value++;
    }
}
