using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeObjectPrefab : MonoBehaviour
{

    public ObjectsPrefab Objects;
    public Vector3 SizeObject = Vector3.one;
    public Vector3 PositionOffset = Vector3.zero;



#if UNITY_EDITOR
    public void Bake()
    {
        Objects.SizeObject = SizeObject;
        Objects.PositionOffset = PositionOffset;

        CildObject cildObject = new CildObject();
        Objects.CildObjects = new List<CildObject>();

        for (int i = 0; i < transform.childCount; i++)
        {

            var obj = transform.GetChild(i).gameObject;
            
            cildObject.type = GetPrimitiveType(obj.GetComponent<MeshFilter>().sharedMesh.name);

            cildObject.name = obj.name;
           
            cildObject.position = obj.transform.localPosition;
            cildObject.rotation = obj.transform.localRotation;
            cildObject.scale = obj.transform.localScale;

            cildObject.material = obj.GetComponent<MeshRenderer>().sharedMaterial;

            cildObject.mass = obj.GetComponent<Rigidbody>().mass;

            var joints = obj.GetComponents<Joint>();
            cildObject.joints = new List<JointsObject>();

            foreach (Joint j in joints)
            {
                
                    JointsObject jointsObject = new JointsObject();
                    switch (j.GetType().Name)
                    {
                        case "CharacterJoint":
                            jointsObject = SetSettingsJoint(j, ComponentType.JointType.CharacterJoint);

                        break;
                        case "ConfigurableJoint":
                            jointsObject = SetSettingsJoint(j, ComponentType.JointType.ConfigurableJoint);
                            break;
                        case "FixedJoint":
                            jointsObject = SetSettingsJoint(j, ComponentType.JointType.FixedJoint);
                            break;
                        case "HingeJoint":
                            jointsObject = SetSettingsJoint(j, ComponentType.JointType.HingeJoint);
                            break;
                        case "SpringJoint":
                            jointsObject = SetSettingsJoint(j, ComponentType.JointType.SpringJoint);
                            break;
                    }

                cildObject.joints.Add(jointsObject);

                
            }
            

            Objects.CildObjects.Add(cildObject);
        }
        
    }
    private JointsObject SetSettingsJoint(in Joint j, ComponentType.JointType jointType)
    {

        JointsObject obj = new JointsObject();
        obj.typeJoint = jointType;
        if (j.connectedBody != null)
            obj.SerialNamberConnectbody = GetSerialNumberObjectInParent(j.connectedBody.gameObject);
        else obj.SerialNamberConnectbody = -1;
        obj.breakForce = j.breakForce;
        obj.breakTorque = j.breakTorque;
        obj.massScale = j.massScale;
        obj.anchor = j.anchor;
        obj.axis = j.axis;
        return obj;
    }
    private int GetSerialNumberObjectInParent(in GameObject ojb)
    {
        var patern = ojb.transform.parent;
        for (int i = 0; i < patern.transform.childCount; i++)
        {
            if (ojb == patern.GetChild(i).gameObject)
            {
                return i;
            }
        }
        return -1;
    }
#endif
    public void Build()
    {

        SizeObject = Objects.SizeObject;
        PositionOffset = Objects.PositionOffset;

        foreach (CildObject item in Objects.CildObjects)
        {
            // Create objent as primitive
            var t = GameObject.CreatePrimitive(item.type);

            // Set parent and name
            t.transform.parent = transform;
            t.name = item.name;
            // Set Transform
           
            t.transform.localPosition = item.position;
            t.transform.localRotation = item.rotation;
            t.transform.localScale = item.scale;

            // Set material
            t.GetComponent<MeshRenderer>().material = item.material;

            // Set rigitbody
            var rb = t.AddComponent<Rigidbody>();
            rb.mass = item.mass;

            // set joint

            foreach (JointsObject jointdata in item.joints)
            {
                var j = t.AddComponent(ComponentType.GetCompType(jointdata.typeJoint)) as Joint;
                j.breakForce = jointdata.breakForce;
                j.breakTorque = jointdata.breakTorque;
                j.massScale = jointdata.massScale;
                j.anchor = jointdata.anchor;
                j.axis = jointdata.axis;
            }
            // Set MehsDefor script
            t.AddComponent<MeshDeform>();
        }
        // set connect body 
        for (int i = 0; i < transform.childCount; i++)
        {
            var item = Objects.CildObjects[i];

            foreach (JointsObject jointdata in item.joints)
            {
                if (jointdata.SerialNamberConnectbody < 0) continue;
                var obj = transform.GetChild(i);
                var j = obj.GetComponent(ComponentType.GetCompType(jointdata.typeJoint)) as Joint;
                j.connectedBody = transform.GetChild(jointdata.SerialNamberConnectbody).GetComponent<Rigidbody>();
            }
        }
    }

#if UNITY_EDITOR
    PrimitiveType GetPrimitiveType(string name)
    {
        switch (name[1])
        {
            case 'a':
                return PrimitiveType.Capsule;
            case 'u':
                return (name[0] == 'C')? PrimitiveType.Cube: PrimitiveType.Quad;
            case 'y':
                return PrimitiveType.Cylinder;
            case 'p':
                return PrimitiveType.Sphere;
            case 'l':
                return PrimitiveType.Plane;
        }

        return PrimitiveType.Cube;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + PositionOffset, SizeObject);
    }
#endif
}
