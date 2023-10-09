using System.Collections;
using System.Collections.Generic;
using murciainvaders;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [Header("Bullet parameters")]
    // Start is called before the first frame update
    [SerializeField]
    float m_BulletSpeed = 5f;
    int m_BulletDamage = 1;

    Rigidbody2D m_RigidBody;

    [Header("Pools references")]
    //Parent pool to enable or disable this object properly
    [SerializeField]
    Pool m_ParentPool;

    //Variable where the color of this bullet will be saved
    private Color m_BulletColor;
    public Color BulletColor
    {
        get { return m_BulletColor; }
    }

    private SpriteRenderer m_SpriteRenderer;

    [Header("Delegates and GameEvents")]
    [SerializeField]
    private GameEventInt m_OnBossDamageEvent;


    private void Awake()
    {   
        //Loading components
        m_ParentPool = GetComponentInParent<Pool>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        //Bullet will spawn according to its rotation
        m_RigidBody.velocity = (transform.rotation * Vector2.up) * m_BulletSpeed;
        m_RigidBody.AddForce(Vector2.up * m_BulletSpeed);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyBehaviour enemy = collision.gameObject.GetComponent<EnemyBehaviour>();
            enemy.OnEnemyDamage(m_BulletDamage, m_BulletColor);
            m_ParentPool.ReturnElement(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            m_OnBossDamageEvent.Raise(m_BulletDamage);
            m_ParentPool.ReturnElement(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Eliminator"))
        {
            Debug.Log("Bullet colides vs Eliminator");
            m_ParentPool.ReturnElement(this.gameObject);
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
        //We save the color on the variable
        m_BulletColor = PlayerBehaviour.PlayerInstance.BulletColor; 
        SetColor(m_BulletColor);
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
        m_ParentPool.ReturnElement(this.gameObject);
    }

    public void SetColor(Color color)
    {
        m_SpriteRenderer.color = color; 
    }

    
}
