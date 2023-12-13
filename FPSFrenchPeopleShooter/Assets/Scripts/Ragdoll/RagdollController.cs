using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class RagdollController : MonoBehaviour
{
    private HealthBehaviour m_Health;
    private float m_HP = 100f;

    private Rigidbody[] m_Bones;
    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Health = GetComponent<HealthBehaviour>();
        m_Bones = GetComponentsInChildren<Rigidbody>();
        Activate(false);

        foreach (Rigidbody bone in m_Bones)
            if (bone.TryGetComponent<DamageableBehaviour>(out DamageableBehaviour damageable))
                damageable.OnDamage += ReceiveDamage;
    }

    public void Activate(bool state)
    {
        foreach (Rigidbody bone in m_Bones)
            bone.isKinematic = !state;
        m_Animator.enabled = !state;
    }

    private void ReceiveDamage(float damage)
    {
        m_HP -= damage;
        if (m_HP <= 0)
            Die();
    }

    private void Die()
    {
        foreach (Rigidbody bone in m_Bones)
            if (bone.TryGetComponent<DamageableBehaviour>(out DamageableBehaviour damageable))
                damageable.OnDamage -= ReceiveDamage;

        Activate(true);
    }
}

