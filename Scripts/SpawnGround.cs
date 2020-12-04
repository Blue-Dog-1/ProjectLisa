using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGround : MonoBehaviour
{
    [SerializeField] GameObject groundBox;
    [SerializeField] Vector3 startPosition;
    [SerializeField] int distance;
    bool isGenerationNaw = false;

    private void Start()
    {
        startPosition = transform.position;
        Debug.Log("Star Position " + startPosition);
        Generation(distance);
    }

    void Generation(int quantity)
    {
        isGenerationNaw = true;
        for (int i = 0; i < quantity; i++)
        {
            var instan = Instantiate(groundBox, startPosition, Quaternion.identity, transform);
            startPosition.z += 2;

        }
        isGenerationNaw = false;
    }
    private void Update()
    {
        if ((Events.Player.transform.position - startPosition).magnitude < distance && !isGenerationNaw)
            Generation(2);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(Events.Player.transform.position, startPosition);
    }
#endif
}
