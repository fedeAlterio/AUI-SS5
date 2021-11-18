using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Call this method when something collides with this enemy
    // Check if the colliding object is a Player, in which case call Manager
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision: Enemy");

        if(other.gameObject.tag == "Player")
        {
            DeathManager.instance.PlayerDeath();
        }        
    }
}
