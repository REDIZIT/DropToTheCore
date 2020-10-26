using InGame.Game;
using System.Collections.Generic;
using System.Linq;

namespace InGame.Level.Generation
{
    public class HardInfiniteLevelGenerator : BasicLevelGenerator
    {
        public override float GetPlayerStartDepth(float depth) { return 0; }
        protected override void GenerateNewPattern()
        {
            AreaSO area = GetRandomArea();
            SpawnPrefabAsNext(area.prefab);
        }

        protected override void OnDestroyLoop() { }
        private AreaSO GetRandomArea()
        {
            IEnumerable<AreaSO> availableAreas = GameManager.instance.sodb.patterns.SkipWhile(c => c.startDepth < 1000);

            return GetRandomArea(availableAreas);
        }
    }
}