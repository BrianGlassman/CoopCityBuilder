using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://catlikecoding.com/unity/tutorials/hex-map/part-1/
// Section 3 - Rendering Hexagons

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    Mesh hexMesh;
    List<Vector3> vertices;
    List<int> triangles; // Indices into the vertex arrays

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    /// <summary>
    /// Clears all the internal storages
    /// </summary>
    void Clear()
    {
        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();
    }

    /// <summary>
    /// Adds a triangle to the mesh with vertices at the given locations
    /// Vertices must be given in order
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        // First index for the new triangles, starting at the end of the existing ones
        int firstIndex = vertices.Count;
        triangles.Add(firstIndex);
        triangles.Add(firstIndex + 1);
        triangles.Add(firstIndex + 2);
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
    }

    public void Triangulate(HexCell[] cells)
    {
        Clear();
        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();
    }
    private void Triangulate(HexCell cell)
    {
        var center = cell.CellToWorld();
        for (int i = 0; i < 6; i++)
        {
            AddTriangle(
                center,
                center + HexMetrics.corners[i],
                center + HexMetrics.corners[i + 1]
            );
        }
    }
}
