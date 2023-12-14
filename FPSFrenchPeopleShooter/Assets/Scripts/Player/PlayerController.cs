using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(MovableBehaviour))]
[RequireComponent(typeof(ShootingBehaviour))]
public class PlayerController : MonoBehaviour
{
    //Reference to the InputSystem
    [Header("Reference to the Input System")]
    [SerializeField]
    private InputActionAsset m_InputAsset;
    private InputActionAsset m_Input;
    private InputAction m_MovementAction;
    public InputAction MovementAction => m_MovementAction;
    private InputAction m_CameraActionY;
    private InputAction m_CameraActionX;
    private InputActionMap m_CurrentActionMap;

    //Setting up the rigid body variable
    private Rigidbody m_Rigidbody;
    private CapsuleCollider m_Collider;

    [Header("Reference to the player camera")]
    [SerializeField]
    private Camera m_Camera;

    //Declaration of components
    private MovableBehaviour m_Moving;
    private ShootingBehaviour m_Shooting;

    //Enemy animator
    private Animator m_Animator;

    //Variables for the current state and an Enum for setting the Player States
    private enum PlayerMachineStates { NONE, IDLE, WALK, ATTACK1, ATTACK2, COMBO1, COMBO2, SUPER, JUMP, CROUCHATTACK1, CROUCHATTACK2, CROUCH, HIT, DEAD }
    private PlayerMachineStates m_CurrentState;

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
        //Loading components
        m_Moving = GetComponent<MovableBehaviour>();
        m_Shooting = GetComponent<ShootingBehaviour>();

