using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{

    private GridLayout gridLayout;
    private Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        gridLayout = GameObject.Find("Grid").GetComponent<GridLayout>();
        tilemap = gridLayout.GetComponentInChildren<Tilemap>();
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
                    Debug.Log("Spawn You at " + position); // Instantiate You here
                }
                if (tile.name == "Block")
                {
                    Debug.Log("Spawn Block at " + position); // Instantiate Blocks here
                }
            }
        }
    }
}
