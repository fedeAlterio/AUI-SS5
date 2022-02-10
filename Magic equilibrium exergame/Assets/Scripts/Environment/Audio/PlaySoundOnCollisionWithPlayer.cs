using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Environment.Audio
{
    public class PlaySoundOnCollisionWithPlayer : MonoBehaviour
    {
        // Editor fields
        [SerializeField] private Sfx _clip;



        // Core
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(UnityTag.Player))
                return;

            AudioManager.instance.PlayClip(_clip.ToString());
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag(UnityTag.Player))
                return;

            AudioManager.instance.PlayClip(_clip.ToString());
        }
    }
}
