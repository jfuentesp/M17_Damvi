using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float m_BulletSpeed = 5f;

    [SerializeField]
    Rigidbody2D m_RigidBody;

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
}
