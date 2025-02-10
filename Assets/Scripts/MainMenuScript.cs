using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public GameObject levelButtonContainer;

// Start is called before the first frame update, it's a default Unity method
    private void Start()
    {
        /* The 'Resources.LoadAll' method is used to load all assets in a folder or file at the specified path.

            If you don't have a 'Resources' folder created you won't be able to load the assets this way.
        */
        Sprite[] thumbnails = Resources.LoadAll<Sprite>("Levels");

        foreach (Sprite thumbnail in thumbnails)
        {
            GameObject container = Instantiate(levelButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = thumbnail;
            //make sure to set a 'transform' as a parent, since setting just a game object will not work
            container.transform.SetParent(levelButtonContainer.transform, false);

            //just parsing the name to a string
            string sceneName = thumbnail.name;
            container.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneName));
        }
    }

    private void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
