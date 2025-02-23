using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
	[SerializeField] float magnitude;
	[SerializeField] float angle;
    Vector2 startingVelocity { get { return Quaternion.Euler(0,0,angle) * Vector2.right * magnitude; } set { ; } }
    [SerializeField] Rigidbody2D rb;

	[SerializeField] List<Tuple<Vector3, float>> forces = new List<Tuple<Vector3, float>>();

	void Start()
    {
        rb.linearVelocity = startingVelocity;
    }

	private void Reset()
	{
		rb = GetComponent<Rigidbody2D>();	
	}

	private void LateUpdate()
	{
		if (forces.Count == 0)
		{
			return;
		}
		else if (forces.Count == 1)
		{
			rb.AddForce(forces[0].Item1 * Time.deltaTime, ForceMode2D.Impulse);
		}
		else
		{
			Vector3 usedforce = Vector3.zero;
			float distance = float.MaxValue;

			foreach (var force in forces)
			{
				if (force.Item2 < distance)
				{
					usedforce = force.Item1;
					distance = force.Item2;
				}
			}

			rb.AddForce(usedforce * Time.deltaTime, ForceMode2D.Impulse);
		}
		forces.Clear();
	}

	public void TryApplyGravity(Vector3 gravitypull, float distance)
	{
		forces.Add(new Tuple<Vector3, float>(gravitypull, distance));
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawLine(transform.position, transform.position + (Vector3)startingVelocity);
	}
}
