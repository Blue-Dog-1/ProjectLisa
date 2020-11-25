#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BakeObjectPrefab))]
public class InspectorBake : Editor
{
    BakeObjectPrefab bake;

    private void OnEnable()
    {
        bake = (BakeObjectPrefab)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Objects Prefab");
        bake.Objects = (ObjectsPrefab)EditorGUILayout.ObjectField("Objects Prefab", bake.Objects, typeof(ObjectsPrefab), false);
        bake.SizeObject = EditorGUILayout.Vector3Field("Space Object", bake.SizeObject);
        bake.PositionOffset = EditorGUILayout.Vector3Field("Position Offset", bake.PositionOffset);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Bake GameOjects", GUILayout.Height(30) ))
        {
            bake.Bake();
        }
        GUILayout.Space(40);
        if (GUILayout.Button("Buid GameObjects", GUILayout.Height(30) ))
        {
            bake.Build();
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Debug") )
        {
        }
    }
}
#endif