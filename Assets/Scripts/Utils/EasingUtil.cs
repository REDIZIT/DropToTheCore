using UnityEngine;

namespace InGame.Utils
{
    public static class EasingUtil
    {
        public static float SlowingAtEnd(float currentN, float maxN)
        {
            float temp = currentN / maxN;
            temp = 1 - Mathf.Pow(1 - temp, 2);

            return temp;
        }
    }

}