#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGroundBox))]
public class InspectorTGB : Editor
{
    TerrainGroundBox TBox;

    void OnEnable()
    {
        TBox = (TerrainGroundBox)target;

    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Prefab GraundBox");
        TBox.groundBox = (GameObject)EditorGUILayout.ObjectField("Ground Box", TBox.groundBox, typeof(GameObject), false);
        EditorGUILayout.LabelField("Wigth and Wigth");
        TBox.angles[0] = EditorGUILayout.FloatField("angles 0", TBox.angles[0]);
        TBox.angles[1] = EditorGUILayout.FloatField("angles 1", TBox.angles[1]);
        TBox.angles[2] = EditorGUILayout.FloatField("angles 2", TBox.angles[2]);
        TBox.angles[3] = EditorGUILayout.FloatField("angles 3", TBox.angles[3]);


        EditorGUILayout.Space();
        if (GUILayout.Button("add box"))
        {
            TBox.RandomQaternion();
            EditorUtility.SetDirty(TBox.gameObject);
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Remove Child"))
        {
            TBox.RemoveChild();
        }

    }
   

}
#endif