using Assets.Scripts.ScenesManager;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Menu
{
    public class EndMenuCommands : MonoBehaviour
    {
        // Commands
        private UICommand _backToMenuCommand;
        private UICommand _restartLevelCommand;


        // Initialization
        private void Awake()
        {
            _backToMenuCommand = new UICommand(async () => await GameSceneManager.LoadScene(GameScene.MainMenu));
            _restartLevelCommand = new UICommand(async () => await GameSceneManager.LoadPreviousScene());
        }


        // Public
        public void GoToMenu() => _backToMenuCommand.Execute();      
        public void RestartLevel() => _restartLevelCommand.Execute();
    }
}
