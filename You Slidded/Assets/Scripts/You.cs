using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class You : MonoBehaviour
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
        groundDir = Vector2Int.down;
        gridLayout = GameObject.Find("Grid").GetComponent<GridLayout>(); // Setup grid getting
        tilemap = gridLayout.GetComponentInChildren<Tilemap>();
    }

    // Update is called once per frame
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

    #region My Functions

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

    private void Jump() // Jump 1 space off of wall
    {
        if (Physics2D.Raycast(transform.position, groundDir, 1) && !Physics2D.Raycast(transform.position, -groundDir, 1))
        {
            TileBase tile = tilemap.GetTile(cellPosition + (Vector3Int)groundDir);
            Debug.Log("tile on position " + cellPosition + " is " + tile);
            // if we hit a jumpable object, jump
            transform.position = transform.position - (Vector3Int)groundDir;
        }
        UpdateCellPosition();
    }

    private void UpdateCellPosition()
    {
        cellPosition = gridLayout.WorldToCell(transform.position);
    }    
    
    #endregion

}
