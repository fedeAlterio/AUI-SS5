using Assets.Scripts.UI.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameDebug
{
    public class EscKeyForMenu : MonoBehaviour
    {
        // Private fields
        private InGameMenuManager _inGameMenuManager;



        // Initialization
        private void Awake()
        {
            _inGameMenuManager = FindObjectOfType<InGameMenuManager>();
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
                _inGameMenuManager.Show();
        }
    }
}
