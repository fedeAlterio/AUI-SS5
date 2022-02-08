using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ScenesManager
{
    public class GameScene
    {
        // Initialization
        private GameScene(string name) => Name = name;
        public string Name { get; }



        // Enum
        public static GameScene MainMenu { get; } = new GameScene("Menu Scene");
        public static GameScene EndMenu { get; } = new GameScene("EndMenu");
    }
}
