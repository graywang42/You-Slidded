using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.VFX;

public class SceneMan : MonoBehaviour
{

    private GridLayout grid;
    private Tilemap tilemap;
    public GameObject You;
    public GameObject Block;
    private bool hasWonded;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.what.restart += RestartLevel;
        EventManager.what.youWonded += YouWonded;

        grid = GameObject.Find("Grid").GetComponent<GridLayout>();
        tilemap = grid.GetComponentInChildren<Tilemap>();
        hasWonded = false;
        SpawnObjects();
    }

    private void Update()
    {
        if (hasWonded && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
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

    private void RestartLevel()
    {
        // Play somee kind of restart/death animation
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void YouWonded()
    {
        Debug.Log("YOU WONDED - Press Space to advance to the next level");
        hasWonded = true;
        // Advance to the next scene or return to some kind of level select hub
    }
}
