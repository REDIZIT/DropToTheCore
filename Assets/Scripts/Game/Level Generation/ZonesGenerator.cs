using System.Collections.Generic;
using UnityEngine;

namespace InGame.Level.Generation
{
    public class ZonesGenerator : Generator
    {
        [SerializeField] private List<ZoneSO> zones = new List<ZoneSO>();

        private float nextSpawnBonusDepth;

        protected override void OnInit()
        {
            nextSpawnBonusDepth = Mathf.Max(3000, generator.player.Depth);
            CalculateNextDepth();
        }
        public override void OnRevive()
        {
            OnInit();
        }

        public override void OnUpdate(float lastSpawnedDepth)
        {
            if (generator.player.Depth >= nextSpawnBonusDepth)
            {
                SpawnZone(lastSpawnedDepth);

                CalculateNextDepth();
            }
        }

        private void CalculateNextDepth()
        {
            nextSpawnBonusDepth += Random.Range(150, 450);
        }

        private void SpawnZone(float depth)
        {
            ZoneSO zone = zones.Random(z => z.chance);
            GameObject inst = Instantiate(zone.prefab);
            inst.transform.position = new Vector3(0, -depth);
        }
    }
}