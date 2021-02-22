using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.VFX;

public class SceneManager : MonoBehaviour
{

    private GridLayout grid;
    private Tilemap tilemap;
    public GameObject You;
    public GameObject Block;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<GridLayout>();
        tilemap = grid.GetComponentInChildren<Tilemap>();
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        Vector3 offset = new Vector3(0.5f, 0.5f, 0);
        foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position))
            {
                TileBase tile = tilemap.GetTile(position);
                if (tile.name == "You Spawn")
                {
                    tilemap.SetTile(position, null);
                    Instantiate(You, position + offset, Quaternion.identity);
                }
                if (tile.name == "Block")
                {
                    tilemap.SetTile(position, null);
                    Instantiate(Block, position + offset, Quaternion.identity);
                }
            }
        }
    }
}
