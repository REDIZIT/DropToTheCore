using InGame.Game;
using InGame.SceneLoading;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InGame.Level.Generation
{
    public class CheckpointLevelGenerator : BasicLevelGenerator
    {
        public CheckpointController checkpointPrefab;
        public GameObject lavaPrefab;

        private List<GameObject> spawnedCheckpoints = new List<GameObject>();

        private AreaSO prevPattern;
        private bool isEndReached;
        private float maxDepth = 1e+10F;

        public const float CHECKPOINTS_DISTANCE = 500;


        private void Awake()
        {
            maxDepth = GameManager.instance.sodb.patterns.Max(c => c.endDepth);

            lastSpawnedDepth = SceneLoader.LoadOnDepth;
        }
        private void Start()
        {
            FirstGenerationLoop();
        }

        private void FirstGenerationLoop()
        {
            if (lastSpawnedDepth - 50 >= maxDepth)
            {
                GenerateNewPattern();
                FirstGenerationLoop();
            }
        }

        protected override void GenerateNewPattern()
        {
            if (isEndReached) return;

            if (lastSpawnedDepth - 50 >= maxDepth)
            {
                SpawnLava();
                isEndReached = true;
                return;
            }
            SpawnNextArea();
        }

        protected override void OnDestroyLoop()
        {
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

            float areaHeight = area.prefab.GetHeight();

            if (distanceToCheckpoint <= areaHeight + AREA_SPACING)
            {
                SpawnCheckpoint();
            }
            else
            {
                SpawnPrefabAsNext(area.prefab);
            }
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
        
        private void SpawnLava()
        {
            GameObject inst = Instantiate(lavaPrefab);
            inst.transform.position = new Vector3(0, -lastSpawnedDepth - AREA_SPACING);
            lastSpawnedDepth = -inst.transform.position.y + 1000;
        }







        private AreaSO GetRandomArea()
        {
            // Get list of patterns could be spawned on this depth
            IEnumerable<AreaSO> availableAreas = GameManager.instance.sodb.patterns
                .Where(c => 
                    GameManager.instance.depth >= c.startDepth 
                    && (c.endDepth == 0 || GameManager.instance.depth <= c.endDepth));

            return GetRandomArea(availableAreas);
        }

        public override float GetPlayerStartDepth(float depth)
        {
            float result = GetCurrentCheckpointDepth(depth);
            return result;
        }
    }
}
