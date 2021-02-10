using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class You : MonoBehaviour
{

    private Vector2 groundDir;

    private void Awake()
    {
        Physics2D.queriesStartInColliders = false; // Disables objects from raycasting themselves
    }

    // Start is called before the first frame update
    void Start()
    {
        groundDir = Vector2.down;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            int slideDist = GetSlideDist(Vector2.up);
            transform.position = transform.position + Vector3.up * slideDist;
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            int slideDist = GetSlideDist(Vector2.left);
            transform.position = transform.position + Vector3.left * slideDist;
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            int slideDist = GetSlideDist(Vector2.down);
            transform.position = transform.position + Vector3.down * slideDist;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            int slideDist = GetSlideDist(Vector2.right);
            transform.position = transform.position + Vector3.right * slideDist;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spacebar");
            Vector3 pushDir = Explosion();
            transform.position = transform.position + pushDir;
            Jump();
        }
    }

    #region My Functions
    
    // Can make less redundant if this function updates the object's transform.
    private int GetSlideDist(Vector2 slideDir)
    { // Raycasts in input direction, returns slide distance
        groundDir = slideDir;
        if (!Physics2D.Raycast(transform.position, slideDir, 100)) // Shoot into the void
        {
            Debug.Log("YOU DIEDED");
            return 100;
        }
        int rayLength = 1;
        while (!Physics2D.Raycast(transform.position, slideDir, rayLength)) {
            rayLength++;
        }
        return rayLength - 1;
    }

    // Will probably get rid of this method in favor of 'Jump', because Jump is more fun and intuitive
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
        if (Physics2D.Raycast(transform.position, pushDir, 1))
        {
            return new Vector3(0, 0, 0);
        }
        Debug.Log("Pushing in Direction " + pushDir);
        return pushDir;
    }

    private void Jump()
    {
        Debug.Log("Ground is in " + groundDir);
        // Shoot raycast in ground direction
        // if we hit something, check what we hit
        // if we hit a jumpable object, try to jump
        // shoot raycast in opposite ground direction
        // if the square is empty, jump
    }

    #endregion

}
