using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathManager : MonoBehaviour
{
    public static DeathManager instance;
    UnityEvent playerDeathEvent;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if(playerDeathEvent == null)
        {
            playerDeathEvent = new UnityEvent();
        }
    }

    // Called by other entities when the player dies
    public void PlayerDeath()
    {
        playerDeathEvent.Invoke();
        Debug.Log("Player Death Event Invoked");
    }
}
