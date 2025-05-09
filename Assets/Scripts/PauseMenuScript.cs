
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public string mainMenuToLoad = "MainMenu";
    public GameObject overlayCanvas;

    public void OnPauseButtonClicked()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void OnMenuButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnQuitButtonClicked()
    {
        
    }

    public void OnResumeButtonClicked()
    {
        Resume();
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        overlayCanvas.SetActive(true);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        overlayCanvas.SetActive(false);
    }

}
