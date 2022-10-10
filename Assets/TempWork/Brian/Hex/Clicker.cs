using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    [SerializeField] private HexGrid grid;

    [SerializeField] private Sprite toBuild;

    [SerializeField] private Sprite red;
    [SerializeField] private Sprite blue;
    [SerializeField] private Sprite green;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            toBuild = red;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            toBuild = green;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            toBuild = blue;
        }

        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            HexCell cell;
            if (hit.collider.TryGetComponent<HexCell>(out cell))
            {
                cell.spriteRenderer.sprite = toBuild;
            }
        }
    }
}
