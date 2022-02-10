using Assets.Scripts.Animations;
using Assets.Scripts.ScenesManager;
using Assets.Scripts.UI.Game_UI;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Menu
{
    public class InGameMenuManager : MonoBehaviour
    {
        // Private fields
        private CanvasGroup _canvasGroup;
        private AsyncOperationManager _showOperation;
        private AxisSelector _axisSelector;
        private MoveBetweenCheckpoints _moveBetweenCheckpoints;
        private bool _isVisible;



        // Intiialziation
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _showOperation = new AsyncOperationManager(this);
            _axisSelector = FindObjectOfType<AxisSelector>();
            _moveBetweenCheckpoints = FindObjectOfType<MoveBetweenCheckpoints>();
        }

        private void Start()
        {
            SetIsMenuInteractable(false);
        }


        // Public
        public void Show()
        {
            if(_isVisible)
                return;
            _showOperation.New(ShowMenu);
        }



        // Core
        private async UniTask ShowMenu(IAsyncOperationManager manager)
        {
            _isVisible = true;
            _canvasGroup.alpha = 1;
            SetIsMenuInteractable(true);
            var timeScale = Time.timeScale;
            Time.timeScale = 0;

            var selection = await _axisSelector.Select();
            Time.timeScale = timeScale;
            SetIsMenuInteractable(false);
            _canvasGroup.alpha = 0;
            _isVisible = false;

            await manager.Delay(50);
            await HandleSelection(selection);
        }        

        private void SetIsMenuInteractable(bool isInteractable)
        {
            _canvasGroup.interactable = isInteractable;
            _canvasGroup.blocksRaycasts = isInteractable;
        }

        private async UniTask HandleSelection(AxisSelectionDirection selection)
        {
            switch (selection)
            {
                case AxisSelectionDirection.Up:
                    await GameSceneManager.LoadScene(GameScene.MainMenu);
                    break;
                case AxisSelectionDirection.Right:
                    _moveBetweenCheckpoints.MoveNextCheckpoint();
                    break;
                case AxisSelectionDirection.Left:
                    _moveBetweenCheckpoints.MovePreviousCheckpoint();
                    break;
                default:
                    break;
            }
        }
    }
}
