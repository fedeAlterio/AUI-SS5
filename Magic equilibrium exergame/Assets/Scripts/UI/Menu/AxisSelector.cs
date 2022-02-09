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
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class AxisSelector : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private Image _rightImage;
        [SerializeField] private Image _leftImage;
        [SerializeField] private Image _topImage;
        [SerializeField] private Image _bottomImage;
        [SerializeField] private Color _selectionColor = Color.green;


        // Private fields
        private IMovementAxis _axis;
        private AsyncOperationManager _selection;
        private AsyncOperationManager _unselection;
        private UniTaskCompletionSource<AxisSelectionDirection> _selectionTCS;
        private Color _startColor;
        private Image[] _allImages;


        // Initialization
        private void Awake()
        {
            _selection = new AsyncOperationManager(this) { UseGameTimeScale = false };
            _unselection = new AsyncOperationManager(this) { UseGameTimeScale = false };
            _allImages = new[] { _bottomImage, _topImage, _rightImage, _leftImage };
        }

        private void Start()
        {
            _axis = this.GetInstance<IMovementAxis>();
            _startColor = _leftImage.color;
        }



        // Properties
        [field: SerializeField] public AxisSelectionDirection CurrentDirection { get; private set; } = AxisSelectionDirection.Cancel;
        [field: SerializeField] public float SelectionPercentage { get; private set; }
        [field: SerializeField] public float SelectionChangeSpeed { get; set; } = 1;
        public bool IsSelecting { get; private set; }        



        // Unity events
        private void Update()
        {
            if (!IsSelecting)
                return;

            var currentDirection = GetCurrentDirection();
            var isDirectionChanged = CurrentDirection != currentDirection;
            CurrentDirection = currentDirection;
            if(isDirectionChanged)
            {
                ResetAxis();                
                _selection.New(ChangeDirection);
            }
            ChangeColor();
        }

        private void ResetAxis()
        {                        
            _unselection.New(UnSelect);
            SelectionPercentage = 0;            
        }


        private void ChangeColor()
        {
            var currentImage = GetCurrentImage();
            if (currentImage == null)
                return;            
            currentImage.color = Color.Lerp(_startColor, _selectionColor, SelectionPercentage);
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

        private async UniTask UnSelect(IAsyncOperationManager manager)
        {
            var currentImage = GetCurrentImage();
            await _allImages.Where(image => image != currentImage).Select(image => manager.Lerp(image.color, _startColor, c => image.color = c, speed: 3* SelectionChangeSpeed));
        }


        // Core
        private AxisSelectionDirection GetCurrentDirection()
        {
            var axisNorm = new Vector2(_axis.HorizontalAxis, _axis.VerticalAxis).magnitude;
            if (axisNorm < 0.6f)
                return AxisSelectionDirection.Cancel;

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
            if (CurrentDirection == AxisSelectionDirection.Cancel)
                return;

            await manager.Lerp(0, 1, val => SelectionPercentage = val, speed: SelectionChangeSpeed);
            _selectionTCS?.TrySetResult(CurrentDirection);
        }


        // Utils
        private Image GetCurrentImage() => CurrentDirection switch
        {
            AxisSelectionDirection.Left => _leftImage,
            AxisSelectionDirection.Right => _rightImage,
            AxisSelectionDirection.Up => _topImage,
            AxisSelectionDirection.Down => _bottomImage,
            _ => null
        };
    }
}
