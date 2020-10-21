using InGame.Game;
using InGame.Game.Bonuses;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InGame.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        public PlayerController player;

        public CheckpointController checkpointPrefab;

        public float areasSpacing;

        private float lastSpawnedDepth;
        private float lastSpawnedBonusDepth;

        private List<GameObject> toDestroyAfterDeath = new List<GameObject>();
        private List<GameObject> spawnedCheckpoints = new List<GameObject>();

        private AreaSO prevPattern;

        public const float CHECKPOINTS_DISTANCE = 500;


        private void Update()
        {
            GenerationLoop();
        }
        public void GenerationLoop(bool isInit = false)
        {
            if (-player.transform.position.y >= lastSpawnedDepth - 50)
            {
                SpawnNextArea();
                SpawnBonus();

                if (isInit)
                {
                    GenerationLoop(isInit);
                    return;
                }
            }


            if (toDestroyAfterDeath.Count > 0)
            {
                if (toDestroyAfterDeath[0] == null || toDestroyAfterDeath[0].transform == null)
                {
                    toDestroyAfterDeath.RemoveAll(c => c == null || c.transform == null);
                }

                if (-player.transform.position.y - -toDestroyAfterDeath[0].transform.position.y >= 200)
                {
                    Destroy(toDestroyAfterDeath[0]);
                    toDestroyAfterDeath.RemoveAt(0);
                }
            }
            if (spawnedCheckpoints.Count > 4)
            {
                Destroy(spawnedCheckpoints[0]);
                spawnedCheckpoints.RemoveAt(0);
            }
        }


        public float GetNextCheckpointDepth(float currentDepth)
        {
            return Mathf.CeilToInt(currentDepth / CHECKPOINTS_DISTANCE) * CHECKPOINTS_DISTANCE;
        }
        public float GetCurrentCheckpointDepth(float currentDepth)
        {
            return Mathf.FloorToInt(currentDepth / CHECKPOINTS_DISTANCE) * CHECKPOINTS_DISTANCE;
        }
        public void ClearSpawnedAreas(CheckpointController checkpoint)
        {
            if (checkpoint == null)
            {
                lastSpawnedDepth = 0;
            }
            else
            {
                lastSpawnedDepth = -checkpoint.transform.position.y + 40;
            }
             
            foreach (GameObject area in toDestroyAfterDeath) Destroy(area);
            toDestroyAfterDeath.Clear();
        }








        private void SpawnNextArea()
        {
            float distanceToCheckpoint = GetNextCheckpointDepth(lastSpawnedDepth) - lastSpawnedDepth;

            AreaSO area = GetRandomArea();
            while(area == prevPattern)
            {
                area = GetRandomArea();
                if (Time.deltaTime >= 1) throw new System.TimeoutException("SpawnNextArea get random area timeout");
            }
            prevPattern = area;

            float areaHeight = GetAreaHeight(area.prefab.transform);

            if (distanceToCheckpoint <= areaHeight + areasSpacing)
            {
                SpawnCheckpoint();
            }
            else
            {
                SpawnArea(area);
            }
        }
        private void SpawnArea(AreaSO area)
        {
            GameObject inst = Instantiate(area.prefab);
            toDestroyAfterDeath.Add(inst);
            float areaHeight = GetAreaHeight(inst.transform);

            inst.transform.position = new Vector3(0, -lastSpawnedDepth - areasSpacing);
            lastSpawnedDepth = -inst.transform.position.y + areaHeight;
        }
        private void SpawnCheckpoint()
        {
            GameObject inst = Instantiate(checkpointPrefab.gameObject);
            spawnedCheckpoints.Add(inst);
            float areaHeight = 40;

            float depth = GetNextCheckpointDepth(lastSpawnedDepth);
            inst.transform.position = new Vector3(0, -depth);
            lastSpawnedDepth = -inst.transform.position.y + areaHeight;

            CheckpointController controller = inst.GetComponent<CheckpointController>();
            controller.Refresh(depth);
        }
        private void SpawnBonus()
        {
            if (Random.value >= 0.1f) return;

            if (lastSpawnedDepth - lastSpawnedBonusDepth <= 250) return;

            Bonus bonus = GameManager.instance.sodb.bonuses[Random.Range(0, GameManager.instance.sodb.bonuses.Count - 1)];


            float randomX = Random.Range(-GameManager.WORLD_WIDTH / 2f / 1.5f, GameManager.WORLD_WIDTH / 2f / 1.5f);
            float height = lastSpawnedDepth - Random.Range(0.5f, 10);
            lastSpawnedBonusDepth = height;


            GameObject inst = Instantiate(bonus.prefab);
            inst.transform.position = new Vector3(randomX, -height);
            toDestroyAfterDeath.Add(inst);

            inst.GetComponent<BonusController>().model = bonus;
        }


        




        private AreaSO GetRandomArea()
        {
            // Get list of patterns could be spawned on this depth
            IEnumerable<AreaSO> availableAreas = GameManager.instance.sodb.patterns
                .Where(c => 
                    GameManager.instance.depth >= c.startDepth 
                    && (c.endDepth == 0 || GameManager.instance.depth <= c.endDepth));

            float maxPoints = availableAreas.Sum(c => c.chancePoints);
            float randomPoint = Random.Range(0, maxPoints);
            float currentPoint = randomPoint;

            foreach (AreaSO area in availableAreas)
            {
                currentPoint -= area.chancePoints;
                if (currentPoint <= 0) return area;
            }

            return null;
        }
        private float GetAreaHeight(Transform prefab)
        {
            //float minY = prefab.transform.Cast<Transform>().OrderBy(t => t.position.y).First().localPosition.y;

            //return -minY;

            Bounds bounds;
            Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();

            if (renderers.Length > 0)
            {
                bounds = renderers[0].bounds;
                for (int i = 1, ni = renderers.Length; i < ni; i++)
                {
                    bounds.Encapsulate(renderers[i].bounds);
                }
            }
            else
            {
                bounds = new Bounds();
            }

            return bounds.size.y;
        }
    }
}
