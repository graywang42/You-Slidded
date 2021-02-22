using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Jump : MonoBehaviour
{
    private Vector2Int groundDir;
    private GridLayout gridLayout;
    private Tilemap tilemap;
    private Vector3Int cellPosition;

    private void Awake()
    {
        Physics2D.queriesStartInColliders = false; // Disables objects from raycasting themselves
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.what.pressSpace += pressSpace;
        EventManager.what.updateCellPosition += updateCellPosition;

        gridLayout = GameObject.Find("Grid").GetComponent<GridLayout>();
        tilemap = gridLayout.GetComponentInChildren<Tilemap>();
    }

    private void pressSpace()
    {
        Jumpded();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Jumpded()
    {
        // Get direction of ground from Movement script
        Movement movement = transform.GetComponent<Movement>();
        groundDir = movement.groundDir;
        int iteratingCenter = 0;
        // Need to update logic for smarter jump.
        // Shoot raycast down. If null, can't jump. If antijump, can't jump. If object, become that object's child (or just assume we will only have one jumping character)
        // While hit object, adopt hit as child, and increment ray origin. Loop ends when we have hit all objects (thus adopted) in our jumping direction.
        // Final check for valid jump. If next ray increment is null, we can jump. If next ray increment is a wall, we can't jump.

        if (Physics2D.Raycast(transform.position, groundDir, 1)) // We're not floating
        {
            TileBase tile = GetTileHit(groundDir, 1);
            if (tile.name != "AntiJump") // We're on a jumpable block
            {
                RaycastHit2D hit = (Physics2D.Raycast(transform.position, -groundDir, 1)); // Check space above
                while (hit.transform.gameObject.tag == "Block") // There is a block above us // THERES AN ERROR HERE
                {
                    // Attach object as child
                    Debug.Log("Attach block as child");
                    iteratingCenter++;
                    hit = (Physics2D.Raycast(transform.position - ((Vector3Int)groundDir * iteratingCenter), -groundDir, 1));
                }
                if (hit.transform == null) // The end of things is null
                {
                    transform.position = transform.position - (Vector3Int)groundDir;
                }
            }
        }

/*        if (Physics2D.Raycast(transform.position, groundDir, 1) && !Physics2D.Raycast(transform.position, -groundDir, 1))
        {
            TileBase tile = GetTileHit(groundDir, 1);
            if (tile.name != "AntiJump")
            {
                transform.position = transform.position - (Vector3Int)groundDir;
            }
        }*/
    }

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
}
