using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    private int m_HitboxDamage = 0;
    public int HitboxDamage
    {
       get { return m_HitboxDamage; }
    }

    public void SetDamage(int damage)
    {
        m_HitboxDamage = damage;
    }
}
