using UnityEngine;

public class TheSun : MonoBehaviour
{
	[SerializeField]
	private float Power = 1.0f;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Rigidbody2D Body = collision.gameObject.GetComponent<Rigidbody2D>();
		if (Body != null)
		{
			Vector2 Push = (collision.GetContact(0).normal * -Power);
			Body.AddForce(Push);
		}
	}
}
