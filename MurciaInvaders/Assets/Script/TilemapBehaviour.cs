using System.Collections;
using System.Collections.Generic;
using murciainvaders;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TilemapBehaviour : MonoBehaviour
{
    [Header("Object references from the scene")]
    [SerializeField]
    private Tilemap m_BaseTilemap;
    [SerializeField]
    private Tilemap m_CharacterTilemap;

    //Sprite references
    [SerializeField]
    private Sprite m_AlienSprite;
    [SerializeField]
    private Sprite m_MurcianoSprite;
    [SerializeField]
    private Sprite m_DeadAlienSprite;
    [SerializeField]
    private Sprite m_DeadMurcianoSprite;

    //Tile references
    [SerializeField]
    private Tile m_AlienTile;
    [SerializeField]
    private Tile m_MurcianoTile;
    [SerializeField]
    private Tile m_DeadAlienTile;
    [SerializeField]
    private Tile m_DeadMurcianoTile;

    //Reference to InputActions: Input controls to move the cannon
    [SerializeField]
    private InputActionAsset m_InputActions;
    //Instance of InputActions!
    private InputActionAsset m_Input;
    private InputAction m_ClickInput;
    private InputAction m_PointerPosition;

    GameManager m_GameManager;
    Camera m_Cam;


    private void Awake()
    {
        //Instance of InputActions
        m_Input = Instantiate(m_InputActions);
        //We set the controls of this scene (just on click or touch the screen)
        m_ClickInput = m_Input.FindActionMap("PlayerActionMap").FindAction("Click");
        m_ClickInput.performed += CaptureClick;
        m_PointerPosition = m_Input.FindActionMap("PlayerActionMap").FindAction("PointerPosition");

        //And the most important thing, Enable the actionmap
        m_Input.FindActionMap("PlayerActionMap").Enable();

        m_CharacterTilemap = GetComponent<Tilemap>();
        m_Cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        //First, we set the reference to the instance of the gamemanager
        m_GameManager = GameManager.GameManagerInstance;
        int tilemapLengthX = m_BaseTilemap.cellBounds.x;
        int tilemapLengthY = m_BaseTilemap.cellBounds.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CaptureClick(InputAction.CallbackContext context)
    {
        //Mouse coordinates on screen
        Vector2 pointerPosition = m_PointerPosition.ReadValue<Vector2>();

        //First, we translate the pointer coordinates to tilemap
        Vector3Int tilePosition = m_CharacterTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(pointerPosition));
        tilePosition.z = 0; //We should give a value of 0 to Z
    }

    private void SetCharactersTileMap()
    {
        int tilemapLengthX = m_BaseTilemap.cellBounds.xMax;
        int tilemapLengthY = m_BaseTilemap.cellBounds.yMax;

        for(int x = 0; x < tilemapLengthX; x++)
        {
            for(int y = 0; y < tilemapLengthY; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                //Tile currentTile = m_CharacterTilemap.GetTile(position);
            }
        }


        
    }
}
