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

    // JUMPDED ASSUMES WE HAVE ONE PLAYER OBJECT
    // There would be bugs if a player object jumps off another player object (both should jump? weird behavior)
    // Also, if a player object tries to push another player object by jumping into it, it will not detect the player because its tag is not "Block"
    private void Jumpded()
    {
        // Get direction of ground from Movement script
        Movement movement = transform.GetComponent<Movement>();
        groundDir = movement.groundDir;
        bool isGrounded = false;
        int iteratingCenter = 0;

        // Check if we're grounded
        // Outstanding Bug: If you begin on an antijump block, you can jump off of it as your very first input.
        if (Physics2D.Raycast(transform.position, groundDir, 1))
        {
            TileBase tile = GetTileHit(groundDir, 1);
            if (tile == null || tile.name != "AntiJump")
            {
                isGrounded = true;
            }
        }

        if (isGrounded)
        {
            RaycastHit2D hit = (Physics2D.Raycast(transform.position, -groundDir, 1)); // Check space above
            while (hit.transform != null && hit.transform.gameObject.tag == "Block") // There is a block above us
            {
                Debug.Log("Attach block as child");
                // Attach hit as child
                hit.transform.parent = transform;
                iteratingCenter++;
                hit = (Physics2D.Raycast(transform.position - ((Vector3Int)groundDir * iteratingCenter), -groundDir, 1));
            }
            if (hit.transform == null) // We end at an empty tile... or there's an object there that isn't tagged with Block
            {
                transform.position = transform.position - (Vector3Int)groundDir;
            } // else, we ended at a wall so we cannot jump
        }
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
