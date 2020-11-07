using InGame.Secrets;
using System;
using UnityEngine;

namespace InGame.Tutorial
{
    public class TutorialPager : MonoBehaviour
    {
        public static TutorialPager instance;

        public Animator group;
        public Transform pageContainer;

        [Header("Prefabs")]
        public GameObject coinTakenPagePrefab;


        [Flags]
        public enum Page
        {
            None = 0x0,
            CoinTaken = 0x1
        }



        public static class Events
        {
            public static void OnCoinTaken() { instance.ShowTutorialIfNotPasssed(Page.CoinTaken); }
        }







        void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(this);
        }





        public void ShowTutorialIfNotPasssed(Page page)
        {
            // If already passed - return
            if (SecretsManager.Secrets.PassedTutorials.HasFlag(page)) return;



            GameObject prefab = GetPagePrefab(page);

            foreach (Transform child in pageContainer) Destroy(child.gameObject);
            Instantiate(prefab, pageContainer);


            group.gameObject.SetActive(true);
            group.Play("ShowTutorialPage");

            Time.timeScale = 0;



            SecretsManager.Secrets.PassedTutorials = SecretsManager.Secrets.PassedTutorials | page;
            SecretsManager.Save();
        }
        public void HideTutorial()
        {
            Time.timeScale = 1;
            group.gameObject.SetActive(false);
        }







        private GameObject GetPagePrefab(Page page)
        {
            switch (page)
            {
                case Page.CoinTaken: return coinTakenPagePrefab;
                default: throw new Exception($"Tried to get tutorial page for {page} but there is no prefab for it");
            }
        }
    }
}