using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float x = 0;
    public float z = 0;

    private Vector3 speed = new Vector3(0, 0, 0);

    void Start()
    {
        x = 1;
    }

    // Update is called once per frame
    void Update()
    {
        speed = new Vector3 (x, 0, z);

        transform.Translate(speed * Time.deltaTime);
    }
}
