using InGame.Game;
using InGame.GooglePlay;
using System.Collections;
using UnityEngine;

namespace InGame.Level
{
    public class LavaController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                StartCoroutine(IEOnCollision(collision.GetComponent<PlayerController>()));
            }
        }

        private IEnumerator IEOnCollision(PlayerController player)
        {
            Rigidbody2D rig = player.GetComponent<Rigidbody2D>();
            player.canMove = false;
            rig.gravityScale = 0;

            GooglePlayManager.GiveComingSoonAchievement();


            while (rig.drag <= 8f)
            {
                rig.drag += Time.deltaTime * 10f;
                yield return null;
            }

            GameManager.instance.DeadActions(false);
        }
    }
}