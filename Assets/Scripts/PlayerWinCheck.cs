using UnityEngine;

public class PlayerWinCheck : MonoBehaviour
{
    //this will be UI elements that need to be disabled or enabled
    public GameObject pauseMenuOverlay;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.tag);
        Debug.Log(GameObject.Find("GameEndMenu").name);
        if (collision.gameObject.CompareTag("Winnable"))
        {
            //check if a canvas with the name "GameEndMenu" exists
            if (GameObject.Find("GameEndMenu") != null)
            {
                //get the player win panel (child of canvas), and display it
                pauseMenuOverlay.SetActive(false);
                GameObject.Find("GameEndMenu").transform.GetChild(1).gameObject.SetActive(true);

            }
        }

        if (collision.gameObject.CompareTag("PlayerKiller"))
        {
            //check if a canvas with the name "GameEndMenu" exists
            if (GameObject.Find("GameEndMenu") != null)
            {
                //get the player lose panel (child of canvas), and display it
                pauseMenuOverlay.SetActive(false);
                GameObject.Find("GameEndMenu").transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
