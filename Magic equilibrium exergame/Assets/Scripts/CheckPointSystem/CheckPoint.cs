using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Vector3 checkPointPosition;

    // Call this method when something collides with the checkpoint
    // Check if the colliding object is a Player, in which case call Manager
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            CheckPointManager.instance.NewCheckpoint(this);
        }        
    }
}
