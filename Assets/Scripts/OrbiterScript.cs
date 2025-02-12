using UnityEngine;

public class OrbiterScript : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rb;
    [SerializeField] Transform whoIOrbit;
	[SerializeField] bool displayGizmo;

	// orbit offset from whoIOrbit
    [SerializeField] Vector2 offset;
	// orbit scale
    [SerializeField] Vector2 scale = Vector2.one;
	// orbit rotation
    [SerializeField] float rotation = 0.0f;
	// distance away from whoIOrbit the orbit should be
	[SerializeField] float distance = 5.0f;
	// where the orbiter should start about the orbit
    [SerializeField] float startRotation = 0.0f;
	// speed of orbit
	[SerializeField] float speed = 10.0f;

	// current angle about the orbit
	float angle = 0.0f;

	private void Start()
	{
		angle = startRotation;
	}

	void Update()
    {
		angle += Time.deltaTime * speed;

        Vector3 rotOffset = (Quaternion.Euler(0, 0, angle) * Vector3.right * distance);
		rotOffset.Scale((Vector3)scale);
		rotOffset = Quaternion.Euler(0, 0, rotation) * rotOffset;

		transform.position = whoIOrbit.position + (Vector3)offset + rotOffset;
    }

	private void Reset()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void OnDrawGizmos()
	{
		// this code can very likely be optimized

		// The corners of this polygon represent the extents of the circular orbit

		// then places a wiresphere with a radius of 0.5 at the startRotation about the orbit

		if (whoIOrbit != null && displayGizmo)
        {
            Gizmos.color = Color.red;

			Vector3 rotOffset = (Vector3.right * distance);
			Vector3 rotOffset1 = (Quaternion.Euler(0, 0, 90) * rotOffset);
			Vector3 rotOffset2 = (Quaternion.Euler(0, 0, 180) * rotOffset);
			Vector3 rotOffset3 = (Quaternion.Euler(0, 0, 270) * rotOffset);

			var scal = ((Vector3)scale);
			rotOffset.Scale(scal);
			rotOffset1.Scale(scal);
			rotOffset2.Scale(scal);
			rotOffset3.Scale(scal);

			rotOffset = Quaternion.Euler(0, 0, rotation) * rotOffset;
			rotOffset1 = Quaternion.Euler(0, 0, rotation) * rotOffset1;
			rotOffset2 = Quaternion.Euler(0, 0, rotation) * rotOffset2;
			rotOffset3 = Quaternion.Euler(0, 0, rotation) * rotOffset3;

			Vector3 startrotoffset = Quaternion.Euler(0, 0, startRotation + rotation) * rotOffset;

			Gizmos.DrawLine(whoIOrbit.position + (Vector3)offset + rotOffset, whoIOrbit.position + (Vector3)offset + rotOffset1);
			Gizmos.DrawLine(whoIOrbit.position + (Vector3)offset + rotOffset1, whoIOrbit.position + (Vector3)offset + rotOffset2);
			Gizmos.DrawLine(whoIOrbit.position + (Vector3)offset + rotOffset2, whoIOrbit.position + (Vector3)offset + rotOffset3);
			Gizmos.DrawLine(whoIOrbit.position + (Vector3)offset + rotOffset3, whoIOrbit.position + (Vector3)offset + rotOffset);

			Gizmos.DrawWireSphere(startrotoffset, 0.5f);
		}
	}
}
