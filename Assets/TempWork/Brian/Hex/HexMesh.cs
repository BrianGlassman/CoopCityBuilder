using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://catlikecoding.com/unity/tutorials/hex-map/part-1/
// Section 3 - Rendering Hexagons

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles; // Indices into the vertex arrays

    LineRenderer

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {

    }
}
