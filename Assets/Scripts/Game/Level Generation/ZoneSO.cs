using UnityEngine;

namespace InGame.Level.Generation
{
    [CreateAssetMenu(menuName = "InGame/Zone")]
    public class ZoneSO : ScriptableObject
    {
        public GameObject prefab;
        public float chance = 1;
    }
}