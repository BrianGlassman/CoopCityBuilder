using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main Grid interface, mostly similar to Unity's Grid objects.
/// Any grid interactions should probably use the properties/functions defined here.
/// If you're following a tutorial/documentation/forum and  want to use the Unity Grid
/// properties/functions, they are defined in HexGrid_Unitylike and are prefixed with 
/// "_Unity_".
/// </summary>
[ExecuteAlways]
public partial class HexGrid : MonoBehaviour
{
    
    // Alternate input for Unity-like function
    public Vector3 GetCellCenterLocal(int cellH, int cellD)
    {
        return HexMetrics.CellToLocal(cellH, cellD);
    }

    public static int width = 6;
    public static int height = 6;

    private HexMesh hexMesh;

    private readonly Dictionary<HexCoordinates, HexCell> cells = new();
    private HexCell[] cellsArray
    {
        get
        {
            HexCell[] arr = new HexCell[cells.Count];
            cells.Values.CopyTo(arr, 0);
            return arr;
        }
    }

    [SerializeField] HexCell hexCellPrefab;

    private void Awake()
    {
        // Create the game-view grid
        if (Application.isPlaying)
        {
            for (int d = 0; d < height; d++)
            {
                for (int h = 0; h < width; h++)
                {
                    CreateCell(h, d);
                }
            }

            hexMesh = GetComponentInChildren<HexMesh>();
        }
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            hexMesh.Triangulate(cellsArray);
        }
    }

    private void OnDrawGizmos()
    {
        // FIXME use non-Unity interface
        Gizmos.color = Color.white;
        for (int d = 0; d < height; d++)
        {
            for (int h = 0; h < width; h++)
            {
                // Get the world position
                var localPos = HexMetrics.CellToLocal(h, d);
                // Label the coordinates
                UnityEditor.Handles.Label(
                    _Unity_LocalToWorld(localPos),
                    "(" + h.ToString() + ", " + d.ToString() + ")"
                );
                // Draw the hex outline
                for (int i = 0; i < 6; i++)
                {
                    Gizmos.DrawLine(
                        _Unity_LocalToWorld(HexMetrics.corners[i] + localPos),
                        _Unity_LocalToWorld(HexMetrics.corners[i + 1] + localPos)
                    );
                }
            }
        }
    }

    private void CreateCell(int cellH, int cellD, Sprite sprite = null)
    {
        var cell = Instantiate<HexCell>(hexCellPrefab, transform);
        cell.H = cellH;
        cell.D = cellD;
        cell.spriteRenderer.sprite = sprite;
        cell.transform.SetLocalPositionAndRotation(GetCellCenterLocal(cellH, cellD), Quaternion.identity);
        cells[new HexCoordinates(cellH, cellD)] = cell;
    }
}
