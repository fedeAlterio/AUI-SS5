using Assets.Scripts.Animations;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Editor fields
    [SerializeField] private int _hitDelay;



    // Private fields
    private AsyncOperationManager _deathNotifyOperation;


    // Initialization
    private void Awake()
    {
        _deathNotifyOperation = new AsyncOperationManager(this);
    }

    // Call this method when something collides with this enemy
    // Check if the colliding object is a Player, in which case call Manager
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _deathNotifyOperation.New(InvokeDeath);
        }        
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            _deathNotifyOperation.New(InvokeDeath);
        }
    }


    private async UniTask InvokeDeath(IAsyncOperationManager manager)
    {
        await manager.Delay(_hitDelay);
        DeathManager.instance.PlayerDeath();
    }
}
