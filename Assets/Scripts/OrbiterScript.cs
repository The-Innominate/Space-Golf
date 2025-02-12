using UnityEngine;

public class OrbiterScript : MonoBehaviour
{
    [SerializeField] Transform whoIOrbit;
	[SerializeField] bool displayGizmo;

    [SerializeField] Vector2 offset;
    [SerializeField] Vector2 scale = Vector2.one;
    [SerializeField] float rotation = 0.0f;
	[SerializeField] float distance = 5.0f;
    [SerializeField] float startRotation = 0.0f;
	[SerializeField] float speed = 10.0f;

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

	private void OnDrawGizmos()
	{
		// this code can very likely be optimized

		// The corners of this polygon represent the extents of the circular orbit

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

			Gizmos.DrawLine(whoIOrbit.position + (Vector3)offset + rotOffset, whoIOrbit.position + (Vector3)offset + rotOffset1);
			Gizmos.DrawLine(whoIOrbit.position + (Vector3)offset + rotOffset1, whoIOrbit.position + (Vector3)offset + rotOffset2);
			Gizmos.DrawLine(whoIOrbit.position + (Vector3)offset + rotOffset2, whoIOrbit.position + (Vector3)offset + rotOffset3);
			Gizmos.DrawLine(whoIOrbit.position + (Vector3)offset + rotOffset3, whoIOrbit.position + (Vector3)offset + rotOffset);
		}
	}
}
