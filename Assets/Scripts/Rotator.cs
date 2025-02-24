using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 axis = Vector3.forward;
    [SerializeField] private float speed = 1.0f;

    void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime);
    }
}
