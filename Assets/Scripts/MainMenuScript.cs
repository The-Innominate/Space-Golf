using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public GameObject levelButtonContainer;
    public GameObject shopButtonPrefab;
    public GameObject shopButtonContainer;

    public GameObject golfBallPrefab;

    private Transform cameraTransform;
    private Transform cameraDesiredLookAt;


    // Start is called before the first frame update, it's a default Unity method
    private void Start()
    {
        // reset the width of the scrolling level selection
        levelButtonContainer.GetComponent<RectTransform>().offsetMin = new Vector2(0, levelButtonContainer.GetComponent<RectTransform>().offsetMin.y);

		cameraTransform = Camera.main.transform;
        /* The 'Resources.LoadAll' method is used to load all assets in a folder or file at the specified path.

            If you don't have a 'Resources' folder created you won't be able to load the assets this way.
        */
        Sprite[] thumbnails = Resources.LoadAll<Sprite>("UpdatedLevelSprites");

        foreach (Sprite thumbnail in thumbnails)
        {
            GameObject container = Instantiate(levelButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = thumbnail;
            //make sure to set a 'transform' as a parent, since setting just a game object will not work
            container.transform.SetParent(levelButtonContainer.transform, false);

            //just parsing the name to a string
            string sceneName = thumbnail.name;

            // this adds increases the width of the scrolling container by the width of the container objects
            // The width is 104 by guess and check because I don't know how to programatically do this sorry
            levelButtonContainer.GetComponent<RectTransform>().offsetMin += Vector2.left * 104;

            container.GetComponent<Button>().onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName));

            // this is the score for the level
            int HighScore = LevelHighScores.Instance.LoadFromJson(sceneName);
            if (HighScore <= 0)
            {
                container.GetComponentInChildren<TextMeshProUGUI>().text = "No Score";
            }
            else
            {
                container.GetComponentInChildren<TextMeshProUGUI>().text = "Lowest Strokes: " + HighScore.ToString();
            }
        }

        Sprite[] textures = Resources.LoadAll<Sprite>("GolfBall");
        foreach (Sprite texture in textures)
        {
            GameObject shopContainer = Instantiate(shopButtonPrefab) as GameObject;
            shopContainer.GetComponent<Image>().sprite = texture;
            shopContainer.transform.SetParent(shopButtonContainer.transform, false);
            shopContainer.SetActive(true); // Ensure it's active
            shopContainer.GetComponent<Button>().interactable = true; // Make sure it's interactable

            string sceneName = texture.name;
            //The guess and check this time is 39
            shopButtonContainer.GetComponent<RectTransform>().offsetMin += Vector2.left * 39;

            // FIX: capture the current texture.name by assigning to a local variable
            string selectedName = texture.name;

            shopContainer.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerPrefs.SetString("SelectedBallSkin", selectedName);
                PlayerPrefs.Save();
                Debug.Log("Selected ball skin: " + selectedName);
            });
        }

            // sets the scrolling level selection to be at the beginning (there is very likely a better way to do this)
            levelButtonContainer.GetComponent<RectTransform>().localPosition = new Vector2(-levelButtonContainer.GetComponent<RectTransform>().localPosition.x, 0);
            shopButtonContainer.GetComponent<RectTransform>().localPosition = new Vector2(-shopButtonContainer.GetComponent<RectTransform>().localPosition.x, 0);

        //reinstantiate the timescale to 1
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(cameraDesiredLookAt != null)
        {
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraDesiredLookAt.rotation, 3 * Time.deltaTime);
        }
    }

    private void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LookAtMenu(Transform menuTransform)
    {
        cameraDesiredLookAt = menuTransform;
    }

    private void ChangePlayerSkin()
    {
        // This will be the implimentation:

        golfBallPrefab = Resources.Load<GameObject>("GolfBall/" + golfBallPrefab.name);
    }
}
