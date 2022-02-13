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
        ContinueButton.interactable = GameState.HasSaveGame();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            Continue();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "MainCityScene")
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void NewGame()
    {
        GameState.Reset();
        LoadCity();
    }

    private static void LoadCity()
    {
        SceneManager.LoadSceneAsync("Scenes/MainCityScene");
        SceneManager.UnloadSceneAsync("Scenes/MainMenu");
    }

    private void Continue()
    {
        if (SceneManager.GetActiveScene().name == "MainCityScene")
        {
            gameObject.SetActive(false);
        }
        else
        {
            LoadCity();
        }
    
    }

    private static void Exit()
    {
        Application.Quit();
    }
}