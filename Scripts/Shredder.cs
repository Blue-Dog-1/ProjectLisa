using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    static public int QuantityObjects { get; set; }
    private void OnTriggerEnter(Collider other)
    {
        QuantityObjects++;
        Destroy(other.gameObject);
    }
}
