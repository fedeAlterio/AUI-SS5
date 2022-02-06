using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Vector3 spawnPosition;
    public int Id;
    public bool checkpointHit;
    public event Action<CheckPoint> Hit;
    private MeshFilter _meshRenderer;



    // Initialization
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshFilter>();
        spawnPosition = _meshRenderer.mesh.bounds.center;
    }


    private void Start()
    {
    }


    public void Initialize(int checkpointId)
    {
        Id = checkpointId;
        CheckPointManager.instance.AddCheckpoint(this);
    }



    // Call this method when something collides with the checkpoint
    // Check if the colliding object is a Player, in which case call Manager
    private void OnCollisionEnter(Collision collisionInfo)
    {        
        if(collisionInfo.gameObject.CompareTag(UnityTag.Player))
        {
            checkpointHit = true;            
            Hit?.Invoke(this);
        }        
    }
}
