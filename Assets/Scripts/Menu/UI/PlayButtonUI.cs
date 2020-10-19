using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame.Menu
{
    public class PlayButtonUI : MonoBehaviour
    {
        public Animator animator;
        
        public void OnClick()
        {
            StartCoroutine(IELoadGame());
        }

        private IEnumerator IELoadGame()
        {
            yield return new WaitForSeconds(0.5f);

            AsyncOperation op = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
            op.allowSceneActivation = false;

            while(op.progress < 0.9f)
            {
                yield return null;
            }

            op.allowSceneActivation = true;
        }
    }
}