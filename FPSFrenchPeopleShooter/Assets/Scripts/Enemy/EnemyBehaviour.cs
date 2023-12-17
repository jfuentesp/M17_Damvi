using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Android;

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
    private enum EnemyMachineStates { NONE, IDLE, WALK, SHOOT, PATROL, CHASE, RUN, HIT, DEAD }
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
        if (distance < 5f)
            return true;
        return false;
    }

    private IEnumerator IdleCoroutine()
    {
        yield return new WaitForSeconds(2f);
        m_Agent.isStopped = false;
        ChangeState(EnemyMachineStates.PATROL);
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
                m_Animator.Play(m_IdleAnimationName);
                StartCoroutine(IdleCoroutine());
                break;

            case EnemyMachineStates.WALK:
                m_Animator.Play(m_WalkAnimationName);
                break;

            case EnemyMachineStates.SHOOT:
                //Attack will set the velocity to zero, so it cant move while attacking
                m_Moving.OnStopMovement();
                m_Animator.Play(m_Attack1AnimationName);
                //_Damaging.SetComboMultiplier(1);
                break;

            case EnemyMachineStates.HIT:
                //Will play the animation and then set the state to Idle
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_HitAnimationName);
                //m_Moving.OnKnockback(m_LastAttackSide, m_LastKnockbackPower);
                //StartCoroutine(OnPlayerHitCoroutine());
                break;

            case EnemyMachineStates.RUN:
                //Crouch sets the movement to zero and it doesn't move.
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_WalkAnimationName);
                break;

            case EnemyMachineStates.PATROL:
                //Attack will set the velocity to zero, so it cant move while attacking
                m_CurrentDestination = m_TargetDestinations[Random.Range(0, m_TargetDestinations.Count - 1)];
                m_Agent.SetDestination(m_CurrentDestination.position);
                m_Animator.Play(m_WalkAnimationName);
                break;

            case EnemyMachineStates.CHASE:
                //Attack will set the velocity to zero, so it cant move while attacking
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_WalkAnimationName);
                //m_Damaging.SetComboMultiplier(1);
                break;

            case EnemyMachineStates.DEAD:
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_DeadAnimationName);
                //m_IsInvulnerable = true;
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
          //      if (m_MovementAction.ReadValue<Vector2>().x != 0)
                    ChangeState(EnemyMachineStates.WALK);
                break;

            case EnemyMachineStates.WALK:
          //      m_Moving.OnMoveByForce(m_MovementAction.ReadValue<Vector2>());
                if (m_Rigidbody.velocity == Vector3.zero)
                    ChangeState(EnemyMachineStates.IDLE);
                break;

            case EnemyMachineStates.PATROL:
                if (CheckPatrolPosition())
                    ChangeState(EnemyMachineStates.IDLE);
                break;

            default:
                break;
        }
    }
    /* !!! FINISHING THE BUILD OF THE STATE MACHINE !!! */
}
