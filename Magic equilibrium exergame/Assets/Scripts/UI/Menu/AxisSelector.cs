using Assets.Scripts.Abstractions;
using Assets.Scripts.Animations;
using Assets.Scripts.Path.BuildingStrategies;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Menu
{
    public class AxisSelector : MonoBehaviour
    {
        // Private fields
        private IMovementAxis _axis;
        private AsyncOperationManager _selection;
        private UniTaskCompletionSource<AxisSelectionDirection> _selectionTCS;        


        // Initialization
        private void Awake()
        {
            _selection = new AsyncOperationManager(this);
        }

        private void Start()
        {
            _axis = this.GetInstance<IMovementAxis>();            
        }



        // Properties
        [field: SerializeField] public AxisSelectionDirection CurrentDirection { get; private set; }
        [field: SerializeField] public float SelectionPercentage { get; private set; }
        [field: SerializeField] public int SelectionChangeSpeed { get; set; } = 1;
        public bool IsSelecting { get; private set; }



        // Unity events
        private void Update()
        {
            if (!IsSelecting)
                return;

            var currentDirection = GetCurrentDirection();
            if (CurrentDirection == currentDirection)
                return;

            CurrentDirection = currentDirection;
            _selection.New(ChangeDirection);
        }



        // Public
        public async UniTask<AxisSelectionDirection> Select()
        {
            IsSelecting = true;
            _selectionTCS?.TrySetResult(AxisSelectionDirection.Cancel);
            _selectionTCS = new UniTaskCompletionSource<AxisSelectionDirection>();
            var selectionTask = _selectionTCS.Task;
            _selection.New(ChangeDirection);
            var selection = await selectionTask;
            IsSelecting = false;            

            return selection;
        }


        // Core
        private AxisSelectionDirection GetCurrentDirection()
        {
            var isHorizontalDominantDirection = Mathf.Abs(_axis.HorizontalAxis) > Mathf.Abs(_axis.VerticalAxis);
            var isPositiveDirection = (isHorizontalDominantDirection ? _axis.HorizontalAxis : _axis.VerticalAxis) > 0;
            return (isHorizontalDominantDirection, isPositiveDirection) switch
            {
                (true, true) => AxisSelectionDirection.Right,
                (true, false) => AxisSelectionDirection.Left,
                (false, true) => AxisSelectionDirection.Up,
                (false, false) => AxisSelectionDirection.Down
            };
        }


        private async UniTask ChangeDirection(IAsyncOperationManager manager)
        {
            await manager.Lerp(0, 1, val => SelectionPercentage = val, speed: SelectionChangeSpeed);
            _selectionTCS?.TrySetResult(CurrentDirection);
        }
    }
}
