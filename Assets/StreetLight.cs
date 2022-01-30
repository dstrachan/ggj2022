using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class StreetLight : MonoBehaviour
{
    private Light _light;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.Instance.Time.Value.Hour is >= 17 or < 7)
        {
            _light.enabled = true;
        }
        else
        {
            _light.enabled = false;
        }
    }
}
