using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("GameObject References")]
    //Reference to Enemy: Enemies will spawn from the boss each cycle (based on time given)
    [SerializeField]
    private GameObject m_Enemy;

    [Header("Boss parameters")]
    [SerializeField]
    //Time which the boss will spawn an enemy every cycle
    private float m_SpawnTime;
    [SerializeField]
    //Movement speed of the boss to move in X Axis
    private float m_BossSpeed = 1.4f;

    //Boss HP
    private int m_MaxBossHP;
    private int m_CurrentBossHP;

    [Header("Movement clamps")]
    private float m_Clamp = 2f;

    private Vector2 m_Direction;

    //Rigidbody of the boss Gameobject
    Rigidbody2D m_RigidBody;

    //Pool object that will implement enemy pool
    Pool m_EnemyPool;

    //LayerMask of PlayerBullets. It's better to serialize that field if you don't want to deal with binaries.
    [SerializeField]
    LayerMask m_LayerMaskBullets;

    //Scriptable Object List that holds every enemy type stats 
    [SerializeField]
    private List<EnemyScriptableObject> m_EnemyScriptables;

    private void Awake()
    {
        //Loading components
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_EnemyPool = GetComponent<Pool>();
        m_Direction = new Vector2(1 * m_BossSpeed,0);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Starting the spawning coroutine
        StartCoroutine(SpawnCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //We catch the current position of the boss
        float currentPosition = Mathf.Clamp(transform.position.x, -m_Clamp, m_Clamp);
        BossMovement(currentPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            //Delegate example. The boss ship will notify the gamemanager to substract 1 HP to the boss HP.
            

        }
    }

    private void BossMovement(float clampedPosition)
    {
        //Movement function of the boss, side to side, checking the current position in order to call for a direction
        if (clampedPosition >= m_Clamp)
            m_Direction.x = -1 * m_BossSpeed;
        if (clampedPosition <= -m_Clamp)
            m_Direction.x = 1 * m_BossSpeed;
        m_RigidBody.velocity = m_Direction;
    }

    private IEnumerator SpawnCoroutine()
    {
        while(true)
        {
            //Spawns an Enemy, randomly, from a total of 3 possible enemies. Then, gives its parameters such as Color, movement and type of enemy.
            int random = Random.Range(0, m_EnemyScriptables.Count);
            EnemyScriptableObject m_EnemyType = m_EnemyScriptables[random];
            GameObject m_CurrentEnemy = m_EnemyPool.GetElement(this.gameObject);
            m_CurrentEnemy.GetComponent<EnemyBehaviour>().SetStats(m_EnemyType);
            m_CurrentEnemy.transform.position = transform.position;
            m_CurrentEnemy.transform.Rotate(-Vector3.forward * 180);
            yield return new WaitForSeconds(2f);
        }     
    }

}
