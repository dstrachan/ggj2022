using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class GameStateGetter : MonoBehaviour
{
    

    public void SetTime(float factorValue)
    {
        GameState.Instance.TimeSpeed = factorValue;
    }
    
    void Update()
    {
    }
}
