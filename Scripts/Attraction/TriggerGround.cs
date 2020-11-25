using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGround : MonoBehaviour
{
    public float secont;
    public void OnTriggerEnter(Collider collider)
    {
        collider.enabled = false;
        collider.gameObject.GetComponent<MeshDeformGround>()._start(gameObject.transform, secont);
    }
}
