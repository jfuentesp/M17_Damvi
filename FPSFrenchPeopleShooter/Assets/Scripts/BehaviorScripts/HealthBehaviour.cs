using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour
{
    [Header("Health parameters")]
    [SerializeField]
    private float m_MaxHealth;
    [SerializeField]
    private float m_CurrentHealth;
    [SerializeField]
    private bool m_IsAlive;
    public float MaxHealth => m_MaxHealth;
    public float CurrentHealth => m_CurrentHealth;
    public bool IsAlive => m_IsAlive;

    private RagdollController m_Ragdoll;

    private void Awake()
    {
        m_MaxHealth = m_CurrentHealth;
        m_IsAlive = true;
        m_Ragdoll = GetComponent<RagdollController>();
    }

    public void OnHealthModify(float value)
    {
        m_CurrentHealth += value;
        Debug.Log(string.Format("Current {0} health: {1}/{2} ", gameObject.name, m_CurrentHealth, m_MaxHealth));
        if(m_CurrentHealth <= 0)
        {
            m_IsAlive = false;
            m_Ragdoll.Die();
        }
    }

    public void OnSetCurrentHealth(float value)
    {
        m_CurrentHealth = value;
    }

    public void OnSetMaxHealth(float value)
    {
        m_MaxHealth = value;
    }
}
