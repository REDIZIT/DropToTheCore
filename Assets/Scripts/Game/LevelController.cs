using UnityEngine;

namespace InGame.Level
{
    public class LevelController : MonoBehaviour
    {
        public PlayerController player;
        public Transform walls;

        private void Update()
        {
            walls.transform.position = new Vector3(walls.transform.position.x, player.transform.position.y);
        }
    }

}