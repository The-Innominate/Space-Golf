using UnityEngine;

public class WinButtonStuff : MonoBehaviour
{
   public void GoToNextLevel()
	{
		Time.timeScale = 1;
		int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
		UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex + 1);
	}

	public void GoToMainMenu()
	{
		Time.timeScale = 1;
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
}
