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

    void Start()
    {
        posPlayr = Playar.position;
    }
    public virtual void OnDrag(PointerEventData eventData)
    {
        position = eventData.delta;

        position *= Time.deltaTime;
        posPlayr.x += position.x;
        posPlayr.z = Playar.position.z;
        posPlayr.x = Mathf.Clamp(posPlayr.x, -Clamp, Clamp);

        Playar.position = posPlayr;

        //Playar.Translate(position.x, 0f, position.y);
    }
   
   
}
