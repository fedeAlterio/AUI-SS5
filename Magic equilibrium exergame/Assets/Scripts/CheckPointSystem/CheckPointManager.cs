using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CheckPointManager : MonoBehaviour
{
    // Events
    public event Action<CheckPoint> CheckpointAdded;

    public static CheckPointManager instance;
    public List<CheckPoint> orderedCheckPoints = new List<CheckPoint>();

    private int lastCheckpoint;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        lastCheckpoint = 0;
    }


    public void AddCheckpoint(CheckPoint checkPoint)
    {
        orderedCheckPoints.Add(checkPoint);
        orderedCheckPoints.OrderBy(checkpoint => checkPoint.iD).ToList();
        CheckpointAdded?.Invoke(checkPoint);
    }


    // When a checkpoint is reached, check whether that is the furthest the player has reached so far
    // If it is, update the index
    public void CheckpointReached(CheckPoint checkPoint)
    {
        int tempIndex = orderedCheckPoints.IndexOf(checkPoint);
        
        if(lastCheckpoint < tempIndex)
        {
            lastCheckpoint = tempIndex;
        }
    }   

    // Tells where to respawn the player
    public Vector3 GetRespawnPosition()
    {
        return orderedCheckPoints[lastCheckpoint].spawnPosition;
    } 
}
