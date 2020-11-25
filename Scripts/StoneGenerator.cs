using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGenerator : MonoBehaviour
{
    public GameObject original;

    public void generait()
    {
        var i = Instantiate(original, transform.position, Quaternion.identity);
        i.SetActive(true);
    }
    
}
