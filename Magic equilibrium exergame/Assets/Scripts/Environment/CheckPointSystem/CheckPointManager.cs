using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CheckPointManager : MonoBehaviour
{
    // Events
    public event Action<CheckPoint> CheckpointAdded;
    public event Action CheckpointTaken;
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


    // Properties
    public Vector3 RespawnPosition => orderedCheckPoints.Any() ? orderedCheckPoints[lastCheckpoint].spawnPosition : Vector3.zero;
    public IReadOnlyList<CheckPoint> CheckPoints => orderedCheckPoints;
    public int LastCheckpoint => lastCheckpoint;


    public void AddCheckpoint(CheckPoint checkPoint)
    {
        orderedCheckPoints.Add(checkPoint);
        orderedCheckPoints.OrderBy(checkpoint => checkPoint.iD).ToList();
        checkPoint.Taken += CheckpointReached;
        CheckpointAdded?.Invoke(checkPoint);
    }


    // When a checkpoint is reached, check whether that is the furthest the player has reached so far
    // If it is, update the index
    private void CheckpointReached(CheckPoint checkPoint)
    {
        int tempIndex = orderedCheckPoints.IndexOf(checkPoint);
        
        if(lastCheckpoint < tempIndex)
        {
            lastCheckpoint = tempIndex;
        }
        CheckpointTaken?.Invoke();
    }
}
