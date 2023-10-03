using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet parameters")]
    // Start is called before the first frame update
    [SerializeField]
    float m_BulletSpeed = 5f;

    [SerializeField]
    Rigidbody2D m_RigidBody;

    [Header("Layer masks")]
    [SerializeField]
    private LayerMask m_EnemyBulletMask;
    [SerializeField]
    private LayerMask m_BossMask;
    [SerializeField]
    private LayerMask m_PickupsMask;

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //Bullet will spawn according to its rotation
        m_RigidBody.velocity = (transform.rotation * Vector2.up) * m_BulletSpeed;
        m_RigidBody.AddForce(Vector2.up * m_BulletSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
