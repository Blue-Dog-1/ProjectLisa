using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    public float Speed;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.right * (Time.deltaTime * Speed));
    }
}
