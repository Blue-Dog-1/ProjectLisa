#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PCControl : MonoBehaviour
{
    [SerializeField] [Range (0f, 0.2f)]
    float velocity;
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * velocity);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(-Vector3.forward * velocity * 2f);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * velocity);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * velocity);
        
    }
}
#endif
