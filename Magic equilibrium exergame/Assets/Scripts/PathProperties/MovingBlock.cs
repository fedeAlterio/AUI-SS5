using Assets.Scripts.Animations;
using Assets.Scripts.Models;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    // Private fields
    private bool _isPlayerOver;
    private Vector3 _startPosition;
    private AsyncOperationManager _delayStartMovement;



    // Initialization
    private void Awake()
    {
        _startPosition = transform.parent.localPosition;
        _delayStartMovement = new AsyncOperationManager(this);
    }



    // Properties
    [field:SerializeField] public Vector3 DeltaPosition { get; set; }
    [field:SerializeField] public float Speed { get; set; }



    // Core
    private void MoveTowardsTarget()
    {
        var endPosition = _startPosition + DeltaPosition;
        var movementDirection = endPosition - _startPosition;
        var middlePoint = (_startPosition + endPosition) / 2;
        var target = _isPlayerOver ? endPosition : _startPosition;
        if (Vector3.Distance(transform.parent.localPosition, target) < float.Epsilon)
            return;


        var direction = movementDirection.normalized * (_isPlayerOver ? 1 : -1);
        var currentPosition = transform.parent.localPosition;
        var newPosition = currentPosition + Speed * Time.smoothDeltaTime * direction;
        var hasGoneTooFar = Vector3.Dot(target - newPosition, target - middlePoint) <= 0; // Dot product < 0 -> opposite directions

        transform.parent.localPosition = hasGoneTooFar ? target : newPosition;
    }



    // Events
    private void Update()
    {
        MoveTowardsTarget();
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

        if(collision.gameObject.transform.parent == transform.parent)
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
