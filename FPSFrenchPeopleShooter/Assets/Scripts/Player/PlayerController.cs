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

    private void Shoot1(InputAction.CallbackContext context)
    {
        m_Shooting.OnPlayerShoot(m_Camera);
    }
}
