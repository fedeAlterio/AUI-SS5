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
            _showNumberAnimation.Speed *= 0.6f;
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
            await manager.Lerp(-0.5f * Mathf.PI, 0.5f * Mathf.PI, t => transform.localScale = _startScale * (1 + 0.4f * Mathf.Cos(t)));
            if (number == 0)
                await FadeOut(manager);
        }

        private async UniTask FadeOut(IAsyncOperationManager manager)
        {
            await manager.Lerp(transform.localScale, Vector3.zero, scale => transform.localScale = scale);
        }        
    }
}
