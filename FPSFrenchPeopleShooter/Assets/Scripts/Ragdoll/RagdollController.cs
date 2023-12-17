using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class RagdollController : MonoBehaviour
{
    private HealthBehaviour m_Health;
    private DamageableBehaviour m_Damage;

    [SerializeField]
    private GameObject m_RigRoot;
    private Rigidbody[] m_Bones;
    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Health = GetComponent<HealthBehaviour>();
        m_Damage = GetComponentInParent<DamageableBehaviour>();
        m_Bones = m_RigRoot.GetComponentsInChildren<Rigidbody>();
        Activate(false);
    }

    public void Activate(bool state)
    {
        foreach (Rigidbody bone in m_Bones)
            bone.isKinematic = !state;
        m_Animator.enabled = !state;
    }

    public void Die()
    {
        Activate(true);
    }
}

