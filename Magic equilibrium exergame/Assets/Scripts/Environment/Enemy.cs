using Assets.Scripts.Animations;
using Assets.Scripts.Environment.Audio;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Editor fields
    [SerializeField] private int _hitDelay;
    [SerializeField] private Sfx _clip = Sfx.EnemyHit;



    // Private fields
    private AsyncOperationManager _deathNotifyOperation;



    // Initialization
    private void Awake()
    {
        _deathNotifyOperation = new AsyncOperationManager(this);
    }




    // Properties
    [field: SerializeField] public bool IsEnabled { get; set; } = true;




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
        if (!IsEnabled)
            return;
        //await UniTask.Delay(_hitDelay, cancellationToken: this.GetCancellationTokenOnDestroy());
        AudioManager.instance.PlayClip(_clip.ToString());
        DeathManager.instance.PlayerDeath();
    }
}
