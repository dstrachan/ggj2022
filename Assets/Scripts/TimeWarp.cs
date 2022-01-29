﻿using System;
using System.Collections;
using Model;
using UnityEngine;

public class TimeWarp : MonoBehaviour
{
    public static bool TimeIsWarping = false;
    
    public int warpSpeed = 3600;
    public void SkipTimeForDuration(TimeSpan duration)
    {
        var currentTime = GameState.Instance.Time.Value;
        var endTime = currentTime.Add(duration);
        //throw new NotImplementedException();

        TimeIsWarping = true;
        GameState.Instance.Time.Factor = warpSpeed;
        StartCoroutine(nameof(Warp), endTime);
    }
    
    IEnumerator Warp(DateTime endTime)
    {
        while (GameState.Instance.Time.Value < endTime)
        {
            yield return new WaitForSeconds(1);
        }
        
        GameState.Instance.Time.Factor = 1;
        TimeIsWarping = false;
    }

}
