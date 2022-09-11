using UnityEngine;

namespace InGame.Level.Generation
{
    public abstract class Generator : MonoBehaviour
    {
        protected BasicLevelGenerator generator;

        public void Init(BasicLevelGenerator generator)
        {
            this.generator = generator;
            OnInit();
        }

        public abstract void OnRevive();
        public abstract void OnUpdate(float lastSpawnedDepth);
        protected virtual void OnInit() { }
    }
}