using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour
{
    [SerializeField]
    private float m_BulletDamage;

    public float BulletDamage => m_BulletDamage;
    [SerializeField]
    private float m_BulletSpeed;

    private Rigidbody2D m_RigidBody;

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        m_RigidBody.velocity = Vector3.right * m_BulletSpeed;
    }
}
