using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public Vector3 startingPosition;
    public Vector3 target;
    public const float speed = 0.5f;
    public Vector3 step;
    public Rigidbody rb;
    private bool moving;

    private void Start()
    {
        startingPosition = gameObject.transform.position;

        rb = GetComponent<Rigidbody>();

        moving = false;

        step = new Vector3(0, 0, speed);

        DeathManager.instance.playerDeathEvent.AddListener(ResetPosition);
    }

    private void Update()
    {
        if(moving)
        {
            rb.velocity = step;
        }

        // Check if the block has reached its destination, if it has then stop moving
        if(Vector3.Distance(gameObject.transform.position, target) < 0.001f)
        {
            VelocityInput.instance.modifierZ = VelocityInput.instance.modifierZ - speed;
            moving = false;
            rb.velocity = Vector3.zero;
        }
    }

    // Start moving this block when the player gets on top of it
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            VelocityInput.instance.modifierZ = VelocityInput.instance.modifierZ + speed;
            moving = true;
        }
    }

    // Reset position of the platform when the player dies
    private void ResetPosition()
    {
        moving = false;
        VelocityInput.instance.modifierZ = VelocityInput.instance.modifierZ - speed;
        gameObject.transform.position = startingPosition;
    }
}
