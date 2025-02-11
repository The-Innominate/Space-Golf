using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] public Vector2 startingVelocity;
    [SerializeField] Rigidbody2D rb;

	void Start()
    {
        rb.linearVelocity = startingVelocity;
    }

	private void Reset()
	{
		rb = GetComponent<Rigidbody2D>();	
	}
}
