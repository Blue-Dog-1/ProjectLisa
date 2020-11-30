using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        Events.onBrakeSpawn();
    }
    private void OnTriggerExit(Collider other)
    {
        Events.onFinish();
    }

    
}
