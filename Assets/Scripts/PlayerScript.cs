using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerScript : MonoBehaviour
{
	bool Dragging = false;
	bool MouseDragging = false;
	Vector3 dragStartPoint = Vector3.zero;
	Vector3 dragEndPoint = Vector3.zero;
	Vector3 LastShotPosition = Vector3.zero;

	int strokes = 0;

	[SerializeField]
	private float power;
	[SerializeField]
	private float minimumSpeed;
	[SerializeField]
	private Collider2D CamConfiner;
	[SerializeField]
	private AudioClip hitFX;
	[SerializeField]
	private ArrowGenerator arrow;
	[SerializeField]
	private TrailRenderer trailRenderer;

	Rigidbody2D rb;
	Collider2D col;
	AudioSource sound;
	private SpriteRenderer sr;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		col = rb.GetComponent<Collider2D>();
		sound = rb.GetComponent<AudioSource>();
		sr = GetComponent<SpriteRenderer>();
		LastShotPosition = transform.position;

		// This controls the player skin prefs and cuts off the last 3 chars of the color name: 'GolfRed_01' -> 'GolfRed'
		string selectedSkin = PlayerPrefs.GetString("SelectedBallSkin", "golf_0"); // "default" fallback
		Sprite skinSprite = null;

		if (selectedSkin.Length > 3)
		{
			string baseName = selectedSkin.Substring(0, selectedSkin.Length - 2);
			skinSprite = Resources.Load<Sprite>("GolfBall/" + baseName);
		}
		else
		{
			Debug.LogWarning("Selected skin name is too short to trim: " + selectedSkin);
		}

		if (skinSprite != null)
		{
			sr.sprite = skinSprite;
		}
		else
		{
			Debug.LogWarning("Could not load sprite for: " + selectedSkin);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (rb.linearVelocity.magnitude < minimumSpeed)
		{
			rb.linearVelocity = Vector3.zero;
			rb.angularVelocity = 0;

			TouchUpdate();
			MouseUpdate();
		}

		if (CamConfiner && (!CamConfiner.IsTouching(col)))
		{
			resetShot();
		}

		arrowUpdate();
	}

	private void TouchUpdate()
	{
		if (Input.touchCount > 0 && !Dragging)
		{
			Touch touch = Input.GetTouch(0);

			if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
				return;

			if (touch.phase == TouchPhase.Began || (touch.phase == TouchPhase.Moved && !Dragging))
			{
				startDragging(touch);
			}
			else if (touch.phase == TouchPhase.Ended && Dragging)
			{
				endDragging(touch);
			}
		}
	}

	private void MouseUpdate()
	{
		if(EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}

		if (Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && !Dragging))
		{
			startMouseDragging();
		}

		if (Input.GetMouseButtonUp(0))
		{
			endMouseDragging();
		}
	}

	private void startDragging(Touch touch)
	{
		Dragging = true;
		print("Drag");

		dragStartPoint = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
	}

	private void endDragging(Touch touch)
	{
		dragEndPoint = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));

		LastShotPosition = transform.position;

		Vector2 forceDirection = dragStartPoint - dragEndPoint;
		rb.AddForce(forceDirection * power, ForceMode2D.Impulse);
		strokes++;

		Dragging = false;

		sound.PlayOneShot(hitFX);
	}

	private void startMouseDragging()
	{
		dragStartPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		print("Start");

		MouseDragging = true;
	}

	private void endMouseDragging()
	{
		dragEndPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

		LastShotPosition = transform.position;

		Vector2 forceDirection = dragStartPoint - dragEndPoint;
		rb.AddForce(forceDirection * power, ForceMode2D.Impulse);
		strokes++;

		print("Adding force: " + forceDirection);

		MouseDragging = false;

		sound.PlayOneShot(hitFX);
	}

	public void resetShot()
	{
		trailRenderer.Clear();
		trailRenderer.enabled = false;
		rb.linearVelocity = Vector3.zero;
		transform.position = LastShotPosition;
		rb.linearVelocity = Vector3.zero;
		rb.angularVelocity = 0;
		trailRenderer.enabled = true;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.tag.Equals("Planet"))
		{
			rb.linearDamping = 2;
			rb.linearVelocity *= 0.5f;
			if (rb.linearVelocity.magnitude < minimumSpeed)
			{
				transform.SetParent(collision.transform, true);
			}
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.collider.tag.Equals("Planet"))
		{
			if (rb.linearVelocity.magnitude < minimumSpeed)
			{
				transform.SetParent(collision.transform, true);
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.tag.Equals("Planet"))
		{
			if (rb.linearVelocity.magnitude > minimumSpeed)
			{
				transform.parent = null;
			}
			rb.linearDamping = 0.2f;
		}
	}

	public int getStrokes()
	{
		return strokes;
	}

	private void arrowUpdate()
	{
		if (Input.touchCount > 0 && Dragging)
		{
			Touch touch = Input.GetTouch(0);

			arrow.stemLength = (Vector3.Distance(transform.position, transform.position + (dragStartPoint - Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0)))) / 5) * 4;
			arrow.stemWidth = arrow.stemLength / 30;
			arrow.tipLength = arrow.stemLength / 4;
			arrow.tipWidth = arrow.stemWidth * 2;

			Vector3 currentTouchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
			currentTouchWorldPos.z = 0;
			Vector2 direction = (Vector2)(dragStartPoint - currentTouchWorldPos);
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
		}
		else if (MouseDragging)
		{
			arrow.stemLength = (Vector3.Distance(transform.position, transform.position + (dragStartPoint - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)))) / 5) * 4;
			arrow.stemWidth = arrow.stemLength / 30;
			arrow.tipLength = arrow.stemLength / 4;
			arrow.tipWidth = arrow.stemWidth * 2;

			Vector3 currentMouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			currentMouseWorldPos.z = 0;
			Vector2 direction = (Vector2)(dragStartPoint - currentMouseWorldPos);
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
		}
		else
		{
			arrow.stemLength = 0;
			arrow.stemWidth = 0;
			arrow.tipLength = arrow.stemLength;
			arrow.tipWidth = arrow.stemWidth;

			print("Nothing");
		}
	}
}
