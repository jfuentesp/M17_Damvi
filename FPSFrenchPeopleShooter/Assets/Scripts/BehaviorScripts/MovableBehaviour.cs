using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovableBehaviour : MonoBehaviour
{
    /*
     * When to use Rigidbody to move by physics or a Character Controller?
     * If has to interact with other physics objects, have any collider shapes, play with velocities -> Rigidbody + physics
     * If has to handle with responsive and smooth movement, control to get stuck, handle in-air movement, etc -> Controller
     * 
     */

    //Reference to the rigidbody
    private Rigidbody m_Rigidbody;

    [Header("Reference to the Player Camera")]
    [SerializeField]
    private Camera m_Camera;

    [Header("Speed and rotation speed atributes")]
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float m_RotationSpeed;
    [SerializeField, Range(0f, 1f)]
    private float m_Sensivity;

    [Header("Limitations if it has a Y axis clamp")]
    [SerializeField]
    private float m_ClampY;
    [SerializeField]
    private bool m_InversePitch;
    [SerializeField]
    private bool m_IsPlayer;

    Vector3 m_Movement;
    float m_RotationMovementX; //Pitch - turns based on X axis (View from top to down)
    Vector3 m_RotationMovementY; //Yaw - turns based on Y axis (View from left to right)

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void OnMove(Vector3 direction)
    {
        Debug.Log(direction);
        m_Movement = direction;
        Debug.Log(m_Movement);
    }

    public void OnRotateYaw(float direction)
    {
        m_RotationMovementY = Vector3.up * direction;
    }

    public void OnRotatePitch(float direction)
    {
        m_RotationMovementX += direction * m_Sensivity;
    }

    public void OnStopMovement()
    {
        m_Rigidbody.velocity = Vector3.zero;
    }

    //This function is made to use it on States machine
    public void OnBasicMovement()
    {
        /*Simple movement, similar to transform.position += m_Movement * m_Speed * Time.deltaTime; but with a big difference:
        * Translate -> Moves in a direction based on local coords. That means the object is affected by its local rotation. Like giving values to transform.forward, right, etc.
        * transform.position += -> Moves based on a global position in the world, even if its rotated. As Vector.up, right, works. */
        transform.Translate(m_Movement * m_Speed * Time.deltaTime);
    }

    private void Update()
    {
        //Rotation with clamped movement on X axis (pitch)
        m_RotationMovementX = Mathf.Clamp(m_RotationMovementX, -m_ClampY, m_ClampY);
        if(m_IsPlayer)
            m_Camera.transform.localRotation = Quaternion.Euler((m_InversePitch ? 1 : -1) * m_RotationMovementX, 0, 0); //Same than -> m_Camera.transform.localEulerAngles = (m_InversePitch ? Vector3.right : -Vector3.right) * m_RotationMovementX;

        //Simple rotation with no limits
        transform.Rotate(m_RotationMovementY * m_RotationSpeed * m_Sensivity * Time.deltaTime);
        m_RotationMovementY = Vector3.zero;

        /*Simple movement, similar to transform.position += m_Movement * m_Speed * Time.deltaTime; but with a big difference:
        * Translate -> Moves in a direction based on local coords. That means the object is affected by its local rotation. Like giving values to transform.forward, right, etc.
        * transform.position += -> Moves based on a global position in the world, even if its rotated. As Vector.up, right, works. */
        transform.Translate(m_Movement * m_Speed * Time.deltaTime);
        m_Movement = Vector3.zero;
    }

    private void FixedUpdate()
    {   
        //If we move by physics
        /*m_Rigidbody.velocity = m_Movement.normalized * m_Speed + Vector3.up * m_Rigidbody.velocity.y;
        Debug.Log(m_Rigidbody.velocity);
        m_Movement = Vector3.zero;*/
    }
}
