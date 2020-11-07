using UnityEngine;

namespace InGame.Game.Bonuses
{
    [CreateAssetMenu(menuName = "InGame/Bonuses/Default")]
    public class Bonus : ScriptableObject
    {
        public GameObject prefab;

        public virtual BonusType BonusType { get; }

        public virtual bool TryApply(PlayerController player) { return false; }
    }
}