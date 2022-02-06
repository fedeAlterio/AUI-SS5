using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Game_UI
{
    public class CheckpointCountLabel : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private TextMeshProUGUI _label;



        // Private fields
        private CheckPointManager _checkPointManager;



        // Initialization
        private void Awake()
        {
            _checkPointManager = FindObjectOfType<CheckPointManager>();
            _checkPointManager.CheckpointTaken += UpdateLabel;
        }
        private void Start()
        {
            UpdateLabel();
        }



        // Core
        private void UpdateLabel()
        {
            _label.text = _checkPointManager.CheckPoints.Count == 0
                ? string.Empty
                : $"{_checkPointManager.LastCheckpoint + 1} / {_checkPointManager.CheckPoints.Count}";
        }
    }
}
