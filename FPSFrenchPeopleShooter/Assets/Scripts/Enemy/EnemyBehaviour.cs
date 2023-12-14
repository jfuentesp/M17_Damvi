using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class EnemyBehaviour : MonoBehaviour
{
    //Enemy animator
    private Animator m_Animator;

    //Variables for the current state and an Enum for setting the Player States
    private enum EnemyMachineStates { NONE, IDLE, WALK, ATTACK1, ATTACK2, COMBO1, COMBO2, SUPER, JUMP, CROUCHATTACK1, CROUCHATTACK2, CROUCH, HIT, DEAD }
    private EnemyMachineStates m_CurrentState;

    //Animation names
    private const string m_IdleAnimationName = "idle";
    private const string m_WalkAnimationName = "walk";
    private const string m_JumpAnimationName = "jump";
    private const string m_Attack1AnimationName = "attack1";
    private const string m_Attack2AnimationName = "attack2";
    private const string m_Combo1AnimationName = "combo1";
    private const string m_Combo2AnimationName = "combo2";
    private const string m_SuperAnimationName = "super";
    private const string m_CrouchAnimationName = "crouch";
    private const string m_HitAnimationName = "hit";
    private const string m_DeadAnimationName = "death";
    private const string m_CrouchAttack1AnimationName = "crouchattack1";
    private const string m_CrouchAttack2AnimationName = "crouchattack2";

    private void Awake()
    {
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
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_IdleAnimationName);
                break;

            case EnemyMachineStates.WALK:
                m_Animator.Play(m_WalkAnimationName);
                break;

            case EnemyMachineStates.JUMP:
                m_Animator.Play(m_JumpAnimationName);
                //m_Jumping.JumpByForce();
                //OnObjectiveCheck(EMission.JUMP);
                break;

            case EnemyMachineStates.ATTACK1:
                //Attack will set the velocity to zero, so it cant move while attacking
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_Attack1AnimationName);
                //_Damaging.SetComboMultiplier(1);
                break;

            case EnemyMachineStates.ATTACK2:
                //Attack will set the velocity to zero, so it cant move while attacking
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_Attack2AnimationName);
                //m_Damaging.SetComboMultiplier(1.5f);
                break;

            case EnemyMachineStates.COMBO1:
                //Attack will set the velocity to zero, so it cant move while attacking
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_Combo1AnimationName);
                //Then we call for the shooting action and we pass the spawnpoint. We do substract the mana in the UpdateState().
                //m_Shooting.Shoot();
                //m_Mana.OnChangeMana(-m_ManaCost.ManaCost);
                //m_OnEnergyUsed.Raise();
                //OnObjectiveCheck(EMission.SHOOT);
                break;

            case EnemyMachineStates.COMBO2:
                //Attack will set the velocity to zero, so it cant move while attacking
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_Combo2AnimationName);
                //m_Damaging.SetComboMultiplier(1.5f);
                break;

            case EnemyMachineStates.HIT:
                //Will play the animation and then set the state to Idle
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_HitAnimationName);
                //m_Moving.OnKnockback(m_LastAttackSide, m_LastKnockbackPower);
                //StartCoroutine(OnPlayerHitCoroutine());
                break;

            case EnemyMachineStates.CROUCH:
                //Crouch sets the movement to zero and it doesn't move.
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_CrouchAnimationName);
                break;

            case EnemyMachineStates.CROUCHATTACK1:
                //Attack will set the velocity to zero, so it cant move while attacking
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_CrouchAttack1AnimationName);
                //m_Damaging.SetComboMultiplier(1);
                break;

            case EnemyMachineStates.CROUCHATTACK2:
                //Attack will set the velocity to zero, so it cant move while attacking
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_CrouchAttack2AnimationName);
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
           //     if (m_Rigidbody.velocity == Vector2.zero)
                    ChangeState(EnemyMachineStates.IDLE);
                break;

            case EnemyMachineStates.JUMP:
           //     if (m_Rigidbody.velocity == Vector2.zero)
                    ChangeState(EnemyMachineStates.IDLE);
           //     if (m_MovementAction.ReadValue<Vector2>().x != 0)
                    ChangeState(EnemyMachineStates.WALK);
                break;

            case EnemyMachineStates.CROUCH:
                //This gets the gameobject of the pickup, just as it would do in OnTriggerEnter/Stay, but with less load since it's a "Raycast"
           /*     if (Physics2D.CircleCast(transform.position, 0.5f, Vector2.up, 0.5f, m_PickupLayerMask))
                {
                    GameObject pickup = Physics2D.CircleCast(transform.position, 0.5f, Vector2.up, 0.5f, m_PickupLayerMask).collider.gameObject;
                    pickup.GetComponent<PickupBehaviour>().GetPickup(m_PlayerSelect);
                    Destroy(pickup.gameObject);
                    m_OnGUIUpdate.Raise();
                } */
                break;

            default:
                break;
        }
    }
    /* !!! FINISHING THE BUILD OF THE STATE MACHINE !!! */
}
