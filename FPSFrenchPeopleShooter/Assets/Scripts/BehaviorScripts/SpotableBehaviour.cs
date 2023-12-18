using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotableBehaviour : MonoBehaviour
{
    private bool m_PlayerIsInRange;
    public bool PlayerIsInRange => m_PlayerIsInRange;

    private GameObject m_Target;
    public GameObject Target => m_Target;

    private void Start()
    {
        m_PlayerIsInRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_PlayerIsInRange = true;
            m_Target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_PlayerIsInRange = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
