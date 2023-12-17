using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableBehaviour : MonoBehaviour
{

    private HealthBehaviour m_Health;
    [Header("Damage that the object has to receive upon touch")]
    [SerializeField]
    private float m_Damage;
    public float Damage => m_Damage;

    private void Awake()
    {
        m_Health = GetComponentInParent<HealthBehaviour>();
        //OnDamage += ReceiveDamage;
    }

    public void ReceiveDamage(float damage)
    {
        Debug.Log(string.Format("{0} deals {1} damage to the target.", gameObject.name, damage));
        m_Health.OnHealthModify(-damage);
    }
}
