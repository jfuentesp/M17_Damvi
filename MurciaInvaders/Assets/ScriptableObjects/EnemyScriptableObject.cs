using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTypeStats", menuName = "Scriptable Objects/Enemy Type Stats")]
public class EnemyScriptableObject : ScriptableObject
{
    [SerializeField]
    private int m_EnemyType;
    [SerializeField]
    private float m_EnemySpeed;
    [SerializeField]
    private int m_ScoreValue;
    [SerializeField]
    private Color m_EnemyColor;
    [SerializeField]
    private int m_MaxHitpoints;
    [SerializeField]
    private int m_Hitpoints;

    //Accessors to get the value (similar to getters)
    public int EnemyType => m_EnemyType;
    public float EnemySpeed => m_EnemySpeed;
    public int ScoreValue => m_ScoreValue;
    public Color Color => m_EnemyColor;
    public int MaxHitpoints => m_MaxHitpoints;
    public int Hitpoints => m_Hitpoints;

    
}
