using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace streetsofraval
{
    public class PlayerBehaviour : MonoBehaviour
    {

        //Reference to the instance of the player

        //Reference to the InputSystem
        [Header("Reference to the Input System")]
        [SerializeField]
        private InputActionAsset m_InputAsset;
        private InputActionAsset m_Input;
        public InputActionAsset Input => m_Input;
        private InputAction m_MovementAction;
        public InputAction MovementAction => m_MovementAction;

        //Player rigidbody
        private Rigidbody2D m_RigidBody;
        //Player animator
        private Animator m_Animator;
        //Animation names
        private const string m_IdleAnimationName = "idle";
        private const string m_WalkAnimationName = "walk";
        private const string m_JumpAnimationName = "jump";
        private const string m_HitAnimationName = "hit1";
        private const string m_ComboAnimationName = "hit2";
        private const string m_CrouchAnimationName = "crouch";


        //Variables for the current state and an Enum for setting the Player States
        private enum PlayerMachineStates { NONE, IDLE, WALK, HIT1, HIT2, JUMP, JUMPINGHIT1, JUMPINGHIT2 }
        private PlayerMachineStates m_CurrentState;

        [Header("Player parameters")]
        [SerializeField]
        private float m_Speed;
        [SerializeField]
        private float m_Damage;
        public float Damage => m_Damage;
        [SerializeField]
        private float m_JumpForce;
        [SerializeField]
        private bool m_IsFlipped;
        [SerializeField]
        private bool m_ComboAvailable;

        private void Awake()
        {
            //We set the player gameobject rigid body
            m_RigidBody = GetComponent<Rigidbody2D>();
            //We set the player gameobject animator
            m_Animator = GetComponent<Animator>();
            //We set the boolean that will control if the character is flipped as false
            m_IsFlipped = false;

            //Setting the input variables. Don't forget to enable.
            Assert.IsNotNull(m_InputAsset);
            m_Input = Instantiate(m_InputAsset);
            m_MovementAction = m_Input.FindActionMap("PlayerActions").FindAction("Movement");
            m_Input.FindActionMap("PlayerActions").FindAction("Attack1").performed += Attack;
            m_Input.FindActionMap("PlayerActions").Enable();
        }
        // Start is called before the first frame update
        void Start()
        {
            //In this case, we can use InitState directly instead of ChangeState as it doesn't have to Exit any state previously. 
            InitState(PlayerMachineStates.IDLE);
        }

        // Update is called once per frame
        void Update()
        {
            //Each frame, player behaviour will be listening 
            UpdateState();
        }

        //Combo implementates with a boolean and few functions
        //This public functions can be triggered from the clip events to trigger the begin and end of the combo frame and the end of the hit animation
        public void InitComboWindow() 
        {
            m_ComboAvailable = true;
        }

        public void EndComboWindow() 
        {
            m_ComboAvailable = false;
        }

        public void EndHit()
        {
            ChangeState(PlayerMachineStates.IDLE);
        }



        private void Attack(InputAction.CallbackContext context)
        {
            switch (m_CurrentState)
            {
                case PlayerMachineStates.IDLE:
                    ChangeState(PlayerMachineStates.HIT1);

                    break;

                case PlayerMachineStates.WALK:
                    ChangeState(PlayerMachineStates.HIT1);

                    break;

                case PlayerMachineStates.HIT1:

                    if (m_ComboAvailable)
                        ChangeState(PlayerMachineStates.HIT2);
                    else
                        ChangeState(PlayerMachineStates.HIT1);

                    break;

                case PlayerMachineStates.HIT2:

                    if (m_ComboAvailable)
                        ChangeState(PlayerMachineStates.HIT1);
                    else
                        ChangeState(PlayerMachineStates.HIT2);
                    break;

                default:
                    break;
            }

        }

        private void Jump(InputAction.CallbackContext context)
        {
            
        }

        

        /* !!! BUILDING UP STATE MACHINE !!! Always change state with the function ChangeState */
        private void ChangeState(PlayerMachineStates newState)
        {
            //if the actual state is the same as the state we are trying to set, it exits the function
            if (newState == m_CurrentState)
                return;
            //First, it will do the actions to exit the current state, then will initiate the new state.
            ExitState();
            InitState(newState);
        }

        /* InitState will run every instruction that has to be started ONLY when enters a state */
        private void InitState(PlayerMachineStates currentState)
        {
            //We declare that the current state of the object is the new state we declare on the function
            m_CurrentState = currentState;
            if (m_IsFlipped)
            {
                m_RigidBody.transform.eulerAngles = Vector3.up * 180;
            } else
            {
                m_RigidBody.transform.eulerAngles = Vector3.zero;
            }              
            
            //Then it will compare the current state to run the state actions
            switch (m_CurrentState)
            {
                case PlayerMachineStates.IDLE:

                    m_RigidBody.velocity = Vector3.zero;
                    
                    m_Animator.Play(m_IdleAnimationName);

                    break;

                case PlayerMachineStates.WALK:

                    m_Animator.Play(m_WalkAnimationName);

                    break;

                case PlayerMachineStates.JUMP:

                    m_Animator.Play(m_JumpAnimationName);
                    m_RigidBody.AddForce(Vector2.up * m_JumpForce);

                    break;

                case PlayerMachineStates.HIT1:
                    //Attack will set the velocity to zero, so it cant move while attacking
                    m_RigidBody.velocity = Vector3.zero;
                    m_Animator.Play(m_HitAnimationName);

                    break;

                case PlayerMachineStates.HIT2:
                    //Attack will set the velocity to zero, so it cant move while attacking
                    m_RigidBody.velocity = Vector3.zero;
                    m_Animator.Play(m_ComboAnimationName);

                    break;

                default:
                    break;
            }
        }

        /* ExitState will run every instruction that has to be started ONLY when exits a state */
        private void ExitState()
        {
            switch(m_CurrentState)
            {
                case PlayerMachineStates.IDLE:

                    break;

                case PlayerMachineStates.WALK:

                    break;

                case PlayerMachineStates.JUMP:

                    break;

                case PlayerMachineStates.HIT1:

                    break;

                case PlayerMachineStates.HIT2:

                    break;

                default:
                    break;
            }
        }

        /* UpdateState will control every frame since it will be called from Update() and will control when it changes the state */
        private void UpdateState()
        {
            switch (m_CurrentState)
            {
                case PlayerMachineStates.IDLE:

                    if (m_MovementAction.ReadValue<Vector2>().x != 0) { 
                        if(m_MovementAction.ReadValue<Vector2>().x < 0)
                          m_IsFlipped = true;
                        if (m_MovementAction.ReadValue<Vector2>().x > 0)
                          m_IsFlipped = false;
                        ChangeState(PlayerMachineStates.WALK);
                    }

                    break;

                case PlayerMachineStates.WALK:

                    m_RigidBody.velocity = Vector2.right * m_MovementAction.ReadValue<Vector2>().x * m_Speed; 

                    if (m_RigidBody.velocity == Vector2.zero)
                        ChangeState(PlayerMachineStates.IDLE);

                    break;

                case PlayerMachineStates.JUMP:

                    break;

                case PlayerMachineStates.HIT1:

                    break;

                case PlayerMachineStates.HIT2:

                    break;

                default:
                    break;
            }
        }

        /* !!! FINISHING THE BUILD OF THE STATE MACHINE !!! */

    }
}

