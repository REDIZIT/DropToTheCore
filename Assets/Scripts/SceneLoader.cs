using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame.SceneLoading
{
    public static class SceneLoader
    {
        public static float LoadOnDepth { get; private set; }

        public static IEnumerator LoadGame(float depth)
        {
            LoadOnDepth = depth;

            yield return new WaitForSeconds(0.8f);

            AsyncOperation op = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                yield return null;
            }

            op.allowSceneActivation = true;
        }

        public static void LoadMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public static void LoadTutorial()
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}