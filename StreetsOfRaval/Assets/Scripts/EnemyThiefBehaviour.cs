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
        private float m_EnemyDamage;
        private float m_EnemySpeed;

        //Animation names
        private const string m_IdleAnimationName = "idle";
        private const string m_WalkAnimationName = "walk";
        private const string m_Attack1AnimationName = "attack1";
        private const string m_Attack2AnimationName = "attack2";
        private const string m_HitAnimationName = "hit";
        private const string m_DieAnimationName = "die";



        private void Awake()
        {
            m_RigidBody = GetComponent<Rigidbody2D>();
            m_Animator = GetComponent<Animator>();
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

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

                    break;

                case EnemyMachineStates.CHASE:

                    m_Animator.Play(m_WalkAnimationName);

                    break;

                case EnemyMachineStates.FLEE:

                    m_Animator.Play(m_WalkAnimationName);

                    break;

                case EnemyMachineStates.ATTACK:
                    //Attack will set the velocity to zero, so it cant move while attacking
                    m_RigidBody.velocity = Vector3.zero;
                    m_Animator.Play(m_Attack1AnimationName);

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
            /*if (m_IsFlipped)
            {
                m_RigidBody.transform.eulerAngles = Vector3.up * 180;
            }
            else
            {
                m_RigidBody.transform.eulerAngles = Vector3.zero;
            }
            */

            switch (m_CurrentState)
            {
                case EnemyMachineStates.IDLE:

                   /* if (m_MovementAction.ReadValue<Vector2>().x != 0)
                    {
                        if (m_MovementAction.ReadValue<Vector2>().x < 0)
                            m_IsFlipped = true;
                        if (m_MovementAction.ReadValue<Vector2>().x > 0)
                            m_IsFlipped = false;
                        ChangeState(PlayerMachineStates.WALK);
                    }*/

                    break;

                case EnemyMachineStates.PATROL:

                    /*m_RigidBody.velocity = new Vector2(m_MovementAction.ReadValue<Vector2>().x * m_Speed, m_RigidBody.velocity.y);
                    //m_RigidBody.velocity = Vector2.right * m_MovementAction.ReadValue<Vector2>().x * m_Speed; 

                    if (m_RigidBody.velocity == Vector2.zero)
                        ChangeState(PlayerMachineStates.IDLE);*/

                    break;

                case EnemyMachineStates.CHASE:

                    /*if (m_RigidBody.velocity == Vector2.zero)
                        ChangeState(PlayerMachineStates.IDLE);*/

                    break;

                case EnemyMachineStates.ATTACK:

                    break;

                default:
                    break;
            }
        }
    }
}
