using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public const float baseX = 1;
    public const float baseZ = 0;

    private float x;
    private float z;

    private Vector3 speed = new Vector3(0, 0, 0);

    // Update is called once per frame
    void Update()
    {
        speed = new Vector3 (x, 0, z);

        transform.Translate(speed * Time.deltaTime);
    }

    public void UpdateSpeed(float inputX, float inputZ)
    {
        x = baseX + inputX;
        z = baseZ + inputZ;
    }
}
