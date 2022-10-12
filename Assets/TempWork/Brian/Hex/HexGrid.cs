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
    /*
    //----------------------
    // Unity-like interface
    //----------------------
    // Grid interface - properties
    // none, all inherited or from GridLayout interface
    // Grid interface - public methods
    /// <summary>
    /// Get the logical center coordinate of a grid cell in local space.
    /// </summary>
    /// <param name="position">The cell coordinates</param>
    /// <returns></returns>
    public Vector3 GetCellCenterLocal(Vector3Int position)
    {
        // FIXME
        return Vector3.zero;
    }
    public Vector3 GetCellCenterWorld(Vector3Int position);
    // Grid interface - static methods
    public static Vector3 InverseSwizzle(GridLayout.CellSwizzle swizzle, Vector3 position);
    public static Vector3 Swizzle(GridLayout.CellSwizzle swizzle, Vector3 position);
    // GridLayout interface - properties
    public Vector3 cellGap;
    public GridLayout.CellLayout cellLayout;
    public Vector3 cellSize;
    public GridLayout.CellSwizzle cellSwizzle;
    // GridLayout interface - public methods
    public Vector3 CellToLocal(Vector3Int cellPosition);
    public Vector3 CellToLocalInterpolated(Vector3 cellPosition);
    public Vector3 CellToWorld(Vector3Int cellPosition);
    public Bounds GetBoundsLocal(Vector3Int cellPosition);
    public Vector3 GetLayoutCellCenter();
    public Vector3Int LocalToCell(Vector3 localPosition);
    public Vector3 LocalToCellInterpolated(Vector3 localPosition);
    public Vector3 LocalToWorld(Vector3 localPosition);
    public Vector3Int WorldToCell(Vector3 worldPosition);
    public Vector3 WorldToLocal(Vector3 worldPosition);
    //----------------------
    */

    /// <summary>
    /// A class for storing hex coordinates and converting between other coordinate systems
    /// </summary>
    [System.Serializable]
    private struct HexCoordinates
    {
        // Coordinates are immutable once created because the properties are read-only
        public int H { get; private set; }
        public int D { get; private set; }

        /*
         * Constructors
         */
        /// <summary>
        /// Create a HexCoordinates object using our coordinate system.
        /// </summary>
        /// <param name="H">Horizontal measurement, increases moving to the right</param>
        /// <param name="D">Diagonal measurement, increases moving to the top-left</param>
        public HexCoordinates(int H, int D)
        {
            this.H = H;
            this.D = D;
        }
        /// <summary>
        /// Converts the Axial system from Redblog Games to the system we're using.
        /// https://www.redblobgames.com/grids/hexagons/#coordinates
        /// </summary>
        /// <param name="q">Horizontal measurement, increases moving to the right</param>
        /// <param name="r">Diagonal measurement, increases moving to the bottom-right</param>
        /// <returns></returns>
        public static HexCoordinates FromRedblogAxialCoordinates(int q, int r)
        {
            return new HexCoordinates(q, -r);
        }

        public override string ToString()
        {
            return "(" + H.ToString() + ", " + D.ToString() + ")";
        }
    }

    public static int width = 6;
    public static int height = 6;

    private HexMesh hexMesh;

    private Dictionary<HexCoordinates, HexCell> cells = new Dictionary<HexCoordinates, HexCell>();
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
                var worldPos = HexMetrics.CellToWorld(h, d);
                // Label the coordinates
                UnityEditor.Handles.Label(worldPos, "(" + h.ToString() + ", " + d.ToString() + ")");
                // Draw the hex outline
                for (int i = 0; i < 6; i++)
                {
                    Gizmos.DrawLine(HexMetrics.corners[i] + worldPos, HexMetrics.corners[i + 1] + worldPos);
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
        cell.transform.SetPositionAndRotation(cell.CellToWorld(), Quaternion.identity);
        cells[new HexCoordinates(cellH, cellD)] = cell;
    }
}
