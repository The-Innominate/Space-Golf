using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class ShootingStar : MonoBehaviour
{
    Animator anim;

    public float minTime;
    public float maxTime;
    private float setTime;
    private float Timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer > setTime)
        {
            StartCoroutine(PlayAnim());
        }
		
	}

    private IEnumerator PlayAnim()
    {
		anim.SetBool("Time", true);

		setTime = Random.Range(minTime, maxTime);
		Timer = 0;

        yield return new WaitForSeconds(1);

		anim.SetBool("Time", false);
	}
}
