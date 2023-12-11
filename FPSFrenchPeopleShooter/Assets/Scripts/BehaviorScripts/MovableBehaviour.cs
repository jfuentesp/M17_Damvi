using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovableBehaviour : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    [Header("Speed and rotation speed atributes")]
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float m_RotationSpeed;

    [Header("Limitations if it has a Y axis clamp")]
    [SerializeField]
    private float m_ClampY;

    Vector3 m_Movement;
    Vector3 m_RotationMovement;
    Vector3 m_RotationMovementY;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void OnMove(Vector3 direction)
    {
        m_Movement = direction;
    }

    public void OnRotate(float direction)
    {
        m_RotationMovementY = Vector3.up * direction;
    }

    private void Update()
    {
        //m_RotationMovementY = Vector3.up * Mathf.Clamp(m_RotationMovement.y, -m_ClampY, m_ClampY);
        transform.Rotate(m_RotationMovementY * m_RotationSpeed * Time.deltaTime);
        m_RotationMovementY = Vector3.zero;
    }

    private void FixedUpdate()
    {
        m_Rigidbody.velocity = m_Movement.normalized * m_Speed + Vector3.up * m_Rigidbody.velocity.y;
        m_Movement = Vector3.zero;
    }
}
