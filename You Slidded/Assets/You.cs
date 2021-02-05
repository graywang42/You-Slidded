using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class You : MonoBehaviour
{
    private void Awake()
    {
        Physics2D.queriesStartInColliders = false; // Disables objects from raycasting themselves
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) // Testing pushing up to shoot raycast up and snap to a block above
        {
            RaycastHit2D hit = GetRaycastHit();
            
            
            transform.position = hit.transform.position;
            Debug.Log("W");

        }
    }

    #region My Functions

    private RaycastHit2D GetRaycastHit()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, UnityEngine.Vector3.up, 10);

        Debug.Log("Hit " + hit.collider + " at " + hit.collider.transform.position);
        Tilemap tilemaphit = hit.collider.GetComponent<Tilemap>();
        Grid gridhit = tilemaphit.layoutGrid;
        Debug.Log("Got Tilemap " + tilemaphit + "Got Grid " + gridhit);
        Vector3Int cellPosition = gridhit.WorldToCell(transform.position);
        Debug.Log("Cell Position " + cellPosition);

        return hit;
    }

    #endregion

}
