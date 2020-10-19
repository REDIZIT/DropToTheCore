using UnityEngine;

namespace InGame.Level
{
    [CreateAssetMenu(menuName = "InGame/Area")]
    public class AreaSO : ScriptableObject
    {
        public GameObject prefab;

        public int startDepth, endDepth;

        public float chancePoints = 1;
    }

}