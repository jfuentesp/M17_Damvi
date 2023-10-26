using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    [Header("Spawnpoints references")]
    [SerializeField]
    private Transform m_LeftSpawnpoint;
    [SerializeField]
    private Transform m_RightSpawnpoint;

    [SerializeField]
    private float m_SpawnTime;
    private int m_Wave;
    private int m_EnemiesToSpawn;
    private int m_EnemiesSpawned;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
