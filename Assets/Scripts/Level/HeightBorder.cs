using InGame.Game;
using System.Collections;
using UnityEngine;

namespace InGame.Level
{
    public class HeightBorder : MonoBehaviour
    {
        public LineRenderer line;
        public new BoxCollider2D collider;

        public float sinHeight;
        private float _sinHeight;

        public bool isClosed;

        private PlayerController player;

        private void Start()
        {
            player = PlayerController.instance;

            _sinHeight = sinHeight;
            DrawSinLine();
        }

        private void Update()
        {
            if (player.transform.position.y < transform.position.y && !isClosed)
            {
                Close();
            }
        }

        public void Close()
        {
            isClosed = true;
            collider.enabled = true;
            StartCoroutine(IEClose());
        }
        private IEnumerator IEClose()
        {
            while(_sinHeight > 0.001f)
            {
                _sinHeight -= 0.8f * Time.deltaTime;
                DrawSinLine();

                yield return null;
            }

            DrawLine();
        }

        private void DrawSinLine()
        {
            Vector3[] positions = new Vector3[128];

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new Vector3(i / 3f - GameManager.WORLD_WIDTH / 2f, Mathf.Sin(i / 1.5f) * _sinHeight, 0);
            }

            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }
        private void DrawLine()
        {
            Vector3[] positions = new Vector3[2]
            {
                new Vector3(-GameManager.WORLD_WIDTH / 2f, 0, 0),
                new Vector3(GameManager.WORLD_WIDTH / 2f, 0, 0)
            };

            line.positionCount = 2;
            line.SetPositions(positions);
        }
    }
}