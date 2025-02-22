using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Collider2D))]
public class PlayerScript : MonoBehaviour
{
	bool Dragging = false;
	bool MouseDragging = false;
	Vector3 dragStartPoint = Vector3.zero;
	Vector3 dragEndPoint = Vector3.zero;
	Vector3 LastShotPosition = Vector3.zero;

	[SerializeField]
	private float power;
	[SerializeField]
	private float minimumSpeed;
	[SerializeField]
	private Collider2D CamConfiner;

	Rigidbody2D rb;
	LineRenderer lr;
	Collider2D col;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		lr = GetComponent<LineRenderer>();
		col = rb.GetComponent<Collider2D>();
		lr.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (rb.linearVelocity.magnitude < minimumSpeed)
		{
			rb.linearVelocity = Vector3.zero;

			TouchUpdate();
			MouseUpdate();
		}

		if (CamConfiner && (!CamConfiner.IsTouching(col))) 
		{
			resetShot();
		}
	}

	private void TouchUpdate()
	{
		if (Input.touchCount > 0 && !Dragging)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				startDragging(touch);
			}
			else if (touch.phase == TouchPhase.Ended && Dragging)
			{
				endDragging(touch);
			}
		}

		if (Dragging)
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				Vector3[] linePoints = { transform.position, transform.position + (dragStartPoint - Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0))) };
				lr.SetPositions(linePoints);
			}

		}
	}

	private void MouseUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			startMouseDragging();
		}

		if (Input.GetMouseButtonUp(0))
		{
			endMouseDragging();
		}

		if (MouseDragging)
		{
			Vector3[] linePoints = { transform.position, transform.position + (dragStartPoint - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0))) };
			lr.SetPositions(linePoints);
		}
	}

	private void startDragging(Touch touch)
	{
		Dragging = true;
		print("Drag");

		lr.enabled = true;
		dragStartPoint = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
	}

	private void endDragging(Touch touch)
	{
		dragEndPoint = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));

		LastShotPosition = transform.position;

		Vector2 forceDirection = dragStartPoint - dragEndPoint;
		rb.AddForce(forceDirection * power, ForceMode2D.Impulse);

		lr.enabled = false;
		Dragging = false;
	}

	private void startMouseDragging()
	{
		dragStartPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		print("Start");
		lr.enabled = true;

		MouseDragging = true;
	}

	private void endMouseDragging()
	{
		dragEndPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

		LastShotPosition = transform.position;

		Vector2 forceDirection = dragStartPoint - dragEndPoint;
		rb.AddForce(forceDirection * power, ForceMode2D.Impulse);

		print("Adding force: " + forceDirection);

		lr.enabled = false;

		MouseDragging = false;
	}

	public void resetShot()
	{
		transform.position = LastShotPosition;
		rb.linearVelocity = Vector2.zero;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.tag.Equals("Planet"))
		{
			rb.linearDamping = 2;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.tag.Equals("Planet"))
		{
			rb.linearDamping = 0.1f;
		}
	}
}
