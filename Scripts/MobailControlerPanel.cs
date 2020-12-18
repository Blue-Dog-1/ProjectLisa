using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MobailControlerPanel : MonoBehaviour, IDragHandler
{
    [SerializeField] Transform Playar;
    Vector2 position;
    [SerializeField] float velocity;
    Vector3 posPlayr;
    [SerializeField] float Clamp;
    [SerializeField] float ClampZ;

    [SerializeField] float ClapmVelosity;
    float booferAxisZ = 0f;

    void Start()
    {
        posPlayr = Playar.position;
    }
    public virtual void OnDrag(PointerEventData eventData)
    {
        position = eventData.delta;

        position *= Time.deltaTime;

        position.x = Mathf.Clamp(position.x, -ClapmVelosity, ClapmVelosity);
        position.y = Mathf.Clamp(position.y, -ClapmVelosity, ClapmVelosity);

        // X axis move
        posPlayr.x += position.x;
        posPlayr.x = Mathf.Clamp(posPlayr.x, -Clamp, Clamp);

        // Z axis move
        posPlayr.z = Playar.position.z;
        posPlayr.z += position.y;
        posPlayr.z = Mathf.Clamp(posPlayr.z, Playar.parent.transform.position.z + ClampZ, Playar.parent.transform.position.z);

        Playar.position = posPlayr;
        //Playar.position = Vector3.Lerp(Playar.position, posPlayr, Time.deltaTime * velocity);

    }
   
   
}
