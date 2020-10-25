using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame.SceneLoading
{
    public static class SceneLoader
    {
        public static float LoadOnDepth { get; private set; }


        public static LoadGameType GameType { get; private set; }
        public enum LoadGameType
        {
            Checkpoints, Infinity, RandomInfinity
        }



        public static IEnumerator LoadCheckpointsGame(float depth)
        {
            LoadOnDepth = depth;
            GameType = LoadGameType.Checkpoints;
            yield return LoadGameScene();
        }
        public static IEnumerator LoadInfinityGame()
        {
            GameType = LoadGameType.Infinity;
            Debug.Log("Load infinity game");
            yield return LoadGameScene();
            Debug.Log("Loaded");
        }
        public static IEnumerator LoadHardInfinityGame()
        {
            GameType = LoadGameType.RandomInfinity;
            yield return LoadGameScene();
        }
        public static void LoadMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public static void LoadTutorial()
        {
            SceneManager.LoadScene("Tutorial");
        }





        private static IEnumerator LoadGameScene()
        {
            yield return new WaitForSeconds(0.8f);

            AsyncOperation op = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                yield return null;
            }

            op.allowSceneActivation = true;
        }
    }
}