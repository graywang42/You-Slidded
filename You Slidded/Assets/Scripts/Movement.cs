using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    private Vector2Int groundDir;
    private GridLayout gridLayout;
    private Tilemap tilemap;
    private Vector3Int cellPosition;
    public bool hasSlidded;

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

        // Initialize
        groundDir = Vector2Int.down;
        gridLayout = GameObject.Find("Grid").GetComponent<GridLayout>();
        tilemap = gridLayout.GetComponentInChildren<Tilemap>();
        hasSlidded = false;
        UpdateCellPosition();
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
        Debug.Log("Prune test");
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

    // Update is called once per frame
    void Update()
    {

    }

    // Slide Method
    private void Slide(Vector2Int slideDir)
    {
        Debug.Log("Sliding " + transform);
        UpdateCellPosition();
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
            // Debug.Log(rayLength);
        }
        int slideDist = rayLength - 1;

        // Check tile
        TileBase tile = GetTileHit(slideDir, rayLength);
        if (tile == null) // We hit a non-tile object
        {
            // Get gameobject hit
            RaycastHit2D hit = (Physics2D.Raycast(transform.position, slideDir, rayLength));
            Debug.Log("We hit tile " + hit.transform);
            transform.parent = hit.transform;
            transform.position = hit.transform.position - (Vector3Int)slideDir;

        } else // We hit a tile
        {
            Debug.Log("Hit tile");
            if (tile.name == "Spikes")
            {
                Debug.Log("YOU DIEDED");
            }
            if (tile.name == "Goal")
            {
                Debug.Log("YOU WONDED");
            }
            transform.position = transform.position + (Vector3Int)slideDir * slideDist;
        }

        UpdateCellPosition();
    }

    #region Smaller Functions
    // Turn world position to grid position
    private void UpdateCellPosition()
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
