using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehaviour : MonoBehaviour
{
    [Header("Layer the Raycast has to keep")]
    [SerializeField]
    LayerMask m_ShootingLayerMask;

    private bool m_PlayerIsInRange;
    public bool PlayerIsInRange => m_PlayerIsInRange;

    private GameObject m_Target;
    public GameObject Target => m_Target;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerIsInRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_PlayerIsInRange = true;
            m_Target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_PlayerIsInRange = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerShoot(Camera cameraReference)
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraReference.transform.position, cameraReference.transform.forward, out hit, 20f, m_ShootingLayerMask))
        {
            Debug.DrawLine(cameraReference.transform.position, hit.point, Color.red, 1f);
            if (hit.collider.TryGetComponent<DamageableBehaviour>(out DamageableBehaviour target))
            {
                Debug.Log("He tocado en algo que se puede dañar.");
                target.ReceiveDamage(target.Damage);
            }
        }
    }

    public void OnEnemyShoot(float variation)
    {
        RaycastHit hit;
        transform.LookAt(m_Target.transform.position);
        if (Physics.Raycast(transform.position, transform.forward + transform.right * variation + transform.up * variation, out hit, 20f, m_ShootingLayerMask))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green, 1f);
            if (hit.collider.TryGetComponent<DamageableBehaviour>(out DamageableBehaviour target))
            {
                Debug.Log("El enemigo dispara a algo que toca.");
                target.ReceiveDamage(target.Damage);
            }
        }
    }
}
