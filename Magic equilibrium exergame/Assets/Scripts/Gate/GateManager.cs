using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gate
{
    public class GateManager : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private GameObject _door;



        // Private fields
        private List<Coin> _coins = new List<Coin>();
        private List<Coin> _coinsTaken = new List<Coin>();
        private DeathManager _deathManager;



        // Initialization
        private void Awake()
        {
            _deathManager = FindObjectOfType<DeathManager>();
            _deathManager.playerDeathEvent.AddListener(OnPlayerDeath);
        }

        public void Initialize(IEnumerable<Coin> coins)
        {
            foreach(var coin in coins)
                AddCoin(coin);
        }



        // Events
        private void OnPlayerDeath()
        {
            if (IsOpen)
                return;

            ResetGate();
        }



        // Properties
        public bool IsOpen { get; private set; }



        // Events
        private void Update()
        {
            _door.SetActive(!IsOpen);
        }


        // Public
        private void AddCoin(Coin coin)
        {
            coin.Taken += OnCoinTaken;
            _coins.Add(coin);
        }

        private void OnCoinTaken(Coin coin)
        {
            _coinsTaken.Add(coin);
            if (_coinsTaken.Count == _coins.Count)
                IsOpen = true;
        }

        private void ResetGate()
        {
            _coinsTaken.Clear();
            foreach (var coin in _coins)
                coin.ResetCoin();
        }
    }
}
