using UnityEngine;

public class PlayerWinCheck : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Winnable"))
        {
            Debug.Log("You can win!");
        }
    }
}
