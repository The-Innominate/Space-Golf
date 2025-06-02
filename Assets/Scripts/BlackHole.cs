using UnityEngine;

public class BlackHole : MonoBehaviour
{

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.tag.Equals("Player"))
		{
			var Player = collision.gameObject.GetComponent<PlayerScript>();
			if (Player != null)
			{
				Player.resetShot();
			}
		}
	}
}
