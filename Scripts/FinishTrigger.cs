using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    bool isTiggerExet = false;
    private void Start()
    {
        var Level = PlayerPrefs.GetInt("Level", 0);
        var step = PlayerPrefs.GetInt("Step", 0);
        step += Level;
        Vector3 stepZ = transform.position;
        stepZ.z += step;
        transform.position = stepZ;

        PlayerPrefs.SetInt("Step", step + 2);
        PlayerPrefs.Save();
    }
    private void OnTriggerEnter(Collider other)
    {
        Events.onBrakeSpawn();
    }
    private void OnTriggerExit(Collider other)
    {
        if (!isTiggerExet)
        {
            GameObject.FindGameObjectWithTag("PlayerSub").GetComponent<Animation>().Play();
            isTiggerExet = true;
        }
    }

    
}
