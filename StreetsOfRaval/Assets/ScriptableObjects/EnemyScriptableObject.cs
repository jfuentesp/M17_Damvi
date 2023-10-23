using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace streetsofraval
{
    [CreateAssetMenu(fileName = "EnemyInfoScriptableObject", menuName = "Scriptable Objects/Scriptable EnemyInfo")]
    public class EnemyScriptableObject : ScriptableObject
    {
        [SerializeField]
        private float m_EnemyMaxHP;
        [SerializeField]
        private float m_EnemyDamage;
        [SerializeField]
        private float m_EnemySpeed;
        [SerializeField]
        private string m_EnemyIdleAnimationName;
        [SerializeField]
        private string m_EnemyWalkingAnimationName;
        [SerializeField]
        private string m_EnemyAttackAnimationName;
        [SerializeField]
        private string m_EnemyHitAnimationName;
        [SerializeField]
        private string m_EnemyDieAnimationName;



        public float EnemyMaxHP => m_EnemyMaxHP;
        public float EnemyDamage => m_EnemyDamage;
        public float EnemySpeed => m_EnemySpeed;
        public string EnemyIdleAnimationName => m_EnemyIdleAnimationName;
        public string EnemyWalkingAnimationName => m_EnemyWalkingAnimationName;
        public string EnemyAttackAnimationName => m_EnemyAttackAnimationName;
        public string EnemyHitAnimationName => m_EnemyHitAnimationName;
        public string EnemyDieAnimationName => m_EnemyDieAnimationName;
    }
}

