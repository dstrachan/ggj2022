using System;
using Model;
using UnityEngine;
using Time = Model.Time;

[RequireComponent(typeof(TimeWarp))]
public class GameManager : MonoBehaviour
{
    public int EndOfDayHour;
    private TimeWarp _timeWarp;

    void Start()
    {
        _timeWarp = GetComponent<TimeWarp>();
    }

    void Update()
    {
        if (GameState.Instance.Time.Value.Hour >= EndOfDayHour)
        {
            var tomorrowMorn = Time.FirstDay.AddDays(GameState.Instance.Days.Value + 1);

            // var timeSpan = tomorrowMorn - GameState.Instance.Time.Value;
            //
            // var adjusted = tomorrowMorn.Subtract(timeSpan / _timeWarp.warpSpeed);
            
            //print(timeSpan / _timeWarp.warpSpeed);
            _timeWarp.SkipUntil(tomorrowMorn);
            
            GameState.Instance.Days.Value++;
        }
    }
}
