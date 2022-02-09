using Assets.Scripts.Animations;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Menu
{
    public class AxisSelectorTest : MonoBehaviour
    {
        // Private fields
        private AxisSelector _axisSelector;
        private AsyncOperationManager _selectionTest;



        // Initialization
        private void Awake()
        {
            _axisSelector = FindObjectOfType<AxisSelector>();
            _selectionTest = new AsyncOperationManager(this);
        }

        private void Start()
        {
            _selectionTest.New(SelectionTest);
        }



        // Core
        private async UniTask SelectionTest(IAsyncOperationManager arg)
        {
            while(true)
            {
                var selection = await _axisSelector.Select();
                Debug.Log(new { selection });
            }
        }
    }
}
