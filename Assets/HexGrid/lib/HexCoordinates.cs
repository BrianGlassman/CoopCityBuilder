using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for storing hex coordinates and converting between other coordinate systems
/// </summary>
[System.Serializable]
public struct HexCoordinates
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
    /// <summary>
    /// Convenience function to convert a Vector3Int to HexCoordinates
    /// </summary>
    /// <param name="coordinates">Coordinates of the form {H, D, _}</param>
    /// <returns></returns>
    public static HexCoordinates FromVector3Int(Vector3Int coordinates)
    {
        return new HexCoordinates(coordinates[0], coordinates[1]);
    }

    public override string ToString()
    {
        return "(" + H.ToString() + ", " + D.ToString() + ")";
    }
}