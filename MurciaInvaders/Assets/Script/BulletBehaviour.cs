using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [Header("Bullet parameters")]
    // Start is called before the first frame update
    [SerializeField]
    float m_BulletSpeed = 5f;

    Rigidbody2D m_RigidBody;

    [Header("Layer masks")]
    [SerializeField]
    private LayerMask m_EnemyBulletMask;
    [SerializeField]
    private LayerMask m_BossMask;
    [SerializeField]
    private LayerMask m_PickupsMask;

    public delegate void BulletCollision();
    public event BulletCollision OnBulletCollision;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Eliminator"))
        {
            Debug.Log("Bullet colides vs Eliminator");
            gameObject.SetActive(false);
        }
    }

    //We will save a coroutine in a variable
    private Coroutine m_AliveCoroutine;

    private void OnEnable()
    {
        //Bullet will spawn according to its rotation on enable
        m_RigidBody.velocity = (transform.rotation * Vector2.up) * m_BulletSpeed;
        m_RigidBody.AddForce(Vector2.up * m_BulletSpeed);
        //Start coroutine
        m_AliveCoroutine = StartCoroutine(BulletIsAlive());
    }

    private void OnDisable()
    {
        //On disable, velocity will be set to zero again
        m_RigidBody.velocity = Vector3.zero;
        //We will stop the coroutine to optimize
        StopCoroutine(m_AliveCoroutine);
    }

    private IEnumerator BulletIsAlive()
    {
        //Coroutine will wait for 5 seconds, then will disable the object. We don't need a while as it will just be alive once for each game object activation.
        yield return new WaitForSeconds(5);
        this.gameObject.SetActive(false);
    }
}
