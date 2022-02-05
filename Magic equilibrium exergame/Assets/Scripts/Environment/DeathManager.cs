using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathManager : MonoBehaviour
{
    public static DeathManager instance;
    public UnityEvent playerDeathEvent = new UnityEvent();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }


    // Called by other entities when the player dies
    public void PlayerDeath()
    {
        playerDeathEvent.Invoke();
    }
}
