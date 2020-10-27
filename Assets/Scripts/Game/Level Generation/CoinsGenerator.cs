using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.Level.Generation
{
    public class CoinsGenerator : MonoBehaviour
    {
        public GameObject coinPrefab;
        private float lastSpawnedDepth;

        private BasicLevelGenerator gen;

        public void Init(BasicLevelGenerator generator)
        {
            gen = generator;
        }
        public void TrySpawnCoin(float depth)
        {
            if (Random.value >= 0.03f) return;
            if (depth - lastSpawnedDepth <= 200) return;


            float randomYOffset = Random.Range(2, 40);

            GameObject inst = Instantiate(coinPrefab);
            inst.transform.position = new Vector3(0, -(depth + randomYOffset));
            gen.spawnedObjects.Add(inst);


            lastSpawnedDepth = depth;
        }
    }
}