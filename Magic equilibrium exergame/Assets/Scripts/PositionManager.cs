using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public PlayerVelocity playerVelocity;

    private void Start()
    {
        DeathManager.instance.playerDeathEvent.AddListener(Respawn);
    }   

    private void Respawn()
    {
        playerVelocity.gameObject.transform.position = CheckPointManager.instance.GetRespawn();
    }
}

    
