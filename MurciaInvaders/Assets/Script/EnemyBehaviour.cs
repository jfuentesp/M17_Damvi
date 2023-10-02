using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    private Rigidbody2D m_RigidBody;

    /*[Header("Limit positions for Blue enemy sinoid")]
    [SerializeField]
    private Transform m_LeftLimit;
    [SerializeField]
    private Transform m_RightLimit;*/
    [Header("Parameters for Blue enemy sinoid")]
    [SerializeField]
    private float m_Frequency = 5f;
    [SerializeField]
    private float m_Offset = 1f;
    [SerializeField]
    private float m_Magnitude = 1f;

    

    private void Awake()
    {
        //Loading components
        m_RigidBody = GetComponent<Rigidbody2D>();     
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    //This function will be called by the Spawner (boss) to set randomly an Scriptable object with the enemy prefab stats
    public void SetStats(EnemyScriptableObject enemyStats)
    {
        m_EnemyType = enemyStats.EnemyType;
        m_EnemySpeed = enemyStats.EnemySpeed;
        m_ScoreValue = enemyStats.ScoreValue;
        m_EnemyColor = enemyStats.Color;
        m_MaxHitpoints = enemyStats.MaxHitpoints;
        m_Hitpoints = enemyStats.Hitpoints;
        //Setting the sprite color in here too
        SpriteRenderer enemySprite = GetComponent<SpriteRenderer>();
        enemySprite.color = m_EnemyColor;
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
        m_RigidBody.velocity = new Vector3(Mathf.Sin(Time.time * m_Frequency + m_Offset) * m_Magnitude, -m_EnemySpeed, 0);
        //float clamp = Mathf.Clamp(transform.position.x, m_LeftLimit.transform.position.x, m_RightLimit.transform.position.x);
    }

}
