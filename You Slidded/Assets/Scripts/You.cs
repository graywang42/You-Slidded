using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class You : MonoBehaviour
{
    // Setup stuff like useful variables
    #region Setup stuff
    private Vector2Int groundDir;
    private GridLayout gridLayout;
    private Tilemap tilemap;
    private Vector3Int cellPosition;
    #endregion

    // Setup more stuff like disable raycast hitting itself
    private void Awake()
    {
        Physics2D.queriesStartInColliders = false; // Disables objects from raycasting themselves
    }

    // Initialize stuff, get relavent components
    void Start()
    {
        groundDir = Vector2Int.down;
        gridLayout = GameObject.Find("Grid").GetComponent<GridLayout>();
        tilemap = gridLayout.GetComponentInChildren<Tilemap>();
        UpdateCellPosition();
    }

    // Keyboard Inputs
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            Slide(Vector2Int.up);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
            Slide(Vector2Int.left);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Slide(Vector2Int.down);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            Slide(Vector2Int.right);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    #region Main Mechanics

    // SLIDE MECHANIC
    // You slides in input direction until You hits a wall
    private void Slide(Vector2Int slideDir)
    {
        groundDir = slideDir;
        if (!Physics2D.Raycast(transform.position, slideDir, 100)) // Shoot into the void
        {
            Debug.Log("YOU DIEDED");
            return;
        }
        int rayLength = 1;
        while (!Physics2D.Raycast(transform.position, slideDir, rayLength))
        {
            rayLength++;
        }
        int slideDist = rayLength - 1;
        transform.position = transform.position + (Vector3Int)slideDir * slideDist;
        UpdateCellPosition();
    }

    // JUMP MECHANIC
    // You jumps with his feet
    private void Jump()
    {
        if (Physics2D.Raycast(transform.position, groundDir, 1) && !Physics2D.Raycast(transform.position, -groundDir, 1))
        {
            TileBase tile = GetTileHit(groundDir, 1);
            Debug.Log(tile.name);
            if (tile.name != "AntiJump")
            {
                transform.position = transform.position - (Vector3Int)groundDir;
            }
        }
        UpdateCellPosition();
    }

    private void UpdateCellPosition()
    {
        cellPosition = gridLayout.WorldToCell(transform.position);
    }

    private TileBase GetTileHit(Vector2Int tileDir, int tileDist)
    {
        TileBase tile = tilemap.GetTile(cellPosition + (Vector3Int)tileDir * tileDist);
        return tile;
    }
    
    #endregion

}
