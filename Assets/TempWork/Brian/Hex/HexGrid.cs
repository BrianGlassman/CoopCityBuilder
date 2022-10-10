using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class HexGrid : MonoBehaviour
{
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
