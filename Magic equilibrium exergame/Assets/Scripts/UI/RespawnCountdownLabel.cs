using Assets.Scripts.Animations;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class RespawnCountdownLabel : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private TextMeshProUGUI _text;


        // Private fields
        private RespawnManager _respawnManager;
        private AsyncOperationManager _showNumberAnimation;
        private Vector3 _startScale;



        // Initialization
        private void Awake()
        {
            _respawnManager = FindObjectOfType<RespawnManager>();
            _showNumberAnimation = new AsyncOperationManager(this);
            _showNumberAnimation.Speed *= 0.8f;
            _respawnManager.CountdownTick += number => _showNumberAnimation.New(manager => ShowNumber(number, manager));
        }

        private void Start()
        {
            _startScale = transform.localScale;
            transform.localScale = Vector3.zero;
        }



        // Core
        private async UniTask ShowNumber(int number, IAsyncOperationManager manager)
        {
            _text.text = $"{number}";
            await manager.Lerp(transform.localScale, _startScale * 1.5f, scale => transform.localScale = scale);
            await manager.Lerp(transform.localScale, number == 0 ? Vector3.zero : _startScale, scale => transform.localScale = scale);
        }

        private async UniTask FadeOut(IAsyncOperationManager manager)
        {
            await manager.Lerp(transform.localScale, Vector3.zero, scale => transform.localScale = scale);
        }
    }
}
