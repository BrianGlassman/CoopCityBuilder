using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Heavily inspired by https://gamedevacademy.org/unity-city-building-game-tutorial/#Placing_Buildings

public class PlacementIndicator : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private CustomGrid grid;

    private void Update()
    {
        if (indicator.transform.hasChanged)
        {
            grid.SnapToGrid(indicator.transform);
        }
    }
}
