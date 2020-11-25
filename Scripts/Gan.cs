using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gan : MonoBehaviour
{
    public GameObject[] Shells;
    public float force;
    public float velocity = 1f;
    public Vector2 DelayRange; // x min, y max

    void Start()
    {
        StartCoroutine(loop());
    }


    IEnumerator loop()
    {
        while(true)
        {
            transform.Translate(Vector3.right * velocity);
            velocity = -velocity;
            yield return new WaitForSeconds(Random.Range(DelayRange.x, DelayRange.y));

            transform.LookAt(Attraction.Playar);

            var shell = Instantiate(Shells[0], transform.position + Vector3.forward, Quaternion.identity);

            var rb = shell.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * force, ForceMode.Impulse);
        }
    }
}
