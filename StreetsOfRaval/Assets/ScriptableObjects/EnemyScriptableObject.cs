using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace streetsofraval
{
    [CreateAssetMenu(fileName = "EnemyInfoScriptableObject", menuName = "Scriptable Objects/Scriptable EnemyInfo")]
    public class EnemyScriptableObject : ScriptableObject
    {
        [SerializeField]
        private int m_MaxHP;


        public int MaxHP => m_MaxHP;


    }
}

