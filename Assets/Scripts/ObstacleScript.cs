using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
	[SerializeField] float magnitude;
	[SerializeField] float angle;
    Vector2 startingVelocity { get { return Quaternion.Euler(0,0,angle) * Vector2.right * magnitude; } set { ; } }
    [SerializeField] Rigidbody2D rb;

	void Start()
    {
        rb.linearVelocity = startingVelocity;
    }

	private void Reset()
	{
		rb = GetComponent<Rigidbody2D>();	
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawLine(transform.position, transform.position + (Vector3)startingVelocity);
	}
}
