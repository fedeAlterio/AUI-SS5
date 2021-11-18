using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    public static Input instance;
    public PlayerMovement playerMovement;

    [Range(-1f, 1f)]
    public float inputX;

    [Range(-1f, 1f)]
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
