using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCenterMass : MonoBehaviour
{
    

    void Start()
    {
        transform.parent.gameObject.GetComponent<Rigidbody>().centerOfMass = transform.localPosition;
    }

}
