using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace murciainvaders
{
    public class PlayerBehaviour : MonoBehaviour
    {
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
        //Instance of InputActions!
        private InputActionAsset m_Input;
        private InputAction m_MovementInput;

        //Speed parameter
        [SerializeField]
        private float m_PlayerSpeed = 3f;

        //For avoiding using some binary stuff, we set the layer through the Unity inspector with this
        [SerializeField]
        private LayerMask m_ActionLayerMask;

        //Actual bullet color
        private Color m_BulletColor;
        private int m_colorCount = 0;

        //Array of BulletColors
        [SerializeField]
        private List<Color> m_Colors = new List<Color>();

        private void Awake()
        {
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
            //Loading components
            //Initializing actual color
            SwitchBulletColor();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void MovementPerformed(InputAction.CallbackContext context)
        {
            Debug.Log(context);
        }

        private void ShootingAction(InputAction.CallbackContext context)
        {
            Debug.Log(context);
        }

        private void SwitchBulletColorAction(InputAction.CallbackContext context)
        {
            SwitchBulletColor();
            Debug.Log(context);
        }

        private void SwitchBulletColor()
        {
            //It will switch the color of the button and bullets
            m_BulletColor = m_Colors[m_colorCount];
            m_ShootingButtonSprite = m_ShootingButton.GetComponent<Image>();
            m_ShootingButtonSprite.color = m_BulletColor;
            if (m_colorCount < m_Colors.Count)
            {
                m_colorCount++;
            }
            else
            {
                m_colorCount = 0;
            }
        }

        private void Shoot()
        {

        }

        private void SwitchColor()
        {

        }
    }
}
