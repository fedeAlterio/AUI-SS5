using Assets.Scripts.Animations;
using Assets.Scripts.Models;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Sea
{
    public class WaterCollisionManager : MonoBehaviour
    {
        // Private fields
        private AsyncOperationManager _collisionManager;
        [SerializeField] MeshRenderer _meshRenderer;
        private Vector3 _holePosition;
        private GameObject _gameObject;
        private MeshRenderer meshRenderer;
        private float _startRadius;
        private float _startHeight;



        // Initialization
        private void Awake()
        {
            _collisionManager = new AsyncOperationManager(this);
        }

        private void Start()
        {
            _startRadius = HoleRadius;
            HoleRadius = 0.5f * _startRadius;
            _startHeight = HoleHeight;
        }



        // Properties
        public float HoleMaxHeight
        {
            get => _meshRenderer.material.GetFloat("_" + nameof(HoleMaxHeight));
            set => _meshRenderer.material.SetFloat("_" + nameof(HoleMaxHeight), value);
        }


        public float HoleHeight
        {
            get => _meshRenderer.material.GetFloat("_" + nameof(HoleHeight));
            set => _meshRenderer.material.SetFloat("_" + nameof(HoleHeight), value);
        }

        private float yOffset;

        public Vector3 HoleCenter
        {
            get => _meshRenderer.material.GetVector("_" + nameof(HoleCenter));
            set => _meshRenderer.material.SetVector("_" + nameof(HoleCenter), value);
        }

        public float HoleRadius
        {
            get => _meshRenderer.material.GetFloat("_" + nameof(HoleRadius));
            set => _meshRenderer.material.SetFloat("_" + nameof(HoleRadius), value);
        }


        // Events
        private void OnTriggerEnter(Collider collider)
        {
            if(collider.gameObject.tag != UnityTag.Player)
                return;

            _holePosition = collider.gameObject.transform.position;
            _gameObject = collider.gameObject;
            meshRenderer = _gameObject.GetComponent<MeshRenderer>();
            HoleCenter = _gameObject.transform.position - yOffset * Vector3.up;
            HoleRadius = _startRadius;
            HoleHeight = _startHeight;
        }

        private void OnTriggerStay(Collider collision)
        {
            yOffset = meshRenderer.bounds.extents.y;
            HoleCenter = Vector3.MoveTowards(HoleCenter, _gameObject.transform.position - yOffset * Vector3.up, 0.08f);  
            var deltaY = _holePosition.y - _gameObject.transform.position.y - yOffset * 0.5f;
            HoleHeight = deltaY;
            Debug.Log(HoleHeight);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag != UnityTag.Player)
                return;

            _collisionManager.New(CloseHoleAnimation);
        }


        // Animations
        private async UniTask CloseHoleAnimation(IAsyncOperationManager manager)
        {
            var closingSpeed = Mathf.PI * 1.5f;
            //var closeHoleHorizontally = manager.Lerp(HoleRadius, 0.5f * _startRadius, val => HoleRadius = val, speed: closingSpeed);
            var closeHoleVertically = manager.Lerp(0, Mathf.PI * 4, t => HoleHeight = OscillatingHeight(t, HoleMaxHeight), speed: closingSpeed, smooth: false);
            var closeHorizontally = manager.Lerp(HoleRadius, _startRadius * 0.6f, val => HoleRadius = val, speed: closingSpeed);
            await new[] { closeHorizontally,  closeHoleVertically };
        }


        private float OscillatingHeight(float t, float startHeight)
        {
            return Mathf.Cos(t) * startHeight * Mathf.Exp(-t * 0.4f);
        }
    }
}
