using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutMesh : MonoBehaviour
{
    Vector3[] vertices;
    int[] triangles;
    Mesh mesh;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        triangles = mesh.triangles;
    }

}
