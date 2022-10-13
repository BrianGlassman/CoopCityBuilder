using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://catlikecoding.com/unity/tutorials/hex-map/part-1/

// Coordinate system
//     H: positive to the right
//     D: positive to the diagonal top-left

/// <summary>
/// A static class to store commonly referenced values
/// </summary>
public static class HexMetrics
{
    // Distance from center to edge, the inner circle radius
    public const float edgeRadius = 0.5f;
    // Distance from center to corner, the outer circle radius
    public const float cornerRadius = edgeRadius * 2 / Utils.sqrt3;

    public static Vector3 cellSize = new Vector3(2 * edgeRadius, 2 * edgeRadius, 2 * edgeRadius);

    // Orient point up because wide-screen monitors encourage left-right thinking
    public static Vector3[] corners =
    {
        new Vector3(0f, 0f, cornerRadius), // Top
        new Vector3(edgeRadius, 0f, 0.5f*cornerRadius), // Top right
        new Vector3(edgeRadius, 0f, -0.5f*cornerRadius), // Bottom right
        new Vector3(0f, 0f, -cornerRadius), // Bottom
        new Vector3(-edgeRadius, 0f, -0.5f*cornerRadius), // Bottom left
        new Vector3(-edgeRadius, 0f, 0.5f*cornerRadius), // Top left
        new Vector3(0f, 0f, cornerRadius) // Top again to avoid index problems when closing the loop
    };
    public static Dictionary<string, Vector3> labelledCorners = new()
    {
        { "top", corners[0] },
        { "top-right", corners[1] },
        { "bottom-right", corners[2] },
        { "bottom", corners[3] },
        { "bottom-left", corners[4] },
        { "top-left", corners[5] },
    };

    public static Vector3 CellToLocal(int cellH, int cellD)
    {
        Vector3 localPos;
        localPos.x = edgeRadius * (2 * cellH - cellD);
        localPos.y = 0;
        localPos.z = edgeRadius * (cellD * Utils.sqrt3);

        return localPos;
    }
    public static Vector3 CellToLocal(HexCoordinates coords)
    {
        return CellToLocal(coords.H, coords.D);
    }
    public static Vector3 CellToLocalInterpolated(float cellH, float cellD)
    {
        Vector3 localPos;
        localPos.x = edgeRadius * (2 * cellH - cellD);
        localPos.y = 0;
        localPos.z = edgeRadius * (cellD * Utils.sqrt3);

        return localPos;
    }

    public static float[] LocalToCellInterpolated(Vector3 localPos)
    {
        // FIXME not tested at all
        // FIXMELOW There's probably a more efficient way to calculate this
        float cellD = localPos.z / (edgeRadius * Utils.sqrt3);
        float cellH = (localPos.x / edgeRadius + cellD) / 2.0f;
        float[] ans = new float[] { cellH, cellD };
        return ans;
    }
    public static int[] LocalToCell(Vector3 localPos)
    {
        // FIXME not tested at all
        var hd = LocalToCellInterpolated(localPos);
        int[] ans = new int[] { Mathf.RoundToInt(hd[0]), Mathf.RoundToInt(hd[1]) };
        return ans;
    }
}
