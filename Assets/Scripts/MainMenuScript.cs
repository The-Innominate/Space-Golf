using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public GameObject levelButtonContainer;

    private Transform cameraTransform;
    private Transform cameraDesiredLookAt;


    // Start is called before the first frame update, it's a default Unity method
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        /* The 'Resources.LoadAll' method is used to load all assets in a folder or file at the specified path.

            If you don't have a 'Resources' folder created you won't be able to load the assets this way.
        */
        Sprite[] thumbnails = Resources.LoadAll<Sprite>("LevelSprites");

        foreach (Sprite thumbnail in thumbnails)
        {
            GameObject container = Instantiate(levelButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = thumbnail;
            //make sure to set a 'transform' as a parent, since setting just a game object will not work
            container.transform.SetParent(levelButtonContainer.transform, false);

            //just parsing the name to a string
            string sceneName = thumbnail.name;

            container.GetComponent<Button>().onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName));
        }

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
}
