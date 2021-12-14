using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameDebug
{
    public class ResetScene : MonoBehaviour
    {
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
