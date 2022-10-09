using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell
{
    // See HexMetrics for coordinate system explanation
    public int H;
    public int D;

    public HexCell(int H, int D)
    {
        this.H = H;
        this.D = D;
    }

    public Vector3 CellToWorld(int cellH, int cellD)
    {
        Vector3 worldPos;
        worldPos.x = HexMetrics.edgeRadius * (2 * cellH - cellD);
        worldPos.y = 0;
        worldPos.z = HexMetrics.edgeRadius * (cellD * HexMetrics.sqrt3);

        return worldPos;
    }
    public Vector3 CellToWorld()
    {
        return CellToWorld(H, D);
    }
}
