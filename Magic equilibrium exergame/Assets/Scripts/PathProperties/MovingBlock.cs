using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public bool moving;
    public Vector3 target;
    public float speed = 01f;

    private void Start()
    {
        moving = false;
        speed = speed * Time.deltaTime;
    }

    private void Update()
    {
        // If the player is on this path block, then move towards the target position
        if(moving)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, speed);
        }

        // Check if the block has reached its destination, if has then stop moving
        if(Vector3.Distance(gameObject.transform.position, target) < -0.001f)
        {
            moving = false;
        }
    }

    // Start moving this block when the player gets on it
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            moving = true;
        }
    }
}
