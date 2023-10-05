using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace murciainvaders
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [Header("GameObject References")]
        //Reference to PlayerBullets: Bullets will spawn from the cannon everytime you shoot
        [SerializeField]
        private GameObject m_PlayerBullet;
        //Reference to Player Cannon: Cannon will rotate from the parent object
        [SerializeField]
        private GameObject m_Cannon;
        //Reference to InputActions: Input controls to move the cannon
        [SerializeField]
        private InputActionAsset m_InputActions;
        //Reference to Shooting Button GUI to be able to change its sprite color
        [SerializeField]
        private GameObject m_ShootingButton;
        private Image m_ShootingButtonSprite;
        
        //Variable for saving the player rigidbody. Will be set in Awake() function.
        private Rigidbody2D m_RigidBody;
        //Instance of InputActions!
        private InputActionAsset m_Input;
        private InputAction m_MovementInput;

        [Header("Speed parameter (in angles per second)")]
        /*[SerializeField]
        private float m_PlayerAngularSpeed = 50f;*/
        [SerializeField]
        private float m_PlayerRotationSpeed = 2f;

        [Header("Angular limits on player rotation")]
        [SerializeField]
        private float m_minClamp = 60;
        [SerializeField]
        private float m_maxClamp = -60;

        //Player HP
        private int m_MaxPlayerHP;
        private int m_CurrentPlayerHP;


        //For avoiding using some binary stuff, we set the layer through the Unity inspector with this (if needed)
        /*[SerializeField]
        private LayerMask m_ActionLayerMask;*/

        //Pool component which will have the bullets to instantiate
        Pool m_BulletPool;
        
        //Actual bullet color
        private Color m_BulletColor;
        private int m_colorCount = 0;

        [Header("Array of bullet colors")]
        [SerializeField]
        private List<Color> m_Colors = new List<Color>();

        [Header("Delegates")]
        [SerializeField]
        private EnemyBehaviour m_Enemy; //Observable


        private void Awake()
        {
            m_Enemy.OnDamageDealt += ReceiveDamage;
            //Loading components
            m_RigidBody = GetComponent<Rigidbody2D>();
            m_BulletPool = this.GetComponentInChildren<Pool>();
            //Loading InputSystem
            //We need to instantiate InputActions first
            m_Input = Instantiate(m_InputActions);
            //Control over movement inputs
            m_MovementInput = m_Input.FindActionMap("PlayerActionMap").FindAction("Movement");
            m_MovementInput.performed += MovementPerformed;
            //Control over shooting input
            m_Input.FindActionMap("PlayerActionMap").FindAction("Shooting").performed += ShootingAction;
            //Control over switching color of the loaded bullets input
            m_Input.FindActionMap("PlayerActionMap").FindAction("ChangeBulletColor").performed += SwitchBulletColorAction;

            //And the most important thing, Enable the actionmap
            m_Input.FindActionMap("PlayerActionMap").Enable();
        }

        void Start()
        {
            //Initializing actual color
            SwitchBulletColor();
        }

        private void FixedUpdate()
        {
            //We capture the direction set on our controller. This would be for keyboard input.
            Vector2 direction = m_MovementInput.ReadValue<Vector2>();
            //We clamp the rotation giving it a minimum and maximum angle so the rotation has limits aside
            float currentRotation = Mathf.Clamp(m_RigidBody.rotation, m_minClamp, m_maxClamp);
            m_RigidBody.MoveRotation(currentRotation + (direction.x * m_PlayerRotationSpeed));
            /*  A simple way to give angles per second velocity with no limitations!
                 m_RigidBody.angularVelocity = m_PlayerAngularSpeed * direction.x;
            */
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
        }

        private void MovementPerformed(InputAction.CallbackContext context)
        {
            Debug.Log(context);
        }

        private void ShootingAction(InputAction.CallbackContext context)
        {
            Shooting();
            Debug.Log(context);
        }

        private void SwitchBulletColorAction(InputAction.CallbackContext context)
        {
            SwitchBulletColor();
            Debug.Log(context);
        }

        private void Shooting()
        {
            /* WIHOUT POOL */
            //GameObject m_CurrentBullet = Instantiate(m_PlayerBullet, m_Cannon.transform.position, transform.localRotation);
            /* WITH POOL */
            GameObject m_CurrentBullet = m_BulletPool.GetElement(this.gameObject);
            //m_CurrentBullet.transform.rotation = transform.rotation;
            m_CurrentBullet.transform.position = m_Cannon.transform.position;           
            Debug.Log("Bullet " + m_CurrentBullet.gameObject.name + " Rotation: " + m_CurrentBullet.transform.rotation);
            SpriteRenderer m_Sprite = m_CurrentBullet.GetComponent<SpriteRenderer>();
            m_Sprite.color = m_BulletColor;
            
        }
        private void SwitchBulletColor()
        {
            //It will switch the color of the button and bullets
            m_BulletColor = m_Colors[m_colorCount];
            m_ShootingButtonSprite = m_ShootingButton.GetComponent<Image>();
            m_ShootingButtonSprite.color = m_BulletColor;
            //Remember that Array.Count count its elements starting by 1, so a -1 is needed since the first element is 0.
            if (m_colorCount < m_Colors.Count - 1)
            {
                m_colorCount++;
            }
            else
            {
                m_colorCount = 0;
            }
        }

        private void ReceiveDamage()
        {

        }
    }
}
