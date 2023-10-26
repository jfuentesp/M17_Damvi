using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

/* !!!! LIST OF ENEMIES:
 * 1. Thief: Turns invisible and static once you look at him. He follows you when you do not look at him and tries to stab you. sprite: enemy2
 * 2. Gang: Walks towards you an tries to stab you. If he is hit, he tries to runaway and throw you projectiles. sprite: enemy1
 * 3. Robber: Tank. Takes time to hit you, but he can resist more attacks. sprite: enemy3
 * 4. Ranger: Fires projectiles. You can hit the projectiles with the melee attack to block or crouch. sprite: enemy4
 */

namespace streetsofraval
{
    public class EnemyThiefBehaviour : MonoBehaviour
    {
        //Reference to this gameobject Rigidbody
        private Rigidbody2D m_RigidBody;
        //Reference to this gameobject Animator
        private Animator m_Animator;
        //Reference to this sprite renderer
        private SpriteRenderer m_SpriteRenderer;

        //States from Enemy statemachine
        private enum EnemyMachineStates { IDLE, PATROL, CHASE, ATTACK, FLEE }
        private EnemyMachineStates m_CurrentState;

        [Header("Enemy parameters")]
        private float m_EnemyMaxHitpoints;
        private float m_EnemyHitpoints;
        [SerializeField]
        private float m_EnemyDamage;
        [SerializeField]
        private float m_InitialSpeed;
        private float m_EnemySpeed;
        private int m_EnemySpawnPoint;

        //Animation names
        private const string m_IdleAnimationName = "idle";
        private const string m_WalkAnimationName = "walk";
        private const string m_Attack1AnimationName = "attack1";
        //private const string m_Attack2AnimationName = "attack2";
        private const string m_HitAnimationName = "hit";
        private const string m_DieAnimationName = "die";

        private bool m_IsFlipped;

        //Waypoint to patrol
        private Vector2 m_SpawnPosition;
        private float m_Direction;

        //Player reference
        PlayerBehaviour m_Player;

        //Child references
        HitboxInfo m_Hitbox;
        AreaBehaviour m_ChaseArea;
        AreaBehaviour m_AttackArea;

        private void Awake()
        {
            m_RigidBody = GetComponent<Rigidbody2D>();
            m_Animator = GetComponent<Animator>();
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_Hitbox = this.transform.GetChild(0).GetComponent<HitboxInfo>();
            m_AttackArea = this.transform.GetChild(1).GetComponent<AreaBehaviour>();
            m_ChaseArea = this.transform.GetChild(2).GetComponent<AreaBehaviour>();
            m_SpawnPosition = transform.position;
            m_Direction = 1;
            m_EnemySpeed = m_InitialSpeed;
            //Ternary. Equals to: if (spawnpoint is 0, then false. Else true) and saves the result inside the variable
            m_IsFlipped = m_EnemySpawnPoint == 0 ? false : true;
        }

        // Start is called before the first frame update
        void Start()
        {
            m_Player = PlayerBehaviour.PlayerInstance;
            InitState(EnemyMachineStates.PATROL);
        }

        // Update is called once per frame
        void Update()
        {
            UpdateState();
            Debug.Log(m_CurrentState);
        }

        public void InitEnemy(EnemyScriptableObject enemyInfo)
        {
            m_EnemyMaxHitpoints = enemyInfo.EnemyMaxHP;
            m_EnemyHitpoints = m_EnemyMaxHitpoints;
            m_EnemyDamage = enemyInfo.EnemyDamage;
            m_EnemySpeed = enemyInfo.EnemySpeed;
            m_SpriteRenderer.color = enemyInfo.SpriteColor;
        } 

