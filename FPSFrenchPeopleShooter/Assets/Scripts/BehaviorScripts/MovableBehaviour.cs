using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovableBehaviour : MonoBehaviour
{
    //Reference to the rigidbody
    private Rigidbody m_Rigidbody;

    [Header("Reference to the Camera")]
    [SerializeField]
    private Camera m_Camera;

    [Header("Speed and rotation speed atributes")]
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float m_RotationSpeed;

    [Header("Limitations if it has a Y axis clamp")]
    [SerializeField]
    private float m_ClampY;
    [SerializeField]
    private bool m_InversePitch;

    Vector3 m_Movement;
    float m_RotationMovementX; //Pitch - turns based on X axis (View from top to down)
    Vector3 m_RotationMovementY; //Yaw - turns based on Y axis (View from left to right)

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void OnMove(Vector3 direction)
    {
        m_Movement = direction;
    }

    public void OnRotateYaw(float direction)
    {
        m_RotationMovementY = Vector3.up * direction;
    }

    public void OnRotatePitch(float direction)
    {
        m_RotationMovementX += direction /* m_RotationSpeed*/;
    }

    private void Update()
    {
        //Rotation with clamped movement on X axis (pitch)
        m_RotationMovementX = Mathf.Clamp(m_RotationMovementX, -m_ClampY, m_ClampY);
        m_Camera.transform.localEulerAngles = (m_InversePitch ? Vector3.right : -Vector3.right) * m_RotationMovementX;
        
        //Simple rotation with no limits
        transform.Rotate(m_RotationMovementY * m_RotationSpeed * Time.deltaTime);
        m_RotationMovementY = Vector3.zero;
    }

    private void FixedUpdate()
    {
        m_Rigidbody.velocity = m_Movement.normalized * m_Speed + Vector3.up * m_Rigidbody.velocity.y;
        m_Movement = Vector3.zero;
    }
}
