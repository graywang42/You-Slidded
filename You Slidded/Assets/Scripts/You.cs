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
    public bool hasSlidded;
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
        hasSlidded = false;
        UpdateCellPosition();
    }

    // Keyboard Inputs
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // transform.rotation = Quaternion.Euler(0, 0, 180);
            Slide(Vector2Int.up);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            // transform.rotation = Quaternion.Euler(0, 0, 270);
            Slide(Vector2Int.left);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            // transform.rotation = Quaternion.Euler(0, 0, 0);
            Slide(Vector2Int.down);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            // transform.rotation = Quaternion.Euler(0, 0, 90);
            Slide(Vector2Int.right);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    #region Main Mechanics

    /*    // NEW REFACTORED SLIDE MECHANIC
        private void Slide(Vector2Int slideDir)
        {
            Debug.Log("Sliding " + transform);
            UpdateCellPosition();
            groundDir = slideDir;

            // Stepping Raycast
            int rayLength = 1;
            while (!Physics2D.Raycast(transform.position, slideDir, rayLength))
            {
                rayLength++;
                Debug.Log(rayLength);
            }
            int slideDist = rayLength - 1;

            // Check tile
            TileBase tile = GetTileHit(slideDir, rayLength);
            if (tile == null) // We hit a non-tile object
            {
                // Get gameobject hit
                RaycastHit2D hit = (Physics2D.Raycast(transform.position, slideDir, rayLength));
                Debug.Log("We hit " + hit.transform);
            }

            transform.position = transform.position + (Vector3Int)slideDir * slideDist;

            UpdateCellPosition();
        }*/

    // REFACTORED SLIDE MECHANIC
    private void Slide(Vector2Int slideDir)
    {
        // Basic stuff
        hasSlidded = false;
        Debug.Log("Sliding " + transform);
        UpdateCellPosition();
        groundDir = slideDir;

        // Death Check
        if (!Physics2D.Raycast(transform.position, slideDir, 100)) // Shoot into the void
        {
            Debug.Log("YOU DIEDED");
            return;
        }

        // Check 1 grid at a time in input direction
        int rayLength = 1;
        while (!Physics2D.Raycast(transform.position, slideDir, rayLength))
        {
            rayLength++;
        }
        int slideDist = rayLength - 1;

        // Prune check
        if (transform.childCount > 0) // If child !hasSlidded, prune
        {
            You[] comps = GetComponentsInChildren<You>();
            foreach (You comp in comps)
            {
                if (comp.gameObject.transform.parent != null)
                {
                    Debug.Log(comp.name + " hasSlidded = " + comp.hasSlidded);
                    if (!comp.hasSlidded)
                    {
                        Prune();
                        Debug.Log("Pruning");
                    }
                }
            }
        }

        // Check tile
        TileBase tile = GetTileHit(slideDir, rayLength);


        if (tile != null) // We hit a real tile
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
        else // We hit a null space, aka a gameobject
        {
            RaycastHit2D hit = (Physics2D.Raycast(transform.position, slideDir, rayLength));
            Debug.Log("Hit " + hit.transform);
            // Parent logic
            // If the parent hasn't moved yet, we want to move with it
            if (!hit.transform.gameObject.GetComponent<You>().hasSlidded)
            {
                Debug.Log("Attaching to " + hit.transform);
                transform.parent = hit.transform;
                transform.position = hit.transform.position - (Vector3Int)slideDir;
            }
            else
            {
                transform.position = hit.transform.position - (Vector3Int)slideDir;
            }
        }
        UpdateCellPosition();
        hasSlidded = true;
    }

    // JUMP MECHANIC
    // You jumps with his feet
    // MECHANIC NEEDS REVISION TO WORK WITH BLOCKS
    private void Jump()
    {
        UpdateCellPosition();
        if (Physics2D.Raycast(transform.position, groundDir, 1) && !Physics2D.Raycast(transform.position, -groundDir, 1))
        {
            TileBase tile = GetTileHit(groundDir, 1);
            if (tile.name != "AntiJump")
            {
                transform.position = transform.position - (Vector3Int)groundDir;
            }
        }
        UpdateCellPosition();
    }

    #endregion

    #region Smaller Functions

    // Turn world position to grid position
    private void UpdateCellPosition()
    {
        cellPosition = gridLayout.WorldToCell(transform.position);
    }

    private void Prune()
    {
        // transform.parent = null;
        transform.DetachChildren();
        // Debug.Log("Prune");
    }

    // Returns tile in direction tileDir, distance tileDist, relative to You
    private TileBase GetTileHit(Vector2Int tileDir, int tileDist)
    {
        TileBase tile = tilemap.GetTile(cellPosition + (Vector3Int)tileDir * tileDist);
        return tile;
    }
    
    #endregion

}
