using Model;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    private GameState GameState { get; set; }

    private void Awake()
    {
        GameState = GameState.Load();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(GameState);
            GameState.Strength.Xp++;
            GameState.Save();
        }
    }
}
