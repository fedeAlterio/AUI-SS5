using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Vector3 checkPointPosition;
    public int iD;


    private void Start()
    {
    }


    public void Initialize(int checkpointId)
    {
        iD = checkpointId;
        CheckPointManager.instance.AddCheckpoint(this);
    }


    // Call this method when something collides with the checkpoint
    // Check if the colliding object is a Player, in which case call Manager
    private void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Player")
        {
            CheckPointManager.instance.CheckpointReached(this);
        }        
    }
}
