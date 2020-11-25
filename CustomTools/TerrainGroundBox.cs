#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainGroundBox : MonoBehaviour
{
    public GameObject groundBox;
    public int wigth;
    public int heigth;
    public float[] angles = new float[4] { 0f, 90f, 180f, 270f };

    [SerializeField]
    public void AddBox()
    {
       
        

    }
    public void RandomQaternion()
    {
        Vector3 center = Vector3.zero;
        Vector3[] axis = new Vector3[3] { Vector3.up, Vector3.forward, Vector3.right };
        for (int i = 0; i < transform.childCount; i++)
        {
            center = transform.GetChild(i).transform.position;
            center.y += 0.75f;
            var _axis = axis[Random.Range(0, 3)];
            var _angle = angles[Random.Range(0, 3)];
            var obj = transform.GetChild(i);

            obj.transform.RotateAround(center, _axis, _angle);
            obj.GetChild(obj.childCount - 1).transform.RotateAround(center, _axis, _angle * -1);
        }
    }
    public void RemoveChild()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
    
    
}
#endif
