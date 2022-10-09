using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public static int width = 6;
    public static int height = 6;

    private HexMesh hexMesh;

    private HexCell[] cells = new HexCell[height * width];

    private void Awake()
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        foreach (HexCell cell in cells)
        {
            if (cell is null) continue;
            
            for (int i = 0; i < 6; i++)
            {
                var worldPos = cell.CellToWorld();
                Gizmos.DrawLine(HexMetrics.corners[i] + worldPos, HexMetrics.corners[i + 1] + worldPos);
            }
        }
    }

    private void CreateCell(int cellH, int cellD, int cellIdx)
    {
        HexCell cell = new HexCell(cellH, cellD);
        cells[cellIdx] = cell;
    }
}
