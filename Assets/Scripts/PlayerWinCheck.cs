using UnityEditor;
using UnityEngine;

public class PlayerWinCheck : MonoBehaviour
{
    //this will be UI elements that need to be disabled or enabled
    //public GameObject pauseMenuOverlay;
    public string nextLevelToLoad;
    public AudioClip WinFX;
    private AudioSource winFXSource;

    [SerializeField] private string CourseName;

	private void Start()
	{
        winFXSource = gameObject.AddComponent<AudioSource>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.tag);
        Debug.Log(GameObject.Find("GameEndMenu").name);
        if (collision.gameObject.CompareTag("Winnable"))
        {
            //check if a canvas with the name "GameEndMenu" exists
            if (GameObject.Find("GameEndMenu") != null)
            {
                winFXSource.PlayOneShot(WinFX);

                //get the player win panel (child of canvas), and display it
                //pauseMenuOverlay.SetActive(false);
                GameObject.Find("GameEndMenu").transform.GetChild(1).gameObject.SetActive(true);
                GameObject.Find("OverlayUI").gameObject.SetActive(false);

				//get the level name from the scene name
				string levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
				//get the score from the player script
				int score = GetComponent<PlayerScript>().getStrokes();
				//save the score to the json file
                LevelHighScores.Instance.SaveToJson(CourseName, levelName, score);

				//sets the timescale to 0, so the game stops
				Time.timeScale = 0;
            }
        }

        if (collision.gameObject.CompareTag("PlayerKiller"))
        {
            //check if a canvas with the name "GameEndMenu" exists
            if (GameObject.Find("GameEndMenu") != null)
            {
                //get the player lose panel (child of canvas), and display it
                //pauseMenuOverlay.SetActive(false);
                GameObject.Find("GameEndMenu").transform.GetChild(0).gameObject.SetActive(true);

                Time.timeScale = 0;
            }
        }
    }

    ////Button click events
    //public void OnRestartButtonClicked()
    //{
    //    //reload the current scene
    //    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    //    Time.timeScale = 1;
    //}

    //public void OnMainMenuButtonClicked()
    //{
    //    //load the main menu scene
    //    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    //}

    //public void OnNextLevelButtonClicked()
    //{
    //    Time.timeScale = 1;
    //    //load the next level scene
    //    UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelToLoad);
    //}
}
