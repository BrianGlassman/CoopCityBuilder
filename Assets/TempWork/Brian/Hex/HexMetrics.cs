using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://catlikecoding.com/unity/tutorials/hex-map/part-1/

public static class HexMetrics
{
    public const float outerRadius = 1f;
    public const float innerRadius = outerRadius * 0.86602540378443864676f; // sqrt(3) / 2

    // Orient point up because wide-screen monitors encourage left-right thinking
    public static Vector3[] corners =
    {
        new Vector3(0f, 0f, outerRadius), // Top
        new Vector3(innerRadius, 0f, 0.5f*outerRadius), // Top right
        new Vector3(innerRadius, 0f, -0.5f*outerRadius), // Bottom right
        new Vector3(0f, 0f, outerRadius), // Bottom
        new Vector3(-innerRadius, 0f, -0.5f*outerRadius), // Bottom left
        new Vector3(-innerRadius, 0f, -0.5f*outerRadius), // Top left
        new Vector3(0f, 0f, outerRadius) // Top again to avoid index problems when closing the loop
    };
}
