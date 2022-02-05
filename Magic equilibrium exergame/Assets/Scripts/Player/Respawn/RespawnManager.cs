using Assets.Scripts.Animations;
using Assets.Scripts.Models.Path.Generation.Line;
using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.Path.BuildingStrategies.Path;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    // Events
    public event Action<int> CountdownTick;



    // Editor fields
    [SerializeField] private int _respawnTimerPeriod;
    [SerializeField] private float _respawnAnimationSpeed = 1;

    
    // Private fields
    private CheckPointManager _checkPointManager;
    private DeathManager _deathManager;
    private AsyncOperationManager _respawnOperation;
    private PlayerVelocity _playerVelocity;
    private Rigidbody _playerRigidbody;
    private RigidbodyConstraints _oldConstraints;
    private IPathConfiguration _pathConfiguration;    



    // Initialization
    private void Awake()
    {
        _playerVelocity = FindObjectOfType<PlayerVelocity>();
        _playerRigidbody = _playerVelocity.GetComponent<Rigidbody>();
        _checkPointManager = FindObjectOfType<CheckPointManager>();
        _deathManager = FindObjectOfType<DeathManager>();
        _respawnOperation = new AsyncOperationManager(this) { PlayerLoopTiming = PlayerLoopTiming.FixedUpdate };
        _deathManager.playerDeathEvent.AddListener(RespawnWithCountdown);
    }

    private void Start()
    {
        _pathConfiguration = this.GetInstance<IPathConfiguration>();
        _respawnOperation.New(FirstSpawn);
    }

    private async UniTask FirstSpawn(IAsyncOperationManager manager)
    {
        while (CheckPointManager.instance.CheckPoints.Count == 0)
            await manager.NextFrame();
        transform.position = CheckPointManager.instance.RespawnPosition;
    }



    // Respawn
    private void RespawnWithCountdown() => _respawnOperation.New(RespawnWithCountdown);
    private async UniTask RespawnWithCountdown(IAsyncOperationManager manager)
    {
        FreezePlayerPosition();
        await MovePlayerToCheckPoint(manager);
        await WaitForCountdown(manager);
        UnFreezePlayerPosition();
    }

    private async UniTask WaitForCountdown(IAsyncOperationManager manager)
    {        
        for (var i = 0; i <= _respawnTimerPeriod; i++)
        {
            CountdownTick?.Invoke(_respawnTimerPeriod - i);
            await manager.Delay(1000);
        }
    }

    private void FreezePlayerPosition()
    {
        _oldConstraints = _playerRigidbody.constraints;
        _playerRigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    private void UnFreezePlayerPosition() => _playerRigidbody.constraints = _oldConstraints;

    private async UniTask MovePlayerToCheckPoint(IAsyncOperationManager manager)
    {
        var startPosition = _playerRigidbody.position;
        var endPosition = CheckPointManager.instance.RespawnPosition;
        var distance = Vector3.Distance(endPosition, startPosition);
        if (distance == 0)
            return;


        var middlePointY = (endPosition.y - startPosition.y) + 3;
        var deltaX = startPosition.x - endPosition.x;
        if (Mathf.Abs(deltaX) < _pathConfiguration.PathThickness*2)
            deltaX = _pathConfiguration.PathThickness * 2f * Mathf.Sign(deltaX);
        var x = endPosition.x + deltaX;
        var middlePoint = new Vector3(x, middlePointY, (startPosition.z + endPosition.z)/2);

        var bezier = new QuadraticBezier(startPosition, middlePoint, endPosition);

        await manager.Lerp(0, distance, t => _playerVelocity.transform.position = bezier.PointAt((t/distance) + bezier.MinT), speed: _respawnAnimationSpeed, smooth: true);
    }
}

    
