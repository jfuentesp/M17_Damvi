using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehaviour : MonoBehaviour
{
    [Header("Layer the Raycast has to keep")]
    [SerializeField]
    LayerMask m_ShootingLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerShoot(Camera cameraReference)
    {
        RaycastHit hit;
        if(Physics.Raycast(cameraReference.transform.position, cameraReference.transform.forward, out hit, 20f, m_ShootingLayerMask))
        {
            Debug.DrawLine(cameraReference.transform.position, hit.point, Color.red, 1f);
            if (hit.collider.TryGetComponent<DamageableBehaviour>(out DamageableBehaviour target))
                Debug.Log("He tocado en algo que se puede dañar.");
        }
    }
}
