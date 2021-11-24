using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVelocity : MonoBehaviour
{
    public const float baseVelocity = 5f;

    public float slope;
    public bool colliding;

    public const float gravity = -5f;

    public float x;
    public float y;
    public float z;

    private Rigidbody rb;

    private Vector3 speed;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        slope = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if(colliding)
        {
            speed = new Vector3 (x, y, z);
        }
        else
        {
            speed = new Vector3 (x, gravity, z);
        }
        
    }

    private void FixedUpdate()
    {
        rb.velocity = speed;
    }

    // Modify player's speed based on input and other modifiers beeing applied
    public void UpdateSpeed(float inputX, float inputZ)
    {   
        float cos = Mathf.Cos(slope);
        float sin = Mathf.Sin(slope);

        x = inputX;
        y = sin * (baseVelocity + inputZ);
        z = cos * (baseVelocity + inputZ);

        if(z < 0f)
        {
            z = 0f;
        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if(collisionInfo == null)
        {
            colliding = false;
        }
        else
        {
            colliding = true;
        }
    }
}
