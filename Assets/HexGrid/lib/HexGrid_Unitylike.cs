using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A wrapper to allow calling a HexGrid as if it were a Unity Grid object
public partial class HexGrid : Mirror.NetworkBehaviour
{
    // ##### Grid interface - public methods #####
    /// <summary>
    /// Get the logical center coordinate of a grid cell in local space.
    /// </summary>
    /// <param name="position">The cell coordinates</param>
    /// <returns></returns>
    public Vector3 _Unity_GetCellCenterLocal(Vector3Int position)
    {
        var coords = HexCoordinates.FromVector3Int(position);
        return HexMetrics.CellToLocal(coords);
    }
    /// <summary>
    /// Get the logical center coordinate of a grid cell in world space.
    /// </summary>
    /// <param name="position">The cell coordinates</param>
    /// <returns></returns>
    public Vector3 _Unity_GetCellCenterWorld(Vector3Int position)
    {
        var coords = HexCoordinates.FromVector3Int(position);
        var local = HexMetrics.CellToLocal(coords);
        return _Unity_LocalToWorld(local);
    }
    // ##### Grid interface - static methods #####
    // TODO public static Vector3 InverseSwizzle(GridLayout.CellSwizzle swizzle, Vector3 position);
    // TODO public static Vector3 Swizzle(GridLayout.CellSwizzle swizzle, Vector3 position);
    // ##### GridLayout interface - properties #####
    /// <summary>
    /// The size of the gap between each cell in the Grid.
    /// Only non-zero for isometric grids.
    /// </summary>
    [HideInInspector] public Vector3 _Unity_cellGap = Vector3.zero;
    /// <summary>
    /// The layout of the GridLayout.
    /// The layout determines the conversion of positions from cell space to local space and vice versa.
    /// </summary>
    [HideInInspector] public GridLayout.CellLayout _Unity_cellLayout = GridLayout.CellLayout.Hexagon;
    /// <summary>
    /// The size of each cell in the Grid
    /// </summary>
    [HideInInspector] public Vector3 _Unity_cellSize { get { return HexMetrics.cellSize; } }
    /// <summary>
    /// The cell swizzle for the grid
    /// </summary>
    [HideInInspector]
    public GridLayout.CellSwizzle _Unity_cellSwizzle()
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
    public Vector3 _Unity_CellToLocal(Vector3Int cellPosition) { return _Unity_GetCellCenterLocal(cellPosition); }
    /// <summary>
    /// Converts an interpolated cell position in floats to local position space.
    /// </summary>
    /// <param name="cellPosition">Interpolated cell position to convert</param>
    /// <returns></returns>
    public Vector3 _Unity_CellToLocalInterpolated(Vector3 cellPosition)
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
    public Vector3 _Unity_CellToWorld(Vector3Int cellPosition) { return _Unity_GetCellCenterWorld(cellPosition); }
    // TODO public Bounds GetBoundsLocal(Vector3Int cellPosition);
    // TODO public Vector3 GetLayoutCellCenter();
    public Vector3Int _Unity_LocalToCell(Vector3 localPosition)
    {
        // FIXMELOW Not future-proofed against changes in [Vector3 --> cell coordinates] conversion
        var hd = HexMetrics.LocalToCell(localPosition);
        return new Vector3Int(hd[0], 0, hd[1]);
    }
    public Vector3 __Unity_LocalToCellInterpolated(Vector3 localPosition)
    {
        // FIXMELOW Not future-proofed against changes in [Vector3 --> cell coordinates] conversion
        var hd = HexMetrics.LocalToCellInterpolated(localPosition);
        return new Vector3(hd[0], 0, hd[1]);
    }
    public Vector3 _Unity_LocalToWorld(Vector3 localPosition)
    {
        return Utils.LocalToWorld(transform, localPosition);
    }
    public Vector3Int _Unity_WorldToCell(Vector3 worldPosition)
    {
        var localPos = _Unity_WorldToLocal(worldPosition);
        return _Unity_LocalToCell(localPos);
    }
    /// <summary>
    /// Converts a world position to local position.
    /// </summary>
    /// <param name="worldPosition">World Position to convert.</param>
    /// <returns></returns>
    public Vector3 _Unity_WorldToLocal(Vector3 worldPosition) { return Utils.WorldToLocal(transform, worldPosition); }

    //-------------------------------------------
    // Alternate inputs for Unity-like functions
    //-------------------------------------------
}
