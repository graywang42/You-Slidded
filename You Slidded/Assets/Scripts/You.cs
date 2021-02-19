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

    // REFACTORED SLIDE MECHANIC
    private void Slide(Vector2Int slideDir)
    {
        // Basic stuff
        UpdateCellPosition();
        Prune();
        groundDir = slideDir;

        // Death mechanic
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

        // Check tile
        TileBase tile = GetTileHit(slideDir, rayLength);
        if (tile != null) // We hit a real tile
        {
            if (tile.name == "Spikes")
            {
                Debug.Log("YOU DIEDED");
            }
            if (tile.name == "Goal")
            {
                Debug.Log("YOU WONDED");
            }
            transform.position = transform.position + (Vector3Int)slideDir * slideDist;
        } else // We hit a null space, aka a gameobject
        {
            // Attach to hit as child
            RaycastHit2D hit = (Physics2D.Raycast(transform.position, slideDir, rayLength));
            transform.parent = hit.transform;
            transform.position = hit.transform.position - (Vector3Int)slideDir;
        }
        UpdateCellPosition();
    }

   /* // SLIDE MECHANIC
    // You slides in input direction until You hits a wall
    private void Slide(Vector2Int slideDir)
    {
        Debug.Log("Sliding " + transform);
        // Prune Children method
        UpdateCellPosition();
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
            // Debug.Log("Raylength is " + rayLength);
        }

        // NOTE TO SELF
        *//* Idk what's going on, BUT the transforms are strange. The transform of a child is relative to its parent, so we see
         * whole numbers for transform position even though all grids are on half numbers.
         * This seems to be where the glitch is kind of coming from (not sure)
         * A quick fix seems to be by using the Raycast that attaches the child to the parent. For some reason, it returns the
         * actual location of the parent's transform, so that could work. Totally not sure why the looping raycast doesn't work.
         * Honestly, this whole part of the code is pretty janky.
        *//*

        // Slide to location
        int slideDist = rayLength - 1;
        transform.position = transform.position + (Vector3Int)slideDir * slideDist;
        Debug.Log("New Position " + transform.position);

        TileBase tile = GetTileHit(slideDir, rayLength);

        // Make parent child relationship
        if (tile == null) // We (as far as I know) hit an object, like You or Block
        {
            RaycastHit2D hit = (Physics2D.Raycast(transform.position, slideDir, rayLength));
            Debug.Log("Hit " + hit.transform + " at " + hit.transform.position);
            transform.parent = hit.transform;
        }
        if (tile != null) // We hit a real tile
        {
            if (tile.name == "Spikes")
            {
                Debug.Log("YOU DIEDED");
            }
            if (tile.name == "Goal")
            {
                Debug.Log("YOU WONDED");
            }
        }
        UpdateCellPosition();
    }*/

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
