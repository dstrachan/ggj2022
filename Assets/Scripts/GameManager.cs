using System;
using System.Collections;
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
    public Transform expensesContent;
    public TextMeshProUGUI expenseItemPrefab;
    public TextMeshProUGUI expensesTotal;
    public Scrollbar expenseScrollbar;
    public TextMeshProUGUI endOfDayMessage;
    public Button nextDayButton;

    private TimeWarp _timeWarp;
    private DateTime _tomorrow;
    private bool _showExpenses;
    private Image _fadeToBlack;
    private bool _fading;

    private Transform _homePosition;
    private Transform _player;

    private long _pendingExpenses;
    private bool _isShowing;

    private void Start()
    {
        _timeWarp = GetComponent<TimeWarp>();
        _homePosition = GameObject.FindGameObjectWithTag(Tags.Home).GetComponent<Transform>();
        _player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Transform>();
        _fadeToBlack = GameObject.FindGameObjectWithTag(Tags.FadeToBlack).GetComponent<Image>();
        nextDayButton.onClick.AddListener(AdvanceDay);
        expensesTotal.text = string.Empty;
    }

    private void AdvanceDay()
    {
        HideExpenses();
        _fading = true;
        StartCoroutine(nameof(FadeIn));
        GameState.Instance.Days.Value++;
    }

    private void Update()
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
        }

        if (nextDayButton.isActiveAndEnabled && Input.GetButtonDown("JoyJump"))
        {
            AdvanceDay();
        }
    }

    private IEnumerator FadeIn()
    {
        for (var i = 0; i < 100; i++)
        {
            _fadeToBlack.color = new Color(_fadeToBlack.color.r, _fadeToBlack.color.g, _fadeToBlack.color.b,
                1 - i / 100f);
            yield return new WaitForSeconds(0.04f);
        }

        _fading = false;
    }

    private IEnumerator FadeOut()
    {
        for (var i = 1; i < 100; i++)
        {
            _fadeToBlack.color = new Color(_fadeToBlack.color.r, _fadeToBlack.color.g, _fadeToBlack.color.b, i / 100f);
            yield return new WaitForSeconds(0.04f);
        }

        endOfDayMessage.enabled = false;

        _player.position = _homePosition.position;

        yield return ShowExpenses();

        _fading = false;
    }

    private IEnumerator ShowExpenses()
    {
        _isShowing = true;
        expensesPanel.SetActive(true);

        var expenses = Expenses.Expenses.GetExpenses();
        foreach (var (cost, title, description) in expenses)
        {
            yield return new WaitForSeconds(0.5f);
            var obj = Instantiate(expenseItemPrefab, expensesContent);
            obj.text = $"<b>{title}</b> <color=red>-${cost}</color>\n<size=16>{description}</size>";
        }

        yield return new WaitForSeconds(1);

        var money = GameState.Instance.Money;
        _pendingExpenses = expenses.Sum(x => x.Cost);
        var remaining = money - _pendingExpenses;
        var color = remaining > 0 ? "green" : "red";
        expensesTotal.text = @$"<color=green>${GameState.Instance.Money:n0}</color>
<color=red>-${expenses.Sum(x => x.Cost):n0}</color>
= <color={color}>${remaining}</color>";

        yield return new WaitForSeconds(1);

        nextDayButton.gameObject.SetActive(true);
        _isShowing = false;
    }

    private void HideExpenses()
    {
        foreach (Transform childTransform in expensesContent.GetComponentInChildren<Transform>())
        {
            Destroy(childTransform.gameObject);
        }

        expensesPanel.SetActive(false);
        expensesTotal.text = string.Empty;
        nextDayButton.gameObject.SetActive(false);

        GameState.Instance.Money -= _pendingExpenses;
        _pendingExpenses = 0;
    }

    private void OnGUI()
    {
        if (_isShowing)
        {
            expenseScrollbar.value = 0;
        }
    }
}