using InGame.Game;
using InGame.Game.Bonuses;
using UnityEngine;

namespace InGame.Level.Generation
{
    public class BonusGenerator : MonoBehaviour
    {
        private BasicLevelGenerator generator;

        private float nextSpawnBonusDepth;

        public void Init(BasicLevelGenerator generator)
        {
            this.generator = generator;
            nextSpawnBonusDepth = generator.player.Depth;
            CalculateNextDepth();
        }
        public void Reset()
        {
            nextSpawnBonusDepth = generator.player.Depth;
        }
        public void TrySpawnBonus(float lastSpawnedDepth)
        {
            if (generator.player.Depth >= nextSpawnBonusDepth)
            {
                SpawnBonus(lastSpawnedDepth);

                CalculateNextDepth();
            }
        }

        private void SpawnBonus(float minDepthToSpawn)
        {
            Bonus bonus = GameManager.instance.sodb.bonuses[Random.Range(0, GameManager.instance.sodb.bonuses.Count - 1)];


            float randomX = Random.Range(-GameManager.WORLD_WIDTH / 2f / 1.5f, GameManager.WORLD_WIDTH / 2f / 1.5f);
            float height = minDepthToSpawn - Random.Range(0.5f, 10);


            GameObject inst = Instantiate(bonus.prefab);
            inst.transform.position = new Vector3(randomX, -height);
            generator.spawnedObjects.Add(inst);

            inst.GetComponent<BonusController>().model = bonus;
        }
        private void CalculateNextDepth()
        {
            nextSpawnBonusDepth += 450 + Secrets.SecretsManager.Secrets.ShieldLevel * 100 + Random.Range(0, 100);
        }
    }
}