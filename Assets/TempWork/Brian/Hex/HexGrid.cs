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

    public static int width = 6;
    public static int height = 6;

    private HexMesh hexMesh;

    private HexCell[] cells = new HexCell[height * width];

    [SerializeField] HexCell hexCellPrefab;

    private void Awake()
    {
        if (Application.isPlaying)
        {
            for (int d = 0, i = 0; d < height; d++)
            {
                for (int h = 0; h < width; h++)
                {
                    CreateCell(h, d, i);
                    i++;
                }
            }

            hexMesh = GetComponentInChildren<HexMesh>();
        }
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            hexMesh.Triangulate(cells);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int d = 0; d < height; d++)
        {
            for (int h = 0; h < width; h++)
            {
                for (int i = 0; i < 6; i++)
                {
                    var worldPos = HexMetrics.CellToWorld(h, d);
                    Gizmos.DrawLine(HexMetrics.corners[i] + worldPos, HexMetrics.corners[i + 1] + worldPos);
                }
            }
        }
    }

    private void CreateCell(int cellH, int cellD, int cellIdx, Sprite sprite = null)
    {
        var cell = Instantiate<HexCell>(hexCellPrefab, transform);
        cell.H = cellH;
        cell.D = cellD;
        cell.spriteRenderer.sprite = sprite;
        cell.transform.SetPositionAndRotation(cell.CellToWorld(), Quaternion.identity);
        cells[cellIdx] = cell;
    }
}
