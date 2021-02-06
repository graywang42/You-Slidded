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
            Debug.Log("W");
            
            int hit = GetRaycastHit();

            transform.position = transform.position + Vector3.up * hit;
                        
            // transform.position = hit.transform.position;
            
        }
    }

    #region My Functions

    private int GetRaycastHit()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1);
        int count = 0;
        while (hit.collider == null)
        {
            count++;
            Debug.Log("We've looped " + count + " time(s)");
            hit = Physics2D.Raycast(transform.position, Vector2.up, 1 * count);
            Debug.Log("Hit " + hit.collider);
        }
        
        return count - 1;
        
        /*
        RaycastHit2D hit = Physics2D.Raycast(transform.position, UnityEngine.Vector3.up, 10);
        Debug.Log("Hit " + hit.collider + " at " + hit.collider.transform.position);
        Tilemap tilemaphit = hit.collider.GetComponent<Tilemap>();
        Grid gridhit = tilemaphit.layoutGrid;
        Debug.Log("Got Tilemap " + tilemaphit + "Got Grid " + gridhit);
        Vector3Int cellPosition = gridhit.WorldToCell(transform.position);
        Debug.Log("Cell Position " + cellPosition);

        return hit;
        */
    }

    #endregion

}
