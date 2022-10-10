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
    public const float sqrt3 = 1.7320508075688772935274463415058723669428052538103806280558069794f;

    // Distance from center to edge, the inner circle radius
    public const float edgeRadius = 0.5f;
    // Distance from center to corner, the outer circle radius
    public const float cornerRadius = edgeRadius * 2 / sqrt3;

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

    public static Vector3 CellToWorld(int cellH, int cellD)
    {
        Vector3 worldPos;
        worldPos.x = HexMetrics.edgeRadius * (2 * cellH - cellD);
        worldPos.y = 0;
        worldPos.z = HexMetrics.edgeRadius * (cellD * HexMetrics.sqrt3);

        return worldPos;
    }
}
