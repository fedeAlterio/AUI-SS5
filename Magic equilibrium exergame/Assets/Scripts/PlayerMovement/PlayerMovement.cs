using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public const float baseX = 0;
    public const float baseZ = 0;

    public float x;
    public float z;

    private Rigidbody rb;

    private Vector3 speed = new Vector3(0, 0, 0);

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, 0, 100));
    }

    // Update is called once per frame
    void Update()
    {
        speed = new Vector3 (x, 0, z);
    }

    private void FixedUpdate()
    {
        rb.AddForce(speed);
    }

    // Modify player's speed based on input and other modifiers beeing applied
    public void UpdateSpeed(float inputX, float inputZ)
    {
        x = inputX;
        z = inputZ;
    }
}
