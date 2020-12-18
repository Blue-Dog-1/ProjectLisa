using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField]
    Vector3 StarPosition;
    [SerializeField]
    List<ObjectsPrefab> Objects;
    [SerializeField]
    int Quantity;
    public int Level;
    [SerializeField] int DistanceToNextSpawn;
    public int DistansSpawn;
    float delay;

    [Space]
    [Header("Ground")]
    [SerializeField] GameObject groundCarent;

    public void Start()
    {
        delay = 0.05f * (DistansSpawn / MobailControl.Speed);
        StartCoroutine(loop(Quantity));
        Events.BrakeSpawn += delegate () { StopAllCoroutines();  };
    }

    int RandomeIndex(int oldindex, int min, int max)
    {
        int i = Random.Range(min, max);
        if (oldindex == i) return RandomeIndex(oldindex, min, max);
        else return i;
    }
    IEnumerator loop (int quantity)
    {
        float indent = 0f;
        int Index = int.MaxValue;

        Vector3 position = groundCarent.transform.position; 
        while (quantity >= 1)
        {
            var o = new GameObject();
            var bop = o.AddComponent<BakeObjectPrefab>();
            Index = RandomeIndex(Index, 0, Objects.Count);
            bop.Objects = Objects[Index];
            indent += bop.Objects.indent;
            
            StarPosition.z = indent;
            //StarPosition.x = Random.Range(-1, 1);
            o.transform.position = StarPosition;

            indent += bop.Objects.indent;

            quantity--;

            bop.Build();
        }
        yield return new WaitForSeconds(delay * quantity);
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            #region Ground generation
            if ((position.z - transform.position.z) < 250f)
            {
                position = groundCarent.transform.position;
                position.z += groundCarent.transform.localScale.y;

                var newgroundCarent = Instantiate(groundCarent, position, groundCarent.transform.rotation);
                groundCarent = newgroundCarent;
            }
            #endregion


            if ((transform.position - StarPosition).magnitude > DistanceToNextSpawn) continue;
            var o = new GameObject();
            var bop = o.AddComponent<BakeObjectPrefab>();
            Index = RandomeIndex(Index, 0, Objects.Count);

            bop.Objects = Objects[Index];

            indent += bop.Objects.indent;
            
            StarPosition.z = indent;
            //StarPosition.x = Random.Range(-1, 1);

            indent += bop.Objects.indent;

            o.transform.position = StarPosition;

            quantity--;
            bop.Build();


           
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, StarPosition);
        Gizmos.DrawLine(transform.position, groundCarent.transform.position);
    }
#endif

    public ObjectsPrefab objects;
    public Vector3 SizeObject = Vector3.one;
    public Vector3 PositionOffset = Vector3.zero;

    public void Build()
    {


        SizeObject = objects.SizeObject;
        PositionOffset = objects.PositionOffset;

        PrimitiveType ConvetrPrimitiveType(primitiveType type)
        {
            switch (type)
            {
                case primitiveType.Capsule:
                    return PrimitiveType.Capsule;
                case primitiveType.Cube:
                    return PrimitiveType.Cube;
                case primitiveType.Quad:
                    return PrimitiveType.Quad;
                case primitiveType.Cylinder:
                    return PrimitiveType.Cylinder;
                case primitiveType.Sphere:
                    return PrimitiveType.Sphere;
                case primitiveType.Plane:
                    return PrimitiveType.Plane;
            }
            return PrimitiveType.Cube;
        }

        foreach (CildObject item in objects.CildObjects)
        {
            // Create objent as primitive
            GameObject t;
            if (item.type != primitiveType.Mesh)
                t = GameObject.CreatePrimitive(ConvetrPrimitiveType(item.type));
            else
            {
                t = GameObject.CreatePrimitive(PrimitiveType.Cube);
                t.GetComponent<MeshFilter>().mesh = item.Mesh;
                if (Application.isEditor)
                    DestroyImmediate(t.GetComponent<Collider>());
                else
                    Destroy(t.GetComponent<Collider>());
                var coll = t.AddComponent<MeshCollider>();
                coll.sharedMesh = item.Mesh;
                coll.convex = true;

            }

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
            var item = objects.CildObjects[i];

            foreach (JointsObject jointdata in item.joints)
            {
                if (jointdata.SerialNamberConnectbody < 0) continue;
                var obj = transform.GetChild(i);
                var j = obj.GetComponent(ComponentType.GetCompType(jointdata.typeJoint)) as Joint;
                j.connectedBody = transform.GetChild(jointdata.SerialNamberConnectbody).GetComponent<Rigidbody>();
            }
        }
    }

}
