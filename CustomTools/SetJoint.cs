#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using UnityEditor.SceneManagement;
using System;
using UnityEngine.Rendering;

public class SetJoint : EditorWindow
{
    int BreakForce = 100;

    int BreakTorque = 100;

    float MassScale = 1f;
    float ConnectedMassScale = 1f;

    bool onlyChange;

    string[] jointsList = new string[] {"Character Joint", "ConfigurableJoint", "Fixed Joint", "Hinge Joint", "Spring Joint" };
    int index = 0;
    int newindex = 0;
    string setJoint = "Set Joint";
    string DeleteAllJoin = "Delete All Join";

    static List<GameObject> SelectionObjects;


    public Vector3 centermass = Vector3.zero;
    GizmoDrawer giz;

    [MenuItem("Tools/Costom Tools/Set Joins")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SetJoint));
    }
    
   
    
    
    void OnGUI()
    {
        // The actual window code goes here
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        BreakForce = EditorGUILayout.IntField("Break Force", BreakForce);
        BreakTorque = EditorGUILayout.IntField("Break Torque", BreakTorque);
        MassScale = EditorGUILayout.FloatField("Mass Scale", MassScale);
        ConnectedMassScale = EditorGUILayout.FloatField("Connected Mass Scale", ConnectedMassScale);
        onlyChange = EditorGUILayout.Toggle("Only Change", onlyChange);


        index = EditorGUILayout.Popup(index, jointsList);

        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button(setJoint,  GUILayout.Height(30) ) )
        {
            if (onlyChange) 
                ChangeJoints(index, BreakForce, BreakTorque, MassScale, ConnectedMassScale);
            else 
                SetJoints(index, BreakForce, BreakTorque, MassScale, ConnectedMassScale);

            SetObjectDirty(Selection.gameObjects);

        }
        if (GUILayout.Button(DeleteAllJoin, GUILayout.Height(30)))
        {
            DeleteJoin(index);
            
            SetObjectDirty(Selection.gameObjects);
        }
        GUILayout.EndHorizontal();

        centermass = EditorGUILayout.Vector3Field("Center mass", centermass);

        if (GUILayout.Button("Set center mass", GUILayout.Height(30)))
        {
            Selection.activeGameObject.GetComponent<Rigidbody>().centerOfMass = Selection.activeGameObject.transform.position + centermass;
        }

    }





    private void OnInspectorUpdate()
    {
        if (newindex != index)
        {
            newindex = index;
            DeleteAllJoin = "Delere " + jointsList[index];
        }

        if (GUI.changed) Debug.Log("GUI.changed");

    }
    public static void SetObjectDirty(GameObject[] objs)
    {
        foreach (GameObject item in objs)
        {
            EditorUtility.SetDirty(item);
            //EditorSceneManager.MarkSceneDirty(item.scene);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("EditorWindow is Closed");
    }




    /*
     *                  //\\    //\\     |||||||| |||||||||| |||          ||||    ||||\\\      ////////
     *                 ///\\\  ///\\\    |||         |||     |||        ///  \\\  |||   \\\   ///
     *                ///  \\\///  \\\   |||||||     |||     ||||||||| |||    ||| |||    |||  \\\\\\\\
     *               ///    \\//    \\\  |||         |||     |||   |||  \\\  ///  |||   ///        ///
     *              ///              \\\ ||||||||    |||     |||   |||    ||||    ||||///    ////////
     */

    
    public static void SetJoints(int type, int breakForce, int BreakTorque, float MassScale, float ConnectedMassScale)
    {
        
        if (Selection.gameObjects.Length <= 1)
        {
            var joint = Selection.activeGameObject.GetComponent(ComponentType.GetCompType(type)) as Joint;
            if (joint == null)
            {
                joint = Selection.activeGameObject.AddComponent(ComponentType.GetCompType(type)) as Joint;
                joint.breakForce = breakForce;
                joint.breakTorque = BreakTorque;
                joint.massScale = MassScale;
                joint.connectedMassScale = ConnectedMassScale;
            }
            else if (joint != null)
            {
                joint.breakForce = breakForce;
                joint.breakTorque = BreakTorque;
                joint.massScale = MassScale;
                joint.connectedMassScale = ConnectedMassScale;
            }
            //throw new Exception("selected " + Selection.gameObjects.Length + " object. select from two or more objects");
        }
        
        foreach (GameObject item in Selection.gameObjects)
        {
            if (Selection.activeGameObject == item) continue;

            var joint = Selection.activeGameObject.GetComponent(ComponentType.GetCompType(type)) as Joint;
            if (joint != null) // have a joint
            {
                if (joint.connectedBody == null) // not have a connect body
                {
                    joint.breakForce = breakForce;
                    joint.breakTorque = BreakTorque;
                    joint.massScale = MassScale;
                    joint.connectedMassScale = ConnectedMassScale;
                    joint.connectedBody = item.GetComponent<Rigidbody>();
                }

                else if (joint.connectedBody != null) // have a connect body
                {
                    joint = Selection.activeGameObject.AddComponent(ComponentType.GetCompType(type)) as Joint;
                    joint.breakForce = breakForce;
                    joint.breakTorque = BreakTorque;
                    joint.massScale = MassScale;
                    joint.connectedMassScale = ConnectedMassScale;
                    joint.connectedBody = item.GetComponent<Rigidbody>();
                }
            }
            else if (joint == null) // not have a joint
            {
                joint = Selection.activeGameObject.AddComponent(ComponentType.GetCompType(type)) as Joint;
                joint.breakForce = breakForce;
                joint.breakTorque = BreakTorque;
                joint.massScale = MassScale;
                joint.connectedMassScale = ConnectedMassScale;
                joint.connectedBody = item.GetComponent<Rigidbody>();
            }
        }
    }
    public static void ChangeJoints(int type, int breakForce, int BreakTorque, float MassScale, float ConnectedMassScale)
    {
        foreach (GameObject item in Selection.gameObjects)
        {
            Component[] joints = item.GetComponents(ComponentType.GetCompType(type)) as Component[];
            foreach ( Joint j in joints)
                if (j != null) // have a joint
                {
                    j.breakForce = breakForce;
                    j.breakTorque = BreakTorque;
                    j.massScale = MassScale;
                    j.connectedMassScale = ConnectedMassScale;
                }
        }
    }

    public static void DeleteJoin(int type)
    {
        foreach (GameObject item in Selection.gameObjects)
        {
            try
            {
                Component[] joints = item.GetComponents(ComponentType.GetCompType(type)) as Component[];
                foreach (Component i in joints)
                    DestroyImmediate(i);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("this componetn does not exist " + ex.Message +" "+ ex.Source);
            }
        }
    }
}





public class GizmoDrawer : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Vector3.zero, Vector3.up * 50f);
    }

    private void Start()
    {
        Gizmos.DrawLine(Vector3.zero, Vector3.up * 50f);
    }

    private void Update()
    {
        Gizmos.DrawLine(Vector3.zero, Vector3.up * 50f); 
    }

}

#endif