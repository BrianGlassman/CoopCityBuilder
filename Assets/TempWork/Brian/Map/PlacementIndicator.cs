using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Heavily inspired by https://gamedevacademy.org/unity-city-building-game-tutorial/#Placing_Buildings

public class PlacementIndicator : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private CustomGrid grid;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile tile;

    [SerializeField] private Tile red;
    [SerializeField] private Tile blue;
    [SerializeField] private Tile green;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            tile = red;
            spriteRenderer.sprite = tile.sprite;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            tile = green;
            spriteRenderer.sprite = tile.sprite;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            tile = blue;
            spriteRenderer.sprite = tile.sprite;
        }

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.y = 0;
        var cell = grid.grid.WorldToCell(mousePos);
        indicator.transform.position = grid.grid.CellToWorld(cell);

        if (Input.GetMouseButtonDown(0))
        {
            tilemap.SetTile(cell, tile);
        }

        /* Original way of doing it, not using right now, might come back later
        if (indicator.transform.hasChanged)
        {
            grid.SnapToGrid(indicator.transform);
        }
        */
    }
}
