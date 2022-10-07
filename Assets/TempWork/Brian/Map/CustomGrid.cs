using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    [SerializeField] private Grid grid;

    /// <summary>
    /// Snaps the target to the nearest grid location
    /// </summary>
    /// <param name="target"></param>
    /// <param name="inPlane">If true, snap to the grid plane</param>
    public void SnapToGrid(Transform target, bool inPlane = true)
    {
        var cell = grid.WorldToCell(target.position);
        target.position = grid.CellToWorld(cell);

        if (inPlane)
        {
            target.position = new Vector3(target.position.x, grid.transform.position.y, target.position.z);
        }
    }
}
