using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    public static Input instance;
    public PlayerMovement playerMovement;

    public float modifierX;
    public float modifierZ;

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

    
    void Update()
    {
        // Modify player's speed based on input and other modifiers beeing applied
        playerMovement.UpdateSpeed(inputX + modifierX, inputZ + modifierZ);
    }
}
