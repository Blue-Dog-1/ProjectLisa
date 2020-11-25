using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformGround : MonoBehaviour
{
    float secont;
    public void _start(Transform a, float secont)
    {
        this.secont = secont;
        StartCoroutine(loop(a));
    }
    IEnumerator loop(Transform attractor)
    {
        float timer = secont;
        Material materail = GetComponent<MeshRenderer>().material;
        materail.SetFloat("_Start", 0f);
        materail.SetFloat("_size", transform.localScale.x);
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
