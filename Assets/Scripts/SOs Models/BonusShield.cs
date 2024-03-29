﻿using UnityEngine;

namespace InGame.Game.Bonuses
{
    [CreateAssetMenu(menuName = "InGame/Bonuses/Shield")]
    public class BonusShield : Bonus
    {
        public override BonusType BonusType => BonusType.Shield;

        public override bool TryApply(PlayerController player)
        {
            player.shield.Charge();
            return true;
        }
    }
}