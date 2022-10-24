using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    [SerializeField] private HexGrid grid;

    // TODO make readonly using this: https://answers.unity.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html
    [SerializeField] private Sprite toBuild;

    [System.Serializable]
    class BuildPair
    {
        public KeyCode key;
        public Sprite building;
    }
    [SerializeField] List<BuildPair> buildPairs;

    private void Update()
    {
        foreach (BuildPair pair in buildPairs)
        {
            if (Input.GetKeyDown(pair.key))
            {
                toBuild = pair.building;
            }
        }

        if (Input.GetMouseButton(0))
        {
            HandleClick();
        }
    }

    private void HandleClick()
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
