using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    public static Input instance;
    public PlayerMovement playerMovement;
    public float inputX;
    public float inputZ;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.UpdateSpeed(inputX, inputZ);
    }
}
