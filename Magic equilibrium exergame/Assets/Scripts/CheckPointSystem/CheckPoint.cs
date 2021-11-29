using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Vector3 spawnPosition;
    public int iD;
    public bool checkpointHit;


    // Initialization
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
        if (checkpointHit)
            return;

        if(collisionInfo.gameObject.CompareTag(UnityTag.Player))
        {
            checkpointHit = true;
            Debug.Log("Checkpoint");    
            spawnPosition = collisionInfo.gameObject.transform.position;
            CheckPointManager.instance.CheckpointReached(this);
        }        
    }
}
