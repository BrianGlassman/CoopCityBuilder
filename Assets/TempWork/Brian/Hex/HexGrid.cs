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
    public Vector3 Unity_GetCellCenterLocal(Vector3Int position)
    {
        var coords = HexCoordinates.FromVector3Int(position);
        return HexMetrics.CellToLocal(coords);
    }
    /// <summary>
    /// Get the logical center coordinate of a grid cell in world space.
    /// </summary>
    /// <param name="position">The cell coordinates</param>
    /// <returns></returns>
    public Vector3 Unity_GetCellCenterWorld(Vector3Int position)
    {
        var coords = HexCoordinates.FromVector3Int(position);
        var local = HexMetrics.CellToLocal(coords);
        return Unity_LocalToWorld(local);
    }
    // ##### Grid interface - static methods #####
    // TODO public static Vector3 InverseSwizzle(GridLayout.CellSwizzle swizzle, Vector3 position);
    // TODO public static Vector3 Swizzle(GridLayout.CellSwizzle swizzle, Vector3 position);
    // ##### GridLayout interface - properties #####
    /// <summary>
    /// The size of the gap between each cell in the Grid.
    /// Only non-zero for isometric grids.
    /// </summary>
    public Vector3 Unity_cellGap = Vector3.zero;
    /// <summary>
    /// The layout of the GridLayout.
    /// The layout determines the conversion of positions from cell space to local space and vice versa.
    /// </summary>
    public GridLayout.CellLayout Unity_cellLayout = GridLayout.CellLayout.Hexagon;
    /// <summary>
    /// The size of each cell in the Grid
    /// </summary>
    public Vector3 Unity_cellSize { get { return HexMetrics.cellSize; } }
    /// <summary>
    /// The cell swizzle for the grid
    /// </summary>
    public GridLayout.CellSwizzle Unity_cellSwizzle()
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
    public Vector3 Unity_CellToLocal(Vector3Int cellPosition) { return Unity_GetCellCenterLocal(cellPosition); }
    /// <summary>
    /// Converts an interpolated cell position in floats to local position space.
    /// </summary>
    /// <param name="cellPosition">Interpolated cell position to convert</param>
    /// <returns></returns>
    public Vector3 Unity_CellToLocalInterpolated(Vector3 cellPosition)
    {
        // FIXMELOW Not future-proofed against changes in [Vector3 --> cell coordinates] conversion
        return HexMetrics.CellToLocalInterpolated(cellPosition[0], cellPosition[1]);
    }
    /// <summary>
    /// For hexagonal grid, is identical to GetCellCenterWorld.
    /// For rectangular grid, gets the bottom-left coordinate in world space.
    /// </summary>
    /// <param name="cellPosition">The cell coordinates</param>
    /// <returns></returns>
    public Vector3 Unity_CellToWorld(Vector3Int cellPosition) { return Unity_GetCellCenterWorld(cellPosition); }
    // TODO public Bounds GetBoundsLocal(Vector3Int cellPosition);
    // TODO public Vector3 GetLayoutCellCenter();
    public Vector3Int Unity_LocalToCell(Vector3 localPosition)
    {
        // FIXMELOW Not future-proofed against changes in [Vector3 --> cell coordinates] conversion
        var hd = HexMetrics.LocalToCell(localPosition);
        return new Vector3Int(hd[0], 0, hd[1]);
    }
    public Vector3 Unity_LocalToCellInterpolated(Vector3 localPosition)
    {
        // FIXMELOW Not future-proofed against changes in [Vector3 --> cell coordinates] conversion
        var hd = HexMetrics.LocalToCellInterpolated(localPosition);
        return new Vector3(hd[0], 0, hd[1]);
    }
    public Vector3 Unity_LocalToWorld(Vector3 localPosition)
    {
        return transform.TransformPoint(localPosition);
    }
    public Vector3Int Unity_WorldToCell(Vector3 worldPosition)
    {
        var localPos = Unity_WorldToLocal(worldPosition);
        return Unity_LocalToCell(localPos);
    }
    /// <summary>
    /// Converts a world position to local position.
    /// </summary>
    /// <param name="worldPosition">World Position to convert.</param>
    /// <returns></returns>
    public Vector3 Unity_WorldToLocal(Vector3 worldPosition) { return transform.InverseTransformPoint(worldPosition); }
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
                    Unity_LocalToWorld(localPos),
                    "(" + h.ToString() + ", " + d.ToString() + ")"
                );
                // Draw the hex outline
                for (int i = 0; i < 6; i++)
                {
                    Gizmos.DrawLine(
                        Unity_LocalToWorld(HexMetrics.corners[i] + localPos),
                        Unity_LocalToWorld(HexMetrics.corners[i + 1] + localPos)
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
