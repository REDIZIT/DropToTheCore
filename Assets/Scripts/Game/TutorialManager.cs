using InGame.SceneLoading;
using InGame.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace InGame.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        public PlayerController player;
        public GameObject tutorialScreen;
        public List<GameObject> steps;
        public Image backgroundDarkner;

        public Volume postProcessing;

        public bool isTutorialRunning;

        private void Start()
        {
            postProcessing.profile.components.Find(c => c.GetType() == typeof(Bloom)).active = SettingsManager.Settings.IsBloomEnabled;
            StartTutorial();
        }
        public void StartTutorial()
        {
            if (isTutorialRunning) return;
            StartCoroutine(IETutorial());
        }
        public void GoToMenu()
        {
            SceneLoader.LoadMenu();
        }



        private IEnumerator IETutorial()
        {
            isTutorialRunning = true;

            tutorialScreen.SetActive(true);


            for (int i = 0; i < steps.Count; i++)
            {
                yield return WaitForStep(i);
            }


            tutorialScreen.SetActive(false);
            isTutorialRunning = false;
        }

        private IEnumerator WaitForStep(int stepNumber)
        {
            steps.ForEach(c => c.SetActive(false));
            steps[stepNumber].SetActive(true);

            Time.timeScale = 0;
            player.canMove = false;
            backgroundDarkner.enabled = true;

            yield return new WaitForSecondsRealtime(1);
            yield return new WaitUntil(() => Input.GetMouseButton(0));

            Time.timeScale = 1;
            player.canMove = true;
            steps[stepNumber].SetActive(false);
            backgroundDarkner.enabled = false;

            yield return new WaitForSeconds(1);
        }
    }
}