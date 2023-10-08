using System.Collections;
using System.Collections.Generic;
using murciainvaders;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    //Instance of the Boss. Refers to this own gameobject.
    private static BossBehaviour m_Instance;
    public static BossBehaviour BossInstance => m_Instance; //A getter for the instance of the boss. Similar to get { return m_Instance }. (Accessor)

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
    [SerializeField]
    private int m_MaxBossHP = 20;
    public int BossMaxHP
    {
        get { return m_MaxBossHP; }
    }
    private int m_CurrentBossHP;
    public int BossCurrentHP
    {
        get { return m_CurrentBossHP;}
    }


    [Header("Movement clamps")]
    private float m_Clamp = 2f;

    private Vector2 m_Direction;

    //Rigidbody of the boss Gameobject
    Rigidbody2D m_RigidBody;

    //Pool object that will implement enemy pool
    [SerializeField]
    Pool m_EnemyPool;
    //Pool object that will implement bullet pool
    //[SerializeField]
    //Pool m_BulletPool;

    //LayerMask of PlayerBullets. It's better to serialize that field if you don't want to deal with binaries.
    [SerializeField]
    LayerMask m_LayerMaskBullets;

    //Scriptable Object List that holds every enemy type stats 
    [SerializeField]
    private List<EnemyScriptableObject> m_EnemyScriptables;

    private void Awake()
    {
        //First, we initialize an instance of Boss. If there is already an instance, it destroys the element and returns.
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        //Loading components
        m_RigidBody = GetComponent<Rigidbody2D>();
        // m_EnemyPool = GetComponent<Pool>();
        m_Direction = new Vector2(1 * m_BossSpeed,0);
    }

    // Start is called before the first frame update
    void Start()
    {
        ReloadStats();
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
            Debug.Log(string.Format("Spawning enemy {0} color {1}", random, m_EnemyType.Color));
            yield return new WaitForSeconds(2f);
        }     
    }

    public void OnBossDamage(int damageReceived)
    {
        m_CurrentBossHP -= damageReceived;
        Debug.Log(string.Format("Boss receives {0} damage. Current HP: {1}", damageReceived, m_CurrentBossHP));
        if(m_CurrentBossHP <= 0)
        {
            //Dies and change scene
        }
    }

    public void ReloadStats()
    {
        m_CurrentBossHP = m_MaxBossHP;
        Debug.Log(string.Format("Boss now has {0} HP", m_CurrentBossHP));
    }

}
