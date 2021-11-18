using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private void Start()
    {
        DeathManager.instance.playerDeathEvent.AddListener(Respawn);
    }   

    private void Respawn()
    {
        playerMovement.gameObject.transform.position = CheckPointManager.instance.GetRespawn();
    }
}

    
