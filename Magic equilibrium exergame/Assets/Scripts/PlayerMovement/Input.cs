using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    public static Input instance;
    public PlayerMovement playerMovement;

    public float modifierX;
    public float modifierZ;

    [Range(-1000f, 1000f)]
    public float preX;

    [Range(-1000f, 1000f)]
    public float preZ;

    [Range(-1000f, 1000f)]
    public float inputX;

    [Range(-1000f, 1000f)]
    public float inputZ;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        preX = inputX;
        preZ = inputZ;
    }

    
    void Update()
    {
        float tempX = inputX - preX;
        float tempZ = inputZ - preZ;

        preX = inputX;
        preZ = inputZ;

        // Modify player's speed based on input and other modifiers beeing applied
        playerMovement.UpdateSpeed(tempX, tempZ);
    }
}
