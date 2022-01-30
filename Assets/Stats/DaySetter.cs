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
        dayValue.text = $"{GameState.Instance.Days.Value + 1:n0}";
    }
}