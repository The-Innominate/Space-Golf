using System.Collections.Generic;
using UnityEngine;

public class GravityArea : MonoBehaviour
{
    [SerializeField] private List<Rigidbody2D> bodies;
    [SerializeField] private float Strength;

    void Update()
    {
        foreach (var body in bodies)
        {
			body.AddForce((transform.position - body.transform.position) * Strength * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.attachedRigidbody)
		{
			bodies.Add(collision.attachedRigidbody);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (bodies.Contains(collision.attachedRigidbody))
		{
			bodies.Remove(collision.attachedRigidbody);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.rigidbody)
		{

		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (bodies.Contains(collision.rigidbody))
		{

		}
	}
}
