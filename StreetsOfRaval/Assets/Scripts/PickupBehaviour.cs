using System.Collections;
using System.Collections.Generic;
using streetsofraval;
using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{
    [Header("Scriptable List References")]
    [SerializeField]
    List<PickupScriptableObject> m_ScriptablePickups;

    private int m_PickupID;
    private string m_PickupName;
    private int m_PickupValue;
    private float m_PickupEffectDuration;
    private Color m_PickupColor;
    private Sprite m_PickupSprite;

    [Header("Time until the pickup disappears")]
    [SerializeField]
    private float m_PickupDuration;

    private Vector2 m_SpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        PickupScriptableObject randomPickup = m_ScriptablePickups[Random.Range(0, m_ScriptablePickups.Count)];
        InitPickup(randomPickup);
        StartCoroutine(AliveCoroutine());
    }

    public void InitPickup(PickupScriptableObject pickupInfo)
    {
        m_PickupID = pickupInfo.PickupID;
        m_PickupName = pickupInfo.PickupName;
        m_PickupValue = pickupInfo.PickupValue;
        m_PickupEffectDuration = pickupInfo.PickupEffectDuration;
        m_PickupColor = pickupInfo.PickupColor;
        m_PickupSprite = pickupInfo.PickupSprite;
    }

    private IEnumerator AliveCoroutine()
    {
        yield return new WaitForSeconds(m_PickupDuration);
    }

    public void ObjectEffect()
    {

    }
}
