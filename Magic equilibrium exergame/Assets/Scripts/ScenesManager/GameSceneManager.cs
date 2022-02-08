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
        public static async UniTask LoadScene(int id, params GameObject[] dontDestroyOnChange)
        {
            MoveToDontDestroyOnLoad(dontDestroyOnChange);
            await SceneManager.LoadSceneAsync(id);
            MoveToCurrentScene(dontDestroyOnChange);
        }

        public static async UniTask LoadScene(GameScene scene, params GameObject[] dontDestroyOnChange)
        {
            MoveToDontDestroyOnLoad(dontDestroyOnChange);
            await SceneManager.LoadSceneAsync(scene.Name);
            MoveToCurrentScene(dontDestroyOnChange);
        }
        public static async UniTask LoadPreviousScene(params GameObject[] dontDestroyOnChange)
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentSceneIndex == 0)
                throw new InvalidOperationException("Scene index is the first, cannot load previous");
            await LoadScene(currentSceneIndex - 1, dontDestroyOnChange);
        }
        public static async UniTask LoadNextScene(params GameObject[] dontDestroyOnChange)
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
                throw new InvalidOperationException("Scene index is the last, cannot load next");

            await LoadScene(currentSceneIndex + 1, dontDestroyOnChange);
        }




        // Utils
        private static void MoveToDontDestroyOnLoad(GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
                GameObject.DontDestroyOnLoad(gameObject);
        }

        private static void MoveToCurrentScene(GameObject[] gameObjects)
        {
            foreach(var gameObject in gameObjects)
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        }
    }
}
