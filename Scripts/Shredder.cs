using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    static public int QuantityLostObjects { get; set; }
    private void OnTriggerEnter(Collider other)
    {
        QuantityLostObjects++;
        Destroy(other.gameObject);
    }
}
