using System.Collections;
using System.Collections.Generic;
using System.IO;
using murciainvaders;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy parameters")]
    //Score value that the enemy will add upon defeated
    [SerializeField]
    private int m_EnemyType;
    [SerializeField]
    private int m_ScoreValue;
    //Speed value of the enemy
    [SerializeField]
    private float m_EnemySpeed;
    //Color of the enemy. It's the color that will be set on the Sprite component.
    [SerializeField]
    private Color m_EnemyColor;
    //Max hitpoints of the enemy (each shot of their same kind of color will substract by 1 its HP
    [SerializeField]
    private int m_MaxHitpoints;
    //Current HP. MaxHitpoints value on spawn.
    [SerializeField]
    private int m_Hitpoints;
    //Damage that will deal to the player if lands
    [SerializeField]
    private int m_Damage;

    [Header("Pools references")]
    //Parent pool to enable or disable this object properly
    [SerializeField]
    Pool m_ParentPool;

    //Reference to an instance of the player.
    PlayerBehaviour m_Player;

    private Rigidbody2D m_RigidBody;

    [Header("Parameters for Blue enemy with sinoidal movement")]
    [SerializeField]
    private float m_Frequency = 5f;
    [SerializeField]
    private float m_Offset = 1f;
    [SerializeField]
    private float m_Magnitude = 1f;
    private Vector3 m_Direction;
    private Vector3 m_Axis;

    //GameEvents and delegates
    [SerializeField]
    private GameEventInt m_OnPlayerDamageEvent;
    [SerializeField]
    private GameEventInt m_OnEnemyDestroyedEvent;
    


    private void Awake()
    {
        //Loading components
        m_Player = PlayerBehaviour.PlayerInstance;
        m_ParentPool = GetComponentInParent<Pool>();
        m_RigidBody = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        //Values used for the sinoid behavior of the blue enemy
        m_Direction = -Vector3.up;
        m_Axis = transform.right;
    }

    private void FixedUpdate()
    {
        //As there will be three different enemy types behavior, we will check which enemy is to set a movement action
        if(m_EnemyType == 0)
        {
            RedMovement();
        }
        if (m_EnemyType == 1)
        {
            GreenMovement();
        }
        if(m_EnemyType == 2)
        {
            BlueMovement();
        }
    }

    private void OnEnable()
    {
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {     
        if(collision.gameObject.CompareTag("Player"))
        {
            //If hits the player, it disables and notifies the observer
            m_OnPlayerDamageEvent.Raise(m_Damage);
            m_ParentPool.ReturnElement(this.gameObject);          
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Eliminator"))
        {
            //If hits the eliminator trigger, it disables and notifies the observer
            m_OnPlayerDamageEvent.Raise(m_Damage);
            m_ParentPool.ReturnElement(this.gameObject);         
        }   
    }

    //This function will substract player bullet damage to the enemy
    public void OnEnemyDamage(int damageReceived, Color bulletColor)
    {
        if (bulletColor.Equals(m_EnemyColor))
        {
            m_Hitpoints -= damageReceived;
            Debug.Log(string.Format("Bullet hits an enemy and deals {0} damage. Current hp: {1}", damageReceived, m_Hitpoints));
            if (m_Hitpoints == 0)
            {
                Debug.Log("Enemy destroyed. Providing score:" + m_ScoreValue);
                m_OnEnemyDestroyedEvent.Raise(m_ScoreValue);
                m_ParentPool.ReturnElement(this.gameObject); //Enemy returned to the pool
            }
        } 
    }

    //This function will be called by the Spawner (boss) to set randomly an Scriptable object with the enemy prefab stats
    public void SetStats(EnemyScriptableObject enemyStats)
    {
        m_EnemyType = enemyStats.EnemyType;
        m_EnemySpeed = enemyStats.EnemySpeed;
        m_ScoreValue = enemyStats.ScoreValue;
        m_EnemyColor = enemyStats.Color;
        m_MaxHitpoints = enemyStats.MaxHitpoints;
        m_Hitpoints = enemyStats.Hitpoints;
        m_Damage = enemyStats.Damage;
        //Setting the sprite color in here too
        SpriteRenderer enemySprite = GetComponent<SpriteRenderer>();
        enemySprite.color = m_EnemyColor;
        //If the enemy is blue, we have to unlock the X axis constraint in order to be able to make the sinoidal move
        if (m_EnemyType == 2)
            //To unlock, we have to make an AND and pass a negate value of FreezePositionX (&= and the ~ char)
            m_RigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
    }

    private void RedMovement()
    {
        m_RigidBody.velocity = -Vector3.up * m_EnemySpeed;
    }

    private void GreenMovement()
    {
        m_RigidBody.velocity = -Vector3.up * m_EnemySpeed;       
    }

    private void BlueMovement()
    {
        m_RigidBody.velocity = m_Direction + m_Axis * Mathf.Sin(m_Frequency * Time.time + m_Offset) * m_Magnitude;
    }

}
