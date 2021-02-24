﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    private GridLayout gridLayout;
    private Tilemap tilemap;
    public Vector2Int groundDir;
    private Vector3Int cellPosition;

    private void Awake()
    {
        Physics2D.queriesStartInColliders = false; // Disables objects from raycasting themselves
    }

    // Start is called before the first frame update
    void Start()
    {
        // Events
        EventManager.what.pressW += pressW;
        EventManager.what.pressA += pressA;
        EventManager.what.pressS += pressS;
        EventManager.what.pressD += pressD;
        EventManager.what.prune += prune;
        EventManager.what.updateDirection += updateDirection;
        EventManager.what.updateCellPosition += updateCellPosition;

        // Initialize
        groundDir = Vector2Int.down;
        gridLayout = GameObject.Find("Grid").GetComponent<GridLayout>();
        tilemap = gridLayout.GetComponentInChildren<Tilemap>();

        updateCellPosition();
    }

    #region Event Triggers
    private void pressW()
    {
        Slide(Vector2Int.up);
    }
    private void pressA()
    {
        Slide(Vector2Int.left);
    }
    private void pressS()
    {
        Slide(Vector2Int.down);
    }
    private void pressD()
    {
        Slide(Vector2Int.right);
    }
    private void prune()
    {
        transform.DetachChildren();
    }
    private void updateDirection()
    {
        if (groundDir == Vector2Int.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        if (groundDir == Vector2Int.left)
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        if (groundDir == Vector2Int.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (groundDir == Vector2Int.right)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
    #endregion

    // Slide Method
    private void Slide(Vector2Int slideDir)
    {
        groundDir = slideDir;

        // Death Check
        if (!Physics2D.Raycast(transform.position, slideDir, 100)) // Shoot into the void
        {
            Debug.Log("YOU DIEDED");
            return;
        }

        // Stepping Raycast
        int rayLength = 1;
        while (!Physics2D.Raycast(transform.position, slideDir, rayLength))
        {
            rayLength++;
        }
        int slideDist = rayLength - 1;

        // Check tile
        TileBase tile = GetTileHit(slideDir, rayLength);
        if (tile == null) // We hit a non-tile object
        {
            RaycastHit2D hit = (Physics2D.Raycast(transform.position, slideDir, rayLength));
            transform.parent = hit.transform;
            transform.position = hit.transform.position - (Vector3Int)slideDir;

        } else // We hit a tile
        {
            if (gameObject.tag == "Player")
            {
                if (tile.name == "Spikes") // Spike check
                {
                    EventManager.what.RestartLevel();
                }
                if (tile.name == "Goal") // Goal check
                {
                    EventManager.what.YouWonded();
                }
            }
            transform.position = transform.position + (Vector3Int)slideDir * slideDist;
        }
    }

    #region Smaller Functions
    // Turn world position to grid position
    public void updateCellPosition()
    {
        cellPosition = gridLayout.WorldToCell(transform.position);
    }

    // Returns tile in direction tileDir, distance tileDist, relative to You
    private TileBase GetTileHit(Vector2Int tileDir, int tileDist)
    {
        TileBase tile = tilemap.GetTile(cellPosition + (Vector3Int)tileDir * tileDist);
        return tile;
    }
    #endregion
}
