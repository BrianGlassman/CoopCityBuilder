using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main Grid interface, mostly similar to Unity's Grid objects.
/// Any grid interactions should probably use the public, non-underscored parts of this class
/// </summary>
[ExecuteAlways]
public class HexGrid : MonoBehaviour
{
    //----------------------
    // Unity-like interface
    //----------------------
    // ##### Grid interface - public methods #####
    /// <summary>
    /// Get the logical center coordinate of a grid cell in local space.
    /// </summary>
    /// <param name="position">The cell coordinates</param>
    /// <returns></returns>
    public Vector3 GetCellCenterLocal(Vector3Int position)
    {
        var coords = HexCoordinates.FromVector3Int(position);
        return HexMetrics.CellToLocal(coords);
    }
    /// <summary>
    /// Get the logical center coordinate of a grid cell in world space.
    /// </summary>
    /// <param name="position">The cell coordinates</param>
    /// <returns></returns>
    public Vector3 GetCellCenterWorld(Vector3Int position)
    {
        var coords = HexCoordinates.FromVector3Int(position);
        var local = HexMetrics.CellToLocal(coords);
        return transform.InverseTransformPoint(local);
    }
    // ##### Grid interface - static methods #####
    //      public static Vector3 InverseSwizzle(GridLayout.CellSwizzle swizzle, Vector3 position);
    //      public static Vector3 Swizzle(GridLayout.CellSwizzle swizzle, Vector3 position);
    // ##### GridLayout interface - properties #####
    /// <summary>
    /// The size of the gap between each cell in the Grid.
    /// Only non-zero for isometric grids.
    /// </summary>
    public Vector3 cellGap = Vector3.zero;
    /// <summary>
    /// The layout of the GridLayout.
    /// The layout determines the conversion of positions from cell space to local space and vice versa.
    /// </summary>
    public GridLayout.CellLayout cellLayout = GridLayout.CellLayout.Hexagon;
    /// <summary>
    /// The size of each cell in the Grid
    /// </summary>
    public Vector3 cellSize { get { return HexMetrics.cellSize; } }
    /// <summary>
    /// The cell swizzle for the grid
    /// </summary>
    public GridLayout.CellSwizzle cellSwizzle()
    {
        return GridLayout.CellSwizzle.XZY;
    }
    // ##### GridLayout interface - public methods #####
    /// <summary>
    /// For hexagonal grid, is identical to GetCellCenterLocal.
    /// For rectangular grid, gets the bottom-left coordinate in local space.
    /// </summary>
    /// <param name="cellPosition">The cell coordinates</param>
    /// <returns></returns>
    public Vector3 CellToLocal(Vector3Int cellPosition) { return GetCellCenterLocal(cellPosition); }
    /// <summary>
    /// Converts an interpolated cell position in floats to local position space.
    /// </summary>
    /// <param name="cellPosition">Interpolated cell position to convert</param>
    /// <returns></returns>
    public Vector3 CellToLocalInterpolated(Vector3 cellPosition)
    {
        // FIXME
        return Vector3.zero;
    }
    /// <summary>
    /// For hexagonal grid, is identical to GetCellCenterWorld.
    /// For rectangular grid, gets the bottom-left coordinate in world space.
    /// </summary>
    /// <param name="cellPosition">The cell coordinates</param>
    /// <returns></returns>
    public Vector3 CellToWorld(Vector3Int cellPosition) { return GetCellCenterWorld(cellPosition); }
    //      public Bounds GetBoundsLocal(Vector3Int cellPosition);
    //      public Vector3 GetLayoutCellCenter();
    //      public Vector3Int LocalToCell(Vector3 localPosition);
    //      public Vector3 LocalToCellInterpolated(Vector3 localPosition);
    //      public Vector3 LocalToWorld(Vector3 localPosition);
    //      public Vector3Int WorldToCell(Vector3 worldPosition);
    /// <summary>
    /// Converts a world position to local position.
    /// </summary>
    /// <param name="worldPosition">World Position to convert.</param>
    /// <returns></returns>
    public Vector3 WorldToLocal(Vector3 worldPosition) { return transform.InverseTransformPoint(worldPosition); }
    //----------------------

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
        Gizmos.color = Color.white;
        for (int d = 0; d < height; d++)
        {
            for (int h = 0; h < width; h++)
            {
                // Get the world position
                var localPos = HexMetrics.CellToLocal(h, d);
                // Label the coordinates
                UnityEditor.Handles.Label(
                    transform.TransformPoint(localPos),
                    "(" + h.ToString() + ", " + d.ToString() + ")"
                );
                // Draw the hex outline
                for (int i = 0; i < 6; i++)
                {
                    Gizmos.DrawLine(
                        transform.TransformPoint(HexMetrics.corners[i] + localPos),
                        transform.TransformPoint(HexMetrics.corners[i + 1] + localPos)
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
