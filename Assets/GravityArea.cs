using System.Collections.Generic;
using UnityEngine;

public class GravityArea : MonoBehaviour
{
	/// <summary>
	/// bodies that gravity will be applied to
	/// </summary>
    [SerializeField] private List<Rigidbody2D> bodies;
	/// <summary>
	/// strength of gravity applied to bodies
	/// </summary>
    [SerializeField] private float strength = 1;
	/// <summary>
	/// area that detects for bodies to apply gravity to
	/// </summary>
	[SerializeField] private CircleCollider2D areaOfEffect;
	/// <summary>
	/// percentage of gravity that should be applied when at the edge of the area of effect
	/// 
	/// as bodies approach the edge, gravity will equal: strength * gravityFalloff
	/// </summary>
	[SerializeField, Range(0.0f,1.0f)] private float gravityFalloff = 0.25f;

    void Update()
    {
        foreach (var body in bodies)
        {
			Vector2 direction = transform.position - body.transform.position;
			float distance = Vector3.Distance(transform.position, body.transform.position);
			float gravStrength = Mathf.Lerp(strength, strength * gravityFalloff, Mathf.Sqrt(distance / (areaOfEffect.radius * transform.lossyScale.x)));

			body.AddForce(direction * gravStrength * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

	private void Reset()
	{
		areaOfEffect = GetComponent<CircleCollider2D>();
		strength = 1;
		gravityFalloff = 0.25f;
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

	private void OnDrawGizmos()
	{
		// Area of effect of gravity
		// also fully fallen off gravity
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, areaOfEffect.radius * transform.lossyScale.x);
	}

	private void OnDrawGizmosSelected()
	{
		// 66% gravity fall off
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, Mathf.Lerp(gravityFalloff * areaOfEffect.radius * transform.lossyScale.x, areaOfEffect.radius * transform.lossyScale.x, 0.66f));

		// 33% gravity fall off
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, Mathf.Lerp(gravityFalloff * areaOfEffect.radius * transform.lossyScale.x, areaOfEffect.radius * transform.lossyScale.x, 0.33f));

		// full strength gravity
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(transform.position, gravityFalloff * areaOfEffect.radius * transform.lossyScale.x);
	}
}
