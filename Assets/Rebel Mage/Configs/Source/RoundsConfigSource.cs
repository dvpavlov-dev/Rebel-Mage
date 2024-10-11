using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rebel_Mage.Configs 
{
    [CreateAssetMenu(menuName = "Configs/Rounds config", fileName = "Rounds config")]
    public class RoundsConfigSource : ScriptableObject
    { 
        public List<RoundParameters> RoundParametersList;

        [Serializable]
        public class RoundParameters
        {
            public string RoundName;
            public List<EnemyParameters> EnemyParameters;
        }

        [Serializable]
        public struct EnemyParameters
        {
            public SpawnType SpawnType;
            
            [Space]
            public EnemyType EnemyType;
            public int EnemyCount;
        }
    }

    public enum EnemyType
    {
        BASE_ENEMY,
        MELEE_ENEMY,
    }

    public enum SpawnType
    {
        CIRCLE,
        ONE_SIDE,
    }
}