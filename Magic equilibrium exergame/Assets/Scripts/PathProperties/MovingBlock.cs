using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public Vector3 startingPosition;
    public Vector3 target;
    public float speed = 100f;
    public Rigidbody rb;

    private void Start()
    {
        startingPosition = gameObject.transform.position;

        DeathManager.instance.playerDeathEvent.AddListener(ResetPosition);
    }

    private void Update()
    {
        // Check if the block has reached its destination, if has then stop moving
        if(Vector3.Distance(gameObject.transform.position, target) < 0.001f)
        {
            rb.AddForce(new Vector3(0, 0, -speed));
        }
    }

    // Start moving this block when the player gets on it
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            rb.AddForce(new Vector3(0, 0, speed));
        }
    }

    // Reset position of the platform when the player dies
    private void ResetPosition()
    {
        Input.instance.modifierX = 0f;
        gameObject.transform.position = startingPosition;
    }
}
