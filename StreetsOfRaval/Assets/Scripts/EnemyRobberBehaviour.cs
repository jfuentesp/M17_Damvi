using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* !!!! LIST OF ENEMIES:
 * 1. Thief: Turns invisible and static once you look at him. He follows you when you do not look at him and tries to stab you. sprite: enemy2
 * 2. Gang: Walks towards you an tries to stab you. If he is hit, he tries to runaway and throw you projectiles. sprite: enemy1
 * 3. Robber: Tank. Takes time to hit you, but he can resist more attacks. sprite: enemy3
 * 4. Ranger: Fires projectiles. You can hit the projectiles with the melee attack to block or crouch. sprite: enemy4
 */

namespace streetsofraval
{
    public class EnemyRobberBehaviour : MonoBehaviour
    {
        //Reference to this gameobject Rigidbody
        private Rigidbody2D m_RigidBody;
        //Reference to this gameobject Animator
        private Animator m_Animator;
        
        //States from Enemy statemachine
        private enum EnemyStatesMachine { IDLE, PATROL, CHASE, ATTACK, FLEE }
        private EnemyStatesMachine m_CurrentState;

        [Header("Enemy parameters")]
        private float m_EnemyMaxHitpoints;
        private float m_EnemyHitpoints;
        private float m_EnemyDamage;
        private float m_EnemySpeed;




        private void Awake()
        {
            m_RigidBody = GetComponent<Rigidbody2D>();
            m_Animator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
