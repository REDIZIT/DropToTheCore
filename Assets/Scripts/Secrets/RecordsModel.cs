using InGame.SceneLoading;
using Newtonsoft.Json;
using UnityEngine;

namespace InGame.Secrets
{
    public class RecordsModel
    {
        [JsonProperty]
        private Record classicDepth, infinityDepth, hardcoreInfinityDepth;



        public bool IsRecord(SceneLoader.LoadGameType gameMode, float depth)
        {
            return depth > GetRecordByMode(gameMode);
        }
        public int GetRecord(SceneLoader.LoadGameType gameMode)
        {
            return GetRecordByMode(gameMode);
        }
        public bool UpdateRecord(SceneLoader.LoadGameType gameMode, float depth, bool forceSet = false)
        {
            return GetRecordByMode(gameMode).Update(Mathf.RoundToInt(depth), forceSet);
        }

        private ref Record GetRecordByMode(SceneLoader.LoadGameType mode)
        {
            switch (mode)
            {
                case SceneLoader.LoadGameType.Checkpoints: return ref classicDepth;
                case SceneLoader.LoadGameType.Infinity: return ref infinityDepth;
                case SceneLoader.LoadGameType.HardInfinity: return ref hardcoreInfinityDepth;
                default: throw new System.Exception($"Unable to define record for game mode '{mode}'");
            }
        }

        public struct Record
        {
            public int value;

            public bool Update(int newValue, bool forceSet = false)
            {
                if (forceSet)
                {
                    value = newValue;
                    return true;
                }

                if (newValue > value)
                {
                    value = newValue;
                    return true;
                }
                return false;
            }

            public static implicit operator int(Record r)
            {
                return r.value;
            }
        }
    }
}