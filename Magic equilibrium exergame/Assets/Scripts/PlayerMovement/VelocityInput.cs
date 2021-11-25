using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityInput : MonoBehaviour
{
    public static VelocityInput instance;
    public PlayerVelocity playerVelocity;

    public float modifierZ;
    public float modifierX;

    [Range(-5f, 5f)]
    public float inputX;

    [Range(-5f, 5f)]
    public float inputZ;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        modifierX = 0f;
        modifierZ = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        // Modify player's speed based on input and other modifiers beeing applied
        playerVelocity.UpdateSpeed(inputX + modifierX, inputZ + modifierZ);
    }
}
