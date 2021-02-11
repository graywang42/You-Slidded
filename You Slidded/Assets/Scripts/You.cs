using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class You : MonoBehaviour
{

    private Vector2 groundDir;
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
        groundDir = Vector2.down;
        gridLayout = GameObject.Find("Grid").GetComponent<GridLayout>(); // Setup grid getting
        tilemap = gridLayout.GetComponentInChildren<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            Slide(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
            Slide(Vector2.left);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Slide(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            Slide(Vector2.right);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    #region My Functions

    private void Slide(Vector2 slideDir)
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
        transform.position = transform.position + (Vector3)slideDir * slideDist;
        UpdateCellPosition();
    }

    private void Jump() // Jump 1 space off of wall
    {
        if (Physics2D.Raycast(transform.position, groundDir, 1) && !Physics2D.Raycast(transform.position, -groundDir, 1))
        {
            TileBase tile = tilemap.GetTile(cellPosition + Vector3Int.down);            // WORKING FROM HERE
            Debug.Log("tile on position " + cellPosition + " is " + tile);              // the code is messy, but it reads the tile sprite
            // if we hit a jumpable object, jump
            transform.position = transform.position - (Vector3)groundDir;
        }
        UpdateCellPosition();
    }

    private void UpdateCellPosition()
    {
        cellPosition = gridLayout.WorldToCell(transform.position);
    }    
    
    #endregion

}
