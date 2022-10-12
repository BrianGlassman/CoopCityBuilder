using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class HexCell : MonoBehaviour
{
    // See HexMetrics for coordinate system explanation
    public int H;
    public int D;

    private Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles; // Indices into the vertex arrays
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Configure the mesh
        {
            MeshCollider coll = GetComponent<MeshCollider>();
            coll.sharedMesh = mesh = new Mesh();
            mesh.name = "Cell Mesh";
            vertices = new List<Vector3>();
            triangles = new List<int>();
            // Triangulate
            {
                var center = CellToWorld();
                for (int i = 0; i < 6; i++)
                {
                    AddTriangle(
                        center,
                        center + HexMetrics.corners[i],
                        center + HexMetrics.corners[i + 1]
                    );
                }
            }
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();

            // FIXME the first mesh is buggy for some reason. Second+ meshes all work fine. First mesh works if the component is disabled then re-enabled
            coll.enabled = false;
            coll.enabled = true;
        }
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

    public Vector3 CellToWorld()
    {
        return HexMetrics.CellToWorld(H, D);
    }
}
