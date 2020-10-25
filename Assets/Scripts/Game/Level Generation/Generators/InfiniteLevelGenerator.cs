using InGame.Game;
using System.Collections.Generic;
using System.Linq;

namespace InGame.Level.Generation
{
    public class InfiniteLevelGenerator : BasicLevelGenerator
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
            IEnumerable<AreaSO> availableAreas = GameManager.instance.sodb.patterns
                .Where(c => GameManager.instance.depth >= c.startDepth);

            return GetRandomArea(availableAreas);
        }
    }
}