using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using TMPro;
using UnityEngine;

public class DaySetter : MonoBehaviour
{
    public TextMeshProUGUI dayValue;

    private void OnGUI()
    {
        dayValue.text = $"Day {GameState.Instance.Days + 1:n0}\n{GameState.Instance.Time:d}";
    }
}