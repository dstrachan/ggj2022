using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button NewGameButton;
    public Button ContinueButton;
    public Button ExitButton;

    // Start is called before the first frame update
    void Start()
    {
        NewGameButton.onClick.AddListener(NewGame);   
        ContinueButton.onClick.AddListener(Continue);
        ExitButton.onClick.AddListener(Exit);

        ContinueButton.enabled = GameState.HasSaveGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("JoyJump"))
        {
            Continue();
        }
    }

    private void NewGame()
    {
        throw new Exception("TODO reset state then");
        Continue();
    }

    private void Continue()
    {
        SceneManager.LoadScene("Scenes/MainCityScene");
    }

    private void Exit()
    {
        Application.Quit();
    }
}
