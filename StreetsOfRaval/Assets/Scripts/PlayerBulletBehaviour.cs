using streetsofraval;
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
    [SerializeField]
    private float m_BulletLifetime;

    private Rigidbody2D m_RigidBody;

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    public void InitBullet(/*float speed, float damage,*/ Vector2 direction)
    {
        /*m_BulletSpeed = speed;
        m_BulletDamage = damage;*/
        //It will initiate the bullet direction and give it a speed
        m_RigidBody.velocity = direction * m_BulletSpeed;
        //If the direction is negative it will invert the sprite. If not, it will let it as default.
        if(direction.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0); 
        } else
        {
            transform.eulerAngles = Vector3.zero;
        }
        StartCoroutine(BulletAlive()); //Starting the lifetime coroutine
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         
    }

    private IEnumerator BulletAlive()
    {
        yield return new WaitForSeconds(m_BulletLifetime);
        this.gameObject.SetActive(false);
    }
}
