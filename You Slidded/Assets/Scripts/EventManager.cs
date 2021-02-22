using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager what;

    private void Awake()
    {
        what = this; // What is this for
    }

    public event Action pressW; // Create and name the action
    public event Action pressA;
    public event Action pressS;
    public event Action pressD;
    public event Action pressSpace;
    public event Action prune;
    public event Action updateDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (pressW != null) // Make sure the action is not null (I think this means it has no subscribers, which creates an error if null)
            {
                pressW(); // Trigger the event
            }
            if (prune != null)
            {
                prune();
            }
            if (updateDirection != null)
            {
                updateDirection();
            }
        } else if (Input.GetKeyDown(KeyCode.A))
        {
            if (pressA != null) // Make sure the action is not null (I think this means it has no subscribers, which creates an error if null)
            {
                pressA(); // Trigger the event
            }
            if (prune != null)
            {
                prune();
            }
            if (updateDirection != null)
            {
                updateDirection();
            }
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            if (pressS != null) // Make sure the action is not null (I think this means it has no subscribers, which creates an error if null)
            {
                pressS(); // Trigger the event
            }
            if (prune != null)
            {
                prune();
            }
            if (updateDirection != null)
            {
                updateDirection();
            }
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            if (pressD != null) // Make sure the action is not null (I think this means it has no subscribers, which creates an error if null)
            {
                pressD(); // Trigger the event
            }
            if (prune != null)
            {
                prune();
            }
            if (updateDirection != null)
            {
                updateDirection();
            }
        } else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (pressSpace != null)
            {
                pressSpace();
            }
            if (prune != null)
            {
                prune();
            }
        }
    }
}
