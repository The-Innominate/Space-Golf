using UnityEngine;

public class PlayerWinCheck : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Winnable"))
        {
            //check if a canvas with the name "GameEndMenu" exists
            if (GameObject.Find("GameEndMenu") != null)
            {
                //get the player win panel (child of canvas), and display it
                GameObject.Find("GameEndMenu").transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
