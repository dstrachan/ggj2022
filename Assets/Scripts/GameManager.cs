using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject _player;
    public const float WarpSpeed = 3600;
    public bool TimeIsWarping => TimeToWarpTo != null;
    public DateTime? TimeToWarpTo { get; private set; }

    public void WarpTo(TimeSpan timeSpan) => WarpTo(GameState.Instance.Time + timeSpan);
    
    public void WarpTo(DateTime timeToWarpTo)
    {
        if (TimeIsWarping)
        {
            Debug.LogError("WarpTo() was called while already warping. Warping to later of the 2 times");
        }
        if (timeToWarpTo < GameState.Instance.Time)
        {
            Debug.LogWarning("WarpTo() was called with a time less than the current time. Ignoring");
            return;
        }

        GameState.Instance.TimeSpeed = WarpSpeed;
        TimeToWarpTo = timeToWarpTo;
    }

    void FixedUpdate()
    {
        var gs = GameState.Instance;
        
        // Tick the time and manage time warping.
        {
            var newTime = gs.Time + TimeSpan.FromSeconds(Time.deltaTime * gs.TimeSpeed);
            if (TimeToWarpTo != null && newTime > TimeToWarpTo.Value)
            {
                // Stop the time warp
                gs.Time = TimeToWarpTo.Value;
                TimeToWarpTo = null;
                GameState.Instance.TimeSpeed = 1;
                
                // We save after every time warp
                GameState.Instance.Save();
            }
            else
            {
                gs.Time = newTime;
            }
        }
    }
}
