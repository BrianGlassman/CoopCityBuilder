using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : Mirror.NetworkBehaviour
{
    [SerializeField] public HexGrid grid;

    // TODO make readonly using this: https://answers.unity.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html
    [SerializeField] private KeyCode toBuild;

    private void Update()
    {
        foreach (KeyValuePair<KeyCode, Sprite> pair in BuildPairs.inst.buildPairs)
        {
            if (Input.GetKeyDown(pair.Key))
            {
                toBuild = pair.Key;
            }
        }

        if (Input.GetMouseButton(0))
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        if (toBuild == KeyCode.None)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            HexCell cell;
            if (hit.collider.TryGetComponent<HexCell>(out cell))
            {
                CmdSetCellModel(cell.H, cell.D, toBuild);
            }
        }
    }

    [Mirror.Command(requiresAuthority = false)]
    public void CmdSetCellModel(int H, int D, KeyCode toBuild)
    {
        grid.RpcSetCell(H, D, toBuild);
    }
}
