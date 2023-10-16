using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

        //Variables for the current state and an Enum for setting the Player States
        private enum PlayerMachineStates { NONE, IDLE, WALK, HIT1, HIT2, JUMP, JUMPINGHIT1, JUMPINGHIT2 }
        private PlayerMachineStates m_CurrentState;

        private void Awake()
        {
            //We set the player gameobject rigid body
            m_RigidBody = GetComponent<Rigidbody2D>();
            //We set the player gameobject animator
            m_Animator = GetComponent<Animator>();

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //Each frame, player behaviour will be listening 
            UpdateState();
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

        private void InitState(PlayerMachineStates currentState)
        {
            //We declare that the current state of the object is the new state we declare on the function
            m_CurrentState = currentState;
            //Then it will compare the current state to run the state actions
            switch (m_CurrentState)
            {
                case PlayerMachineStates.IDLE:
                    m_RigidBody.velocity = Vector3.zero;

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

        private void UpdateState()
        {
            switch (m_CurrentState)
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

        /* !!! FINISHING THE BUILD OF THE STATE MACHINE !!! */

    }
}

