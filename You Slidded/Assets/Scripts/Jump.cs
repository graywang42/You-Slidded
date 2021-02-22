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

    // Start is called before the first frame update
    void Start()
    {
        EventManager.what.pressSpace += pressSpace;

        gridLayout = GameObject.Find("Grid").GetComponent<GridLayout>();
        tilemap = gridLayout.GetComponentInChildren<Tilemap>();
        // UpdateCellPosition();
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
        UpdateCellPosition();
        Movement movement = transform.GetComponent<Movement>();
        groundDir = movement.groundDir;
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

    // Turn world position to grid position
    public void UpdateCellPosition()
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
