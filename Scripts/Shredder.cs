using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    private void Awake()
    {
        Events.QuantityObjects = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        Events.QuantityObjects++;
        Destroy(other.gameObject);
    }
}