        //Simple function that manages the damage the enemy receives
        public void EnemyIsDamaged(int damage)
        {
            m_EnemyHitpoints -= damage;
            if(m_EnemyHitpoints <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        public void EndOfHit()
        {
            ChangeState(EnemyMachineStates.CHASE);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("PlayerHitbox"))
            {
                EnemyIsDamaged(collision.GetComponent<HitboxInfo>().HitboxDamage);
            }
            if (collision.CompareTag("PlayerProjectile"))
            {
                //Projectiles only do damage once, so we disable it after getting the damage
                EnemyIsDamaged(collision.GetComponent<PlayerBulletBehaviour>().BulletDamage);
                collision.gameObject.SetActive(false);
            }
        }

        /* !!! BUILDING UP STATE MACHINE !!! Always change state with the function ChangeState */
        private void ChangeState(EnemyMachineStates newState)
        {
            //if the actual state is the same as the state we are trying to set, it exits the function
            if (newState == m_CurrentState)
                return;
            //First, it will do the actions to exit the current state, then will initiate the new state.
            ExitState();
            InitState(newState);
        }

        Coroutine m_PatrolCoroutine;
        Coroutine m_AttackCoroutine;

        /* InitState will run every instruction that has to be started ONLY when enters a state */
        private void InitState(EnemyMachineStates currentState)
        {
            //We declare that the current state of the object is the new state we declare on the function
            m_CurrentState = currentState;

            //Then it will compare the current state to run the state actions
            switch (m_CurrentState)
            {
                case EnemyMachineStates.IDLE:

                    m_RigidBody.velocity = Vector3.zero;

                    m_Animator.Play(m_IdleAnimationName);

                    break;

                case EnemyMachineStates.PATROL:

                    m_Animator.Play(m_WalkAnimationName);
                    m_EnemySpeed = m_InitialSpeed;
                    //Ternary. Equals to: if (spawnpoint is 0, then direction -1. Else 1) and saves the result inside the variable
                    m_Direction = m_EnemySpawnPoint == 0 ? -1 : 1;
                    m_PatrolCoroutine = StartCoroutine(PatrolCoroutine());

                    break;

                case EnemyMachineStates.CHASE:

                    if (m_PatrolCoroutine != null)
                        StopCoroutine(m_PatrolCoroutine);
                    m_Animator.Play(m_WalkAnimationName);
                    m_EnemySpeed = m_InitialSpeed;

                    break;

                case EnemyMachineStates.FLEE:

                    m_Animator.Play(m_WalkAnimationName);
                    m_EnemySpeed = m_InitialSpeed / 2;

                    break;

                case EnemyMachineStates.ATTACK:
                    //Attack will set the velocity to zero, so it cant move while attacking
                    m_RigidBody.velocity = Vector3.zero;
                    m_Hitbox.SetDamage((int)m_EnemyDamage);
                    m_Animator.Play(m_Attack1AnimationName); // This could be run from a coroutine if necessary
                    //m_AttackCoroutine = StartCoroutine(AttackCoroutine()); //If we would set this with a corroutine (make sure to StopCoroutine(m_AttackCoroutine); in Walkin states
                    break;

                default:
                    break;
            }
        }

        /* ExitState will run every instruction that has to be started ONLY when exits a state */
        private void ExitState()
        {
            switch (m_CurrentState)
            {
                case EnemyMachineStates.IDLE:

                    break;

                case EnemyMachineStates.PATROL:
                
                    break;

                case EnemyMachineStates.CHASE:

                    break;

                case EnemyMachineStates.FLEE:

                    break;

                case EnemyMachineStates.ATTACK:
                    
                    break;

                default:
                    break;
            }
        }

        /* UpdateState will control every frame since it will be called from Update() and will control when it changes the state */
        private void UpdateState()
        {
            if (m_IsFlipped)
            {
                m_RigidBody.transform.eulerAngles = Vector3.up * 180;
            }
            else
            {
                m_RigidBody.transform.eulerAngles = Vector3.zero;
            }
        
            switch (m_CurrentState)
            {
                case EnemyMachineStates.IDLE:
                  
                    if (m_PatrolCoroutine != null)
                        StopCoroutine(m_PatrolCoroutine);
                    if (m_ChaseArea.PlayerDetected)
                        ChangeState(EnemyMachineStates.CHASE);

                    ChangeState(EnemyMachineStates.PATROL);

                    break;

                case EnemyMachineStates.PATROL:              

                    m_RigidBody.velocity = new Vector2(m_Direction * m_EnemySpeed, m_RigidBody.velocity.y);

                    if (m_RigidBody.velocity == Vector2.zero)
                        ChangeState(EnemyMachineStates.IDLE);
                    if (m_ChaseArea.PlayerDetected)
                        ChangeState(EnemyMachineStates.CHASE);

                    break;

                case EnemyMachineStates.CHASE:

                    m_RigidBody.velocity = new Vector2(m_Player.transform.position.x - transform.position.x , 0).normalized 
                        + new Vector2(0, m_RigidBody.velocity.y) * m_EnemySpeed;
                    m_IsFlipped = m_RigidBody.velocity.x < 0 ? true : false;

                    if (m_AttackArea.PlayerDetected)
                        ChangeState(EnemyMachineStates.ATTACK);
                    if (!m_ChaseArea.PlayerDetected)
                        ChangeState(EnemyMachineStates.PATROL);

                    break;

                case EnemyMachineStates.ATTACK:

                    break;

                default:
                    break;
            }
        }
        private IEnumerator PatrolCoroutine()
        {          
            while (true)
            {
                m_Direction *= -1;
                //Ternary. Equals to: if (direction is lesser than 0, then true. Else false) and saves the result inside the variable
                m_IsFlipped = m_Direction < 0 ? true : false;
                yield return new WaitForSeconds(5f);            
            }
        }

        /*private IEnumerator AttackCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(2f);
                m_RigidBody.velocity = Vector3.zero;
                m_Animator.Play(m_Attack1AnimationName);
                Debug.Log("Ataco");               
            }
        }*/
    }
}
