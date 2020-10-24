using InGame.GooglePlay;
using InGame.Settings;
using System.Collections;
using UnityEngine;

namespace InGame.Game
{
    public class FinishController : MonoBehaviour
    {
        public Animator finishScreen;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                StartCoroutine(IESlowDown(collision.GetComponent<PlayerController>()));
            }
        }

        private IEnumerator IESlowDown(PlayerController player)
        {
            Rigidbody2D rig = player.GetComponent<Rigidbody2D>();
            player.canMove = false;
            rig.gravityScale = 0;

            finishScreen.gameObject.SetActive(true);
            finishScreen.Play(1);


            SettingsManager.Settings.IsTutorialPassed = true;
            SettingsManager.Save();

            GooglePlayManager.GiveTutorialAchievement();


            while (rig.drag <= 8f)
            {
                rig.drag += Time.deltaTime * 10f;
                yield return null;
            }
        }
    }
}