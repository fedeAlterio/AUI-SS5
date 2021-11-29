using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gate
{
    public class GateSpawner : MonoBehaviour
    {
        // Private fields
        private GateManager _manager;
        private Coin[] _coins;



        // Initialization
        private void Start()
        {
            _manager = FindObjectOfType<GateManager>();
            _coins = FindObjectsOfType<Coin>();
            InitializeGate(_manager, _coins);
        }



        // Utils
        private void InitializeGate(GateManager gate, IEnumerable<Coin> coins)
        {
            gate.Initialize(coins);
        }
    }
}
