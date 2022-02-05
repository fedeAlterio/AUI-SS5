using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Calibration.UI
{
    public abstract class LabelManager : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private TextMeshProUGUI _text;
        
        
        
        // Private fields
        private CalibrationManager _calibrationManager;
        private Dictionary<CalibrationPhase, string> _descriptions = new Dictionary<CalibrationPhase, string>();



        // Initialization
        protected virtual void Awake()
        {
            _descriptions = CreateDescriptions();
            _calibrationManager = FindObjectOfType<CalibrationManager>();
            _text = GetComponent<TextMeshProUGUI>();
            _calibrationManager.StateChanged += CalibrationManager_StateChanged;
        }

        protected abstract Dictionary<CalibrationPhase, string> CreateDescriptions();



        // Event args
        private void CalibrationManager_StateChanged(CalibrationEventArgs args)
        {
            if(_descriptions.TryGetValue(args.CalibrationPhase, out string description))
                _text.text = description;
        }
    }
}