        //Setting the Input Controls
        Assert.IsNotNull(m_InputAsset);
        m_Input = Instantiate(m_InputAsset);
        m_CurrentActionMap = m_Input.FindActionMap("Character");
        m_MovementAction = m_CurrentActionMap.FindAction("Movement");
        m_CameraActionY = m_CurrentActionMap.FindAction("RotationY");
        m_CameraActionX = m_CurrentActionMap.FindAction("RotationX");
    }

    private void OnEnable()
    {
        m_CurrentActionMap.FindAction("Shoot1").performed += Shoot1;
        //m_CurrentActionMap.FindAction("Shoot2").performed += Shoot2;
        //m_CurrentActionMap.FindAction("Jump").performed += Jump;
        //m_CurrentActionMap.FindAction("Crouch").started += Crouch;
        //m_CurrentActionMap.FindAction("Crouch").canceled += ReturnToIdleState;
        //m_CurrentActionMap.FindAction("Interact").performed += Interact;
        //m_CurrentActionMap.FindAction("Inventory").performed += Inventory;
        m_CurrentActionMap.Enable();
    }

    private void OnDisable()
    {
        m_CurrentActionMap.FindAction("Shoot1").performed -= Shoot1;
        //m_CurrentActionMap.FindAction("Shoot2").performed -= Shoot2;
        //m_CurrentActionMap.FindAction("Jump").performed -= Jump;
        //m_CurrentActionMap.FindAction("Crouch").started -= Crouch;
        //m_CurrentActionMap.FindAction("Crouch").canceled -= ReturnToIdleState;
        //m_CurrentActionMap.FindAction("Interact").performed -= Interact;
        //m_CurrentActionMap.FindAction("Inventory").performed -= Inventory;
        m_CurrentActionMap.Disable();
    }

    void Start()
    {
        //We set this in order to make the cursor to disappear and be locked
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_MovementAction.ReadValue<Vector3>() != Vector3.zero)
            m_Moving.OnMove(m_MovementAction.ReadValue<Vector3>());     

        if(m_CameraActionY.ReadValue<float>() != 0)
            m_Moving.OnRotateYaw(m_CameraActionY.ReadValue<float>());

        if (m_CameraActionX.ReadValue<float>() != 0)
            m_Moving.OnRotatePitch(m_CameraActionX.ReadValue<float>());

        //Debug.Log("Yaw: " + m_CameraActionY.ReadValue<float>() + " | Pitch: " + m_CameraActionX.ReadValue<float>());
    }

    /******** !!! BUILDING UP STATE MACHINE !!! Always change state with the function ChangeState ********/
    private void ChangeState(PlayerMachineStates newState)
    {
        //if the actual state is the same as the state we are trying to set, it exits the function
        if (newState == m_CurrentState)
            return;
        //First, it will do the actions to exit the current state, then will initiate the new state.
        ExitState();
        InitState(newState);
    }

    private void InitState(PlayerMachineStates currentState)
    {
        //We declare that the current state of the object is the new state we declare on the function
        m_CurrentState = currentState;

        //Then it will compare the current state to run the state actions
        switch (m_CurrentState)
        {
            case PlayerMachineStates.IDLE:
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_IdleAnimationName);
                break;

            case PlayerMachineStates.WALK:
                m_Animator.Play(m_WalkAnimationName);
                break;

            case PlayerMachineStates.JUMP:
                m_Animator.Play(m_JumpAnimationName);
                //m_Jumping.JumpByForce();
                //OnObjectiveCheck(EMission.JUMP);
                break;

            case PlayerMachineStates.ATTACK1:
                //Attack will set the velocity to zero, so it cant move while attacking
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_Attack1AnimationName);
                //_Damaging.SetComboMultiplier(1);
                break;

            case PlayerMachineStates.ATTACK2:
                //Attack will set the velocity to zero, so it cant move while attacking
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_Attack2AnimationName);
                //m_Damaging.SetComboMultiplier(1.5f);
                break;

            case PlayerMachineStates.COMBO1:
                //Attack will set the velocity to zero, so it cant move while attacking
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_Combo1AnimationName);
                //Then we call for the shooting action and we pass the spawnpoint. We do substract the mana in the UpdateState().
                //m_Shooting.Shoot();
                //m_Mana.OnChangeMana(-m_ManaCost.ManaCost);
                //m_OnEnergyUsed.Raise();
                //OnObjectiveCheck(EMission.SHOOT);
                break;

            case PlayerMachineStates.COMBO2:
                //Attack will set the velocity to zero, so it cant move while attacking
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_Combo2AnimationName);
                //m_Damaging.SetComboMultiplier(1.5f);
                break;

            case PlayerMachineStates.HIT:
                //Will play the animation and then set the state to Idle
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_HitAnimationName);
                //m_Moving.OnKnockback(m_LastAttackSide, m_LastKnockbackPower);
                //StartCoroutine(OnPlayerHitCoroutine());
                break;

            case PlayerMachineStates.CROUCH:
                //Crouch sets the movement to zero and it doesn't move.
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_CrouchAnimationName);
                break;

            case PlayerMachineStates.CROUCHATTACK1:
                //Attack will set the velocity to zero, so it cant move while attacking
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_CrouchAttack1AnimationName);
                //m_Damaging.SetComboMultiplier(1);
                break;

            case PlayerMachineStates.CROUCHATTACK2:
                //Attack will set the velocity to zero, so it cant move while attacking
                //m_Moving.OnStopMovement();
                m_Animator.Play(m_CrouchAttack2AnimationName);
                //m_Damaging.SetComboMultiplier(1);
                break;

            case PlayerMachineStates.DEAD:
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
            case PlayerMachineStates.IDLE:
                //      if (m_MovementAction.ReadValue<Vector2>().x != 0)
                ChangeState(PlayerMachineStates.WALK);
                break;

            case PlayerMachineStates.WALK:
                //      m_Moving.OnMoveByForce(m_MovementAction.ReadValue<Vector2>());
                //     if (m_Rigidbody.velocity == Vector2.zero)
                ChangeState(PlayerMachineStates.IDLE);
                break;

            case PlayerMachineStates.JUMP:
                //     if (m_Rigidbody.velocity == Vector2.zero)
                ChangeState(PlayerMachineStates.IDLE);
                //     if (m_MovementAction.ReadValue<Vector2>().x != 0)
                ChangeState(PlayerMachineStates.WALK);
                break;

            case PlayerMachineStates.CROUCH:
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

    private void Shoot1(InputAction.CallbackContext context)
    {
        m_Shooting.OnPlayerShoot(m_Camera);
    }
}
