using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CheckPointManager : MonoBehaviour
{
    // Events
    public event Action<CheckPoint> CheckpointAdded;
    public event Action<CheckPoint> CurrentCheckpointChanged;


    // Fields
    public static CheckPointManager instance;
    public List<CheckPoint> orderedCheckPoints = new List<CheckPoint>();



    // Initialization
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }



    // Properties
    public Vector3 RespawnPosition => orderedCheckPoints.Any() ? CurrentCheckpoint.spawnPosition : Vector3.zero;
    public IReadOnlyList<CheckPoint> CheckPoints => orderedCheckPoints;
    public CheckPoint CurrentCheckpoint { get; private set; }
    public int CurrentCheckpointIndex => orderedCheckPoints.IndexOf(CurrentCheckpoint);


    // Public
    public void AddCheckpoint(CheckPoint checkPoint)
    {
        if(CurrentCheckpoint == null)
            CurrentCheckpoint = checkPoint;

        orderedCheckPoints.Add(checkPoint);
        orderedCheckPoints.OrderBy(checkpoint => checkPoint.Id).ToList();
        checkPoint.Hit += CheckpointReached;
        CheckpointAdded?.Invoke(checkPoint);
    }

    public void ForceToCheckpoint(CheckPoint checkpoint)
    {
        CurrentCheckpoint = checkpoint;
        CurrentCheckpointChanged?.Invoke(checkpoint);
    }



    // When a checkpoint is reached, check whether that is the furthest the player has reached so far
    // If it is, update the index
    private void CheckpointReached(CheckPoint checkPoint)
    {
        int hitIndex = orderedCheckPoints.IndexOf(checkPoint);
        var currentCheckpointIndex = orderedCheckPoints.IndexOf(CurrentCheckpoint);
        if (hitIndex <= currentCheckpointIndex)
            return;

        CurrentCheckpoint = checkPoint;
        CurrentCheckpointChanged?.Invoke(checkPoint);
    }
}
