using System;
using System.Linq;
using System.Collections;
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
    public TextMeshProUGUI endOfDayMessage;
    public Button nextDayButton;

    private TimeWarp _timeWarp;
    private DateTime _tomorrow;
    private bool _showExpenses;
    private Image _fadeToBlack;
    private bool _fading;
    private bool _startOfDay;
    
    private Transform _homePosition;
    private Transform _player;

    private void Start()
    {
        _timeWarp = GetComponent<TimeWarp>();
        _homePosition= GameObject.FindGameObjectWithTag(Tags.Home).GetComponent<Transform>();
        _player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Transform>();
        _fadeToBlack = GameObject.FindGameObjectWithTag(Tags.FadeToBlack).GetComponent<Image>();
        nextDayButton.onClick.AddListener(() =>
        {
            expensesPanel.SetActive(false);
            _fading = true;
            StartCoroutine(nameof(FadeIn));
            GameState.Instance.Days.Value++;
        });
    }

     void Update()
    {
        if (GameState.Instance.Time.Value.Hour >= endOfDayHour)
        {   
            // Start warping
            _tomorrow = Time.FirstDay.AddDays(GameState.Instance.Days.Value + 1);
            _timeWarp.SkipUntil(_tomorrow);
            
            if (!_fading)
            {
                _fading = true;
                endOfDayMessage.enabled = true;
                StartCoroutine(nameof(FadeOut));
            }
        
            _startOfDay = false;
        }
        
        // if (!_startOfDay && GameState.Instance.Time.Value.Hour == Time.FirstDay.Hour)
        // {
        //     _startOfDay = true;
        //     expensesPanel.SetActive(false);
        //     _fading = true;
        //     StartCoroutine(nameof(FadeIn));
        // }


    }
    
     IEnumerator FadeIn()
    {
        for (int i = 0; i < 100; i++)
        {
            _fadeToBlack.color = new Color(_fadeToBlack.color.r, _fadeToBlack.color.g, _fadeToBlack.color.b, 1 - i/100f);
            yield return new WaitForSeconds(0.04f);
        }

        _fading = false;
    }
    
    IEnumerator FadeOut()
    {
        for (int i = 1; i < 100; i++)
        {
            _fadeToBlack.color = new Color(_fadeToBlack.color.r, _fadeToBlack.color.g, _fadeToBlack.color.b, (float)i/100f);
            yield return new WaitForSeconds(0.04f);
        }
        endOfDayMessage.enabled = false;
        
        _player.position = _homePosition.position;
        
        // Show expenses window while warping
        expensesContent.text = string.Join("\n\n", Expenses.Expenses.GetExpenses()
            .Select(x => string.Join('\n', $"{x.Title} = ${x.Cost:n0}", x.Description)));
        expensesPanel.SetActive(true);
        
        _fading = false;
    }

    private void OnGUI()
    {
        if (_showExpenses)
        {
        }
    }
}