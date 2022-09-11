using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InGame.Level.Generation
{
    public abstract class BasicLevelGenerator : MonoBehaviour
    {
        public PlayerController player;
        public BonusGenerator bonusGenerator;
        public CoinsGenerator coinsGenerator;
        public ZonesGenerator zonesGenerator;

        /// <summary>These objects will be destroyed after relive</summary>
        public List<GameObject> spawnedObjects = new List<GameObject>();

        /// <summary>Last depth where pattern has been spawned</summary>
        protected float lastSpawnedDepth;

        public const float AREA_SPACING = 20;


        private void Awake()
        {
            bonusGenerator.Init(this);
            coinsGenerator.Init(this);
            zonesGenerator.Init(this);
        }
        private void Update()
        {
            GenerationLoop();
            bonusGenerator.OnUpdate(lastSpawnedDepth);
            coinsGenerator.TrySpawnCoin(lastSpawnedDepth);
            zonesGenerator.OnUpdate(lastSpawnedDepth);
            DestroyLoop();
        }


        protected abstract void GenerateNewPattern();
        protected abstract void OnDestroyLoop();
        protected AreaSO GetRandomArea(IEnumerable<AreaSO> areas)
        {
            float maxPoints = areas.Sum(c => c.chancePoints);
            float randomPoint = Random.Range(0, maxPoints);
            float currentPoint = randomPoint;

            foreach (AreaSO area in areas)
            {
                currentPoint -= area.chancePoints;
                if (currentPoint <= 0) return area;
            }

            return null;
        }
        public abstract float GetPlayerStartDepth(float depth);



        private void GenerationLoop()
        {
            if (-player.transform.position.y >= lastSpawnedDepth - 50)
            {
                GenerateNewPattern();
            }
        }
        private void DestroyLoop()
        {
            if (spawnedObjects.Count > 0)
            {
                if (spawnedObjects[0] == null || spawnedObjects[0].transform == null)
                {
                    spawnedObjects.RemoveAll(c => c == null || c.transform == null);
                }

                if (-player.transform.position.y - -spawnedObjects[0].transform.position.y >= 200)
                {
                    Destroy(spawnedObjects[0]);
                    spawnedObjects.RemoveAt(0);
                }
            }

            OnDestroyLoop();
        }






        public void ResetByRevive(CheckpointController checkpoint)
        {
            if (checkpoint == null)
            {
                lastSpawnedDepth = 0;
            }
            else
            {
                lastSpawnedDepth = -checkpoint.transform.position.y + 40;
            }

            bonusGenerator.OnRevive();
            zonesGenerator.OnRevive();
        }
        public void ClearSpawnedObjects()
        {
            foreach (GameObject area in spawnedObjects) Destroy(area);
            spawnedObjects.Clear();
        }


        /// <summary>Spawn <paramref name="part"/> after last spawned part and returns instance GameObject</summary>
        public GameObject SpawnPrefabAsNext(GameObject prefab, float areaHeight = -1)
        {
            GameObject inst = Instantiate(prefab);
            spawnedObjects.Add(inst);

            areaHeight = areaHeight == -1 ? inst.GetHeight() : areaHeight;


            inst.transform.position = new Vector3(0, -lastSpawnedDepth - AREA_SPACING);
            lastSpawnedDepth = -inst.transform.position.y + areaHeight;

            return inst;
        }
    }
}