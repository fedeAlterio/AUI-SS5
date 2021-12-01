using Assets.Scripts.Animations;
using Assets.Scripts.Models;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    // Editor fields
    [SerializeField] private Vector3 _deltaPosition;
    [SerializeField] private float _speed;



    // Private fields
    private bool _isPlayerOver;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _movementDirection;
    private Vector3 _middlePoint;
    private AsyncOperationManager _delayStartMovement;



    // Initialization
    private void Awake()
    {
        _startPosition = transform.parent.localPosition;
        _endPosition = _startPosition + _deltaPosition;
        _movementDirection = _endPosition - _startPosition;
        _delayStartMovement = new AsyncOperationManager(this);
        _middlePoint = (_startPosition + _endPosition) / 2;
    }



    
    // Events
    private void FixedUpdate()
    {                
        var target = _isPlayerOver ? _endPosition : _startPosition;     
        var direction = _movementDirection * (_isPlayerOver ? 1 : -1);
        var currentPosition = transform.parent.localPosition;
        var newPosition = currentPosition + _speed * Time.fixedDeltaTime * direction;        

        if (Vector3.Dot(target - newPosition, target - _middlePoint) < 0)
            return;
        transform.parent.localPosition = newPosition;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(UnityTag.Player))
            return;

        collision.gameObject.transform.parent = transform.parent;
        var newIsPlayerOver = true;
        _delayStartMovement.New(manager => ChangePlayerIsOver(manager, newIsPlayerOver));
    }


    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag(UnityTag.Player))
            return;

        collision.gameObject.transform.parent = null;
        var newIsPlayerOver = false;
        _delayStartMovement.New(manager => ChangePlayerIsOver(manager, newIsPlayerOver));
    }


    private async UniTask ChangePlayerIsOver(IAsyncOperationManager manager, bool isPlayerOver)
    {
        await manager.Delay(300);
        _isPlayerOver = isPlayerOver;
    }
}
