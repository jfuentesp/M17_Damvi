using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Android;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MovableBehaviour))]
[RequireComponent(typeof(HealthBehaviour))]
[RequireComponent(typeof(DamageableBehaviour))]
public class EnemyBehaviour : MonoBehaviour
{
    //Enemy Rigidbody
    private Rigidbody m_Rigidbody;

    //Enemy animator
    private Animator m_Animator;

    //Variables for the current state and an Enum for setting the Player States
    private enum EnemyMachineStates { NONE, IDLE, SHOOT, PATROL, CHASE, RUN, HIT, DEAD }
    private EnemyMachineStates m_CurrentState;

    //Animation names
    private const string m_IdleAnimationName = "idle";
    private const string m_WalkAnimationName = "walk";
    private const string m_Attack1AnimationName = "shoot";
    private const string m_HitAnimationName = "hit";
    private const string m_DeadAnimationName = "death";

    //Components
    private MovableBehaviour m_Moving;
    private HealthBehaviour m_Health;
    private RagdollController m_Ragdoll;
    private SpotableBehaviour m_PlayerSpotted;
    private ShootingBehaviour m_Shooting;

    //Target destinations to patrol
    [SerializeField]
    private List<Transform> m_TargetDestinations;
    private Transform m_CurrentDestination;

    //Agent for Patrol script
    private NavMeshAgent m_Agent;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponentInChildren<Animator>();
        m_Moving = GetComponent<MovableBehaviour>();
        m_Health = GetComponent<HealthBehaviour>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Ragdoll = GetComponent<RagdollController>();
        m_PlayerSpotted = GetComponent<SpotableBehaviour>();
        m_Shooting = GetComponent<ShootingBehaviour>();
        m_Agent.speed = 2.5f;

        m_Health.OnDeath += Death;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitState(EnemyMachineStates.PATROL);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    private bool CheckPatrolPosition()
    {
        float distance = Vector3.Distance(transform.position, m_CurrentDestination.position);
        if (distance < 3f)
            return true;
        return false;
    }

    private IEnumerator IdleCoroutine()
    {
        yield return new WaitForSeconds(2f);
        m_Agent.isStopped = false;
        m_Agent.speed = 2.5f;
        ChangeState(EnemyMachineStates.PATROL);
    }

    private IEnumerator ShootCoroutine()
    {
        float accuracy = Random.value;
        float variation = Random.Range(-1f, 1f);
        if (accuracy < 0.5f)
        {
            variation = 0f;
            m_Shooting.OnEnemyShoot(variation);
        } else
        {
            m_Shooting.OnEnemyShoot(variation);
        }
        yield return new WaitForSeconds(2f);
        ChangeState(EnemyMachineStates.CHASE);
    }

    private void Death()
    {
        ChangeState(EnemyMachineStates.DEAD);
    }

    /******** !!! BUILDING UP STATE MACHINE !!! Always change state with the function ChangeState ********/
    private void ChangeState(EnemyMachineStates newState)
    {
        //if the actual state is the same as the state we are trying to set, it exits the function
        if (newState == m_CurrentState)
            return;
        //First, it will do the actions to exit the current state, then will initiate the new state.
        ExitState();
        InitState(newState);
    }

    private void InitState(EnemyMachineStates currentState)
    {
        //We declare that the current state of the object is the new state we declare on the function
        m_CurrentState = currentState;

        //Then it will compare the current state to run the state actions
        switch (m_CurrentState)
        {
            case EnemyMachineStates.IDLE:
                m_Agent.isStopped = true;
                m_Agent.speed = 0;
                m_Animator.Play(m_IdleAnimationName);
                StartCoroutine(IdleCoroutine());
                break;

            case EnemyMachineStates.SHOOT:
                //Attack will set the velocity to zero, so it cant move while attacking
                m_Agent.isStopped = true;
                m_Agent.speed = 0;
                m_Animator.Play(m_Attack1AnimationName);
                StartCoroutine(ShootCoroutine());
                break;

            case EnemyMachineStates.PATROL:
                m_Agent.isStopped = false;
                m_Agent.speed = 2.5f;
                m_CurrentDestination = m_TargetDestinations[Random.Range(0, m_TargetDestinations.Count - 1)];
                m_Agent.SetDestination(m_CurrentDestination.position);
                m_Animator.Play(m_WalkAnimationName);
                break;

            case EnemyMachineStates.CHASE:
                m_Agent.isStopped = false;
                m_Agent.speed = 3f;
                m_Animator.Play(m_WalkAnimationName);
                break;

            case EnemyMachineStates.DEAD:
                m_Moving.OnStopMovement();
                m_Agent.isStopped = true;
                m_Agent.speed = 0;
                m_Ragdoll.Die();
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
            //If needs to leave a Coroutine or do something at the finish of the action...
            case EnemyMachineStates.IDLE:
                break;
            default:
                break;
        }
    }

    /* UpdateState will control every frame since it will be called from Update() and will control when it changes the state */
    private void UpdateState()
    {
        //if (!m_IsInvulnerable)
        //    m_Moving.OnFlipCharacter(m_MovementAction.ReadValue<Vector2>());

        switch (m_CurrentState)
        {
            case EnemyMachineStates.IDLE:
                break;

            case EnemyMachineStates.PATROL:
                if (CheckPatrolPosition())
                    ChangeState(EnemyMachineStates.IDLE);
                if (m_PlayerSpotted.PlayerIsInRange)
                    ChangeState(EnemyMachineStates.CHASE);
                break;

            case EnemyMachineStates.CHASE:
                if (!m_PlayerSpotted.PlayerIsInRange)
                    ChangeState(EnemyMachineStates.PATROL);
                if (m_Shooting.PlayerIsInRange)
                    ChangeState(EnemyMachineStates.SHOOT);
                break;

            case EnemyMachineStates.SHOOT:
                if (!m_Shooting.PlayerIsInRange)
                    ChangeState(EnemyMachineStates.CHASE);
                break;

            default:
                break;
        }
    }
    /* !!! FINISHING THE BUILD OF THE STATE MACHINE !!! */
}
