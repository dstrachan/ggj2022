using System;
using System.Collections;
using Model;
using UnityEngine;
using UnityEngine.UI;
using Time = Model.Time;

[RequireComponent(typeof(TimeWarp))]
public class GameManager : MonoBehaviour
{
    public int endOfDayHour;

    private TimeWarp _timeWarp;
    private DateTime _tomorrow;
    private bool _showExpenses;
    private Image _fadeToBlack;
    private bool _fadingOut;

    private void Start()
    {
        _timeWarp = GetComponent<TimeWarp>();
        _fadeToBlack = GameObject.FindGameObjectWithTag(Tags.FadeToBlack).GetComponent<Image>();
    }

     void Update()
    {
        var tomorrow = Time.FirstDay.AddDays(GameState.Instance.Days.Value + 1);

        if (!_fadingOut && GameState.Instance.Time.Value.Hour >= endOfDayHour)
        {
            _fadingOut = true;
            StartCoroutine(nameof(FadeOut));
        }
        
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
    
     IEnumerator FadeIn()
    {
        for (int i = 0; i < 100; i++)
        {
            _fadeToBlack.color = new Color(_fadeToBlack.color.r, _fadeToBlack.color.g, _fadeToBlack.color.b, 1 - i/100f);
            yield return new WaitForSeconds(0.1f);
        }

        _fadingOut = false;
    }
    
    IEnumerator FadeOut()
    {
        for (int i = 1; i <= 100; i++)
        {
            _fadeToBlack.color = new Color(_fadeToBlack.color.r, _fadeToBlack.color.g, _fadeToBlack.color.b, (float)i/100f);
            yield return new WaitForSeconds(0.02f);
        }
       
    }

    private void OnGUI()
    {
        if (_showExpenses)
        {
        }
    }
}