using System.Collections;
using System.Collections.Generic;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{
    public TextMeshProUGUI _moneyText;
    // Start is called before the first frame update
    void Start()
    {
        _moneyText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _moneyText.text = $"${GameState.Instance.Money:n0}";
    }
}
