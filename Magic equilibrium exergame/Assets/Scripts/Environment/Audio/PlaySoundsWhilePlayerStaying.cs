using Assets.Scripts.Abstractions;
using Assets.Scripts.Animations;
using Assets.Scripts.Models;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Environment.Audio
{
    public class PlaySoundsWhilePlayerStaying : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private Sfx _clip;        


        // Private fields
        private bool _playerStaying;
        private AsyncOperationManager _playSoundsOperation;
        private IMovementAxis _playerVelocityInput;
        private RespawnManager _respawnManager;
        private int _counter;

        

        // Initialization
        private void Awake()
        {
            _playSoundsOperation = new AsyncOperationManager(this);
            _respawnManager = FindObjectOfType<RespawnManager>();
            _playerStaying = FindObjectOfType<VelocityInput>();
        }

        private void Start()
        {
            _playSoundsOperation.New(PlaySounds);
        }



        // Properties
        private bool CanPlay => _playerStaying
            && Mathf.Abs(_playerVelocityInput.HorizontalAxis) > 0.2f
            && Mathf.Abs(_playerVelocityInput.HorizontalAxis) > 0.2f
            && !_respawnManager.IsRespawning;




        // Core
        private async UniTask PlaySounds(IAsyncOperationManager manager)
        {
            while (true)
            {
                if (CanPlay)
                    await AudioManager.instance.PlayClipAsync(_clip.ToString());
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!collision.gameObject.CompareTag(UnityTag.Player))
                return;

            _playerStaying = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!collision.gameObject.CompareTag(UnityTag.Player))
                return;
            _playerStaying = false;
        }
    }
}
