using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{

    private GridLayout grid;
    private Tilemap tilemap;
    public GameObject You;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<GridLayout>();
        tilemap = grid.GetComponentInChildren<Tilemap>();
        SpawnObjects();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnObjects()
    {
        foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position))
            {
                TileBase tile = tilemap.GetTile(position);
                if (tile.name == "You Spawn")
                {
                    Vector3 offset = new Vector3(0.5f, 0.5f, 0);
                    Instantiate(You, position + offset, Quaternion.identity);
                }
                if (tile.name == "Block")
                {
                    Debug.Log("Spawn Block at " + position); // Instantiate Blocks here
                }
            }
        }
    }
}
