using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeform : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    public void _start(Transform a)
    {
        StartCoroutine(loop(a));
    }
    IEnumerator loop(Transform attractor)
    {
        float timer = 1.1f;
        Material materail = GetComponent<MeshRenderer>().material;
        materail.SetFloat("_Start", 0f);
        materail.SetFloat("_size", transform.localScale.x);
        rb.isKinematic = true;
        while (timer > 0f)
        {
            yield return new WaitForSeconds(0.05f);

            timer -= 0.1f;
            materail.SetFloat("_velocity", timer);
            materail.SetVector("_point", attractor.position);
        }
        Destroy(gameObject);
    }
}

