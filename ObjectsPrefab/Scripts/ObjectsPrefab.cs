using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ObjectsPrefab", menuName = "ScriptableObject/ObjectsPrefab")]
public class ObjectsPrefab : ScriptableObject
{
    public float indent;
    public Vector3 SizeObject;
    public Vector3 PositionOffset;
    public List<CildObject> CildObjects;
    public List<Material> materials;
    
}

[Serializable]
public struct CildObject
{
    public string name;
    public primitiveType type;
    // transform
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    // mesh 
    public string mesh;
    public Mesh Mesh;
    public Material material;

    public float mass;

    // joint
    
    public List<JointsObject> joints;

}
[Serializable]
public struct JointsObject
{
    public ComponentType.JointType typeJoint;
    public int SerialNamberConnectbody;
    public float breakForce;
    public float breakTorque;
    public float massScale;

    public Vector3 anchor;
    public Vector3 axis;

    
}


