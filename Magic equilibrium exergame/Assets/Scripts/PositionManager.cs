using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{    
    public PlayerVelocity playerVelocity;
    private bool _startSpawnCompleted;
    private CheckPointManager _checkPointManager;
    private DeathManager _deathManager;


    // Initialization
    private void Awake()
    {
        _checkPointManager = FindObjectOfType<CheckPointManager>();
        _deathManager = FindObjectOfType<DeathManager>();
        _checkPointManager.CheckpointAdded += OnCheckpointAdded;
        _deathManager.playerDeathEvent.AddListener(Respawn);
    }
    private void Start()
    {
    }     



    // Events handlers
    private void OnCheckpointAdded(CheckPoint checkpoint)
    {
        if (_startSpawnCompleted)
            return;

        _startSpawnCompleted = true;
        Respawn();
    }



    // Utils
    private void Respawn()
    {
        transform.position = CheckPointManager.instance.GetRespawnPosition();
    }
}

    
