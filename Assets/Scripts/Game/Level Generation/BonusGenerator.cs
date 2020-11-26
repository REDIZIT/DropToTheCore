using InGame.Game;
using InGame.Game.Bonuses;
using UnityEngine;

namespace InGame.Level.Generation
{
    public class BonusGenerator : MonoBehaviour
    {
        private BasicLevelGenerator generator;

        private float lastSpawnedBonusDepth;

        public void Init(BasicLevelGenerator generator)
        {
            this.generator = generator;
        }
        public void TrySpawnBonus(float lastSpawnedDepth)
        {
            if (Random.value >= 0.015f) return;

            if (lastSpawnedDepth - lastSpawnedBonusDepth <= 250) return;

            Bonus bonus = GameManager.instance.sodb.bonuses[Random.Range(0, GameManager.instance.sodb.bonuses.Count - 1)];


            float randomX = Random.Range(-GameManager.WORLD_WIDTH / 2f / 1.5f, GameManager.WORLD_WIDTH / 2f / 1.5f);
            float height = lastSpawnedDepth - Random.Range(0.5f, 10);
            lastSpawnedBonusDepth = height;


            GameObject inst = Instantiate(bonus.prefab);
            inst.transform.position = new Vector3(randomX, -height);
            generator.spawnedObjects.Add(inst);

            inst.GetComponent<BonusController>().model = bonus;
        }
    }
}