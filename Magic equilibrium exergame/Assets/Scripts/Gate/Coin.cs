using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gate
{
    public class Coin : MonoBehaviour
    {
        // Events
        public event Action<Coin> Taken;


        // Events
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(UnityTag.Player))
                OnCoinTaken();
        }



        // Events
        private void OnCoinTaken()
        {
            Debug.Log("Coin");
            AudioManager.instance.PlayClip("CoinTaken");
            transform.gameObject.SetActive(false);
            Taken?.Invoke(this);
        }



        // Public Methods
        public void ResetCoin()
        {
            transform.gameObject.SetActive(true);
        }
    }
}
