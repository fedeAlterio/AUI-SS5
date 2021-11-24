using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public bool moving;
    public Vector3 startingPosition;
    public Vector3 target;
    public float speed = 0.3f;
    public float step;

    private void Start()
    {
        moving = false;
        startingPosition = gameObject.transform.position;

        DeathManager.instance.playerDeathEvent.AddListener(ResetPosition);
    }

    private void Update()
    {
        // If the player is on this path block, then move towards the target position
        if(moving)
        {
            float step = speed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, step);
        }

        // Check if the block has reached its destination, if has then stop moving
        if(Vector3.Distance(gameObject.transform.position, target) < 0.001f)
        {
            Input.instance.modifierX = 0f;
            moving = false;
        }
    }

    // Start moving this block when the player gets on it
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            // Add the platform's speed to player's speed so to move them together, as long as the player remains on the platform
            Input.instance.modifierX = speed;
            moving = true;
        }
    }

    // Reset position of the platform when the player dies
    private void ResetPosition()
    {
        Input.instance.modifierX = 0f;
        moving = false;
        gameObject.transform.position = startingPosition;
    }
}
