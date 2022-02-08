using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.ScenesManager
{
    public static class GameSceneManager
    {
        public static async UniTask LoadScene(int id, params MonoBehaviour[] dontDestroyOnChange)
        {
            MoveToDontDestroyOnLoad(dontDestroyOnChange);
            await SceneManager.LoadSceneAsync(id);
            MoveToCurrentScene(dontDestroyOnChange);
        }

        public static async UniTask LoadScene(string sceneName, params MonoBehaviour[] dontDestroyOnChange)
        {
            MoveToDontDestroyOnLoad(dontDestroyOnChange);
            await SceneManager.LoadSceneAsync(sceneName);
            MoveToCurrentScene(dontDestroyOnChange);
        }



        // Utils
        private static void MoveToDontDestroyOnLoad(MonoBehaviour[] monoBehaviours)
        {
            foreach (var monoBehaviour in monoBehaviours)
                GameObject.DontDestroyOnLoad(monoBehaviour.gameObject);
        }

        private static void MoveToCurrentScene(MonoBehaviour[] monoBehaviours)
        {
            foreach(var monoBehaviour in monoBehaviours)
                SceneManager.MoveGameObjectToScene(monoBehaviour.gameObject, SceneManager.GetActiveScene());
        }
    }
}
