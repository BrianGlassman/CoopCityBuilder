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
    [SerializeField] private static int width = 7;
    [SerializeField] private static int height = 7;

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
            for (int d = Mathf.CeilToInt(-height / 2.0f); d < Mathf.CeilToInt(height / 2.0f); d++)
            {
                for (int h = Mathf.CeilToInt(-width / 2.0f); h < Mathf.CeilToInt(width / 2.0f); h++)
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

    #if UNITY_EDITOR // UnityEditor isn't available in builds, so enclose it to avoid errors
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int d = Mathf.CeilToInt(-height / 2.0f); d < Mathf.CeilToInt(height / 2.0f); d++)
        {
            for (int h = Mathf.CeilToInt(-width / 2.0f); h < Mathf.CeilToInt(width / 2.0f); h++)
            {
                // Get the world position
                var localPos = HexMetrics.CellToLocal(h, d);
                // Label the coordinates
                UnityEditor.Handles.Label(
                    Utils.LocalToWorld(transform, localPos),
                    "(" + h.ToString() + ", " + d.ToString() + ")"
                );
                // Draw the hex outline
                for (int i = 0; i < 6; i++)
                {
                    Gizmos.DrawLine(
                        Utils.LocalToWorld(transform, HexMetrics.corners[i] + localPos),
                        Utils.LocalToWorld(transform, HexMetrics.corners[i + 1] + localPos)
                    );
                }
            }
        }
    }
    #endif

    private void CreateCell(int cellH, int cellD, Sprite sprite = null)
    {
        var cell = Instantiate<HexCell>(hexCellPrefab, transform);
        cell.H = cellH;
        cell.D = cellD;
        cell.SetModel(sprite);
        cell.transform.SetLocalPositionAndRotation(CellToLocal(cellH, cellD), Quaternion.identity);
        cells[new HexCoordinates(cellH, cellD)] = cell;
    }

    public Vector3 CellToLocal(int cellH, int cellD) { return HexMetrics.CellToLocal(cellH, cellD); }
}
