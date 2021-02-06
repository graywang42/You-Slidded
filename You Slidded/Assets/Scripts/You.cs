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
        if (Input.GetKeyDown(KeyCode.W))
        {
            int slideDist = GetSlideDist(Vector2.up);
            transform.position = transform.position + Vector3.up * slideDist;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            int slideDist = GetSlideDist(Vector2.left);
            transform.position = transform.position + Vector3.left * slideDist;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            int slideDist = GetSlideDist(Vector2.down);
            transform.position = transform.position + Vector3.down * slideDist;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            int slideDist = GetSlideDist(Vector2.right);
            transform.position = transform.position + Vector3.right * slideDist;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spacebar");
            Vector3 pushDir = Explosion();
            transform.position = transform.position + pushDir;
        }
    }

    #region My Functions

    private int GetSlideDist(Vector2 slideDir)
    { // Raycasts in input direction, returns slide distance
        int rayLength = 1;
        while (!Physics2D.Raycast(transform.position, slideDir, rayLength)) {
            rayLength++;
        }
        return rayLength - 1;
    }

    private Vector2 Explosion()
    { // Raycasts in 4 directions, returns vector of push-off direction
        int pushX = 0;
        int pushY = 0;
        if (Physics2D.Raycast(transform.position, Vector2.right, 1))
            pushX--;
        if (Physics2D.Raycast(transform.position, Vector2.left, 1))
            pushX++;
        if (Physics2D.Raycast(transform.position, Vector2.up, 1))
            pushY--;
        if (Physics2D.Raycast(transform.position, Vector2.down, 1))
            pushY++;
        Vector3 pushDir = new Vector3(pushX, pushY, 0);
        Debug.Log("Pushing in Direction " + pushDir);
        return pushDir;
    }

    #endregion

}
