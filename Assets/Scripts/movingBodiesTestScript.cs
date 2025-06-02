using UnityEngine;

public class movingBodiesTestScript : MonoBehaviour
{
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f)), ForceMode2D.Impulse);
        }
    }
}
