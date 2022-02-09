﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PropertyToLabelBinding : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private TextMeshProUGUI _label;



        // Initialization
        private void Awake()
        {
            if(_label == null)
                _label = GetComponent<TextMeshProUGUI>();
            var playerCount = GameSetting.instance?.players?.Count ?? 0;

            int playerCount2;
            if (GameSetting.instance != null && GameSetting.instance.players != null)
                playerCount2 = GameSetting.instance.players.Count;
            else
                playerCount2 = 0;
        }



        // Properties
        [field:SerializeField] public string PropertyName { get; set; }
        protected object Object { get; set; }



        // Core
        private void Update()
        {
            if (Object == null)
                return;

            var text = Object.GetType().GetProperty(PropertyName)?.GetValue(Object)?.ToString();
            _label.text = text ?? string.Empty;
        }
    }
}
