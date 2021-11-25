using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVelocity : MonoBehaviour
{
    public const float baseVelocity = 5f;

    public float slope;

    public const float gravity = -5f;

    public float collisionCount;

    public float x;
    public float y;
    public float z;

    private Rigidbody rb;

    private Vector3 speed;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        collisionCount = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if(collisionCount == 0)
        {
            speed = new Vector3 (x, gravity, z);
        }
        else
        {
            speed = new Vector3 (x, y, z);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = speed;
    }

    // Modify player's speed based on input and other modifiers beeing applied
    // Also modify the angle at which the speed is applied
    public void UpdateSpeed(float inputX, float inputZ)
    {   
        if(collisionCount == 0)
        {
            slope = 0;
        }

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

    private void OnCollisionEnter(Collision collisionInfo)
    {
        collisionCount++;
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        slope = AngleFromCollision(collisionInfo);
    }

    private void OnCollisionExit(Collision collisionInfo)
    {
        collisionCount--;
    }

    private float AngleFromCollision(Collision collisionInfo)
    {
        var cosAngle = collisionInfo.contacts[0].normal.normalized.y;

        // Get angle of slope in RADIANTS
        var angle = Mathf.Acos(cosAngle);
        return angle; 
    }
}
