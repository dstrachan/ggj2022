using Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button NewGameButton;
    public Button ContinueButton;
    public Button ExitButton;

    private void Start()
    {
        NewGameButton.onClick.AddListener(NewGame);
        ContinueButton.onClick.AddListener(Continue);
        ExitButton.onClick.AddListener(Exit);

        ContinueButton.enabled = GameState.HasSaveGame();
    }

    private void Update()
    {
        if (Input.GetButtonDown("JoyJump"))
        {
            Continue();
        }
    }

    private static void NewGame()
    {
        GameState.Instance.Reset();
        Continue();
    }

    private static void Continue()
    {
        SceneManager.LoadSceneAsync("Scenes/MainCityScene");
        SceneManager.UnloadSceneAsync("Scenes/MainMenu");
    }

    private static void Exit()
    {
        Application.Quit();
    }
}