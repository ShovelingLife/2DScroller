using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputComponent : MonoBehaviour
{
    Player player;
    PlayerInputAsset playerInputAsset;

    InputAction move;
    Vector2 moveDir = Vector2.zero;

    [SerializeField]
    float speed;

    InputAction jump;
    Vector2 jumpDir = Vector2.zero;

    [SerializeField]
    float jumpSpeed;

    [SerializeField]
    float doubleJumpCheckTime;

    float curDoubleJumpCheckTime;

    int jumpCnt = 0;

    public float slideSpeed;

    bool isSliding = false;



    private void Awake()
    {
        Global.inputComp = this;
        player = transform.parent.GetComponent<Player>();
        //controller = GetComponent<CharacterController>();
        BindActions();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            LevelManager.instance.SetPlayerOnStart();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // No player in world
        if (player == null)
        {
            Debug.LogError("-----NO PLAYER-----");
            return;
        }

        #region Move

        player.position += (Vector3)moveDir;

        #endregion

        #region Jump

        if (jumpCnt > 0)
            curDoubleJumpCheckTime += Time.fixedDeltaTime; 

        if (curDoubleJumpCheckTime > doubleJumpCheckTime)
        {
            jumpCnt = 0;
            curDoubleJumpCheckTime = 0f;
            jumpDir = Vector3.zero;
        }
        player.position += (Vector3)jumpDir;
        
        #endregion
    }

    void BindActions()
    {
        playerInputAsset = new PlayerInputAsset();

        #region Movement
        move = playerInputAsset.Player.Move;
        Action<InputAction.CallbackContext> Move = (InputAction.CallbackContext context) =>
        {
            moveDir = context.ReadValue<Vector2>() * speed * Time.fixedDeltaTime;

            // Rotate the player to facing side
            if (moveDir.x < 0)
                player.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            else if (moveDir.x > 0)
                player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        };

        if (move != null)
        {
            move.performed += Move;
            move.canceled  += Move;
        }
        #endregion
        return;
        #region Jump
        jump = playerInputAsset.Player.Jump;

        if (jump != null)
        {
            jump.performed += (InputAction.CallbackContext context) =>
            {
                var moveVal = context.ReadValue<Vector2>() * jumpSpeed * Time.fixedDeltaTime;

                if (jumpCnt.Equals(1))
                    jumpDir += moveVal;

                else if (jumpCnt.Equals(0))
                    jumpDir = moveVal;

                jumpCnt++;
            };
            jump.canceled  += (InputAction.CallbackContext context) =>
            {
                //jumpDir = Vector3.zero;
            }; ;
        }
        #endregion
    }

    private void OnEnable()
    {
        move?.Enable();
        jump?.Enable();   
    }

    private void OnDisable()
    {
        move?.Disable();
        jump?.Disable();
    }


    public void CheckCollision(Collision2D collision)
    {
        var collidedObj = collision.gameObject;
        isSliding = false;

        // Jump and then entered
        if (Global.IsSameLayer(collidedObj, "Floor") &&
            jumpCnt > 0)
        {
            // Check for continuous press 
            if (jump.IsPressed())
            {
                jumpDir = jump.ReadValue<Vector2>() * jumpSpeed * Time.fixedDeltaTime;
                jumpCnt = 1;
            }
            else
            {
                if (!isSliding)
                {
                    jumpDir = Vector3.zero;
                    jumpCnt = 0;
                }
            }
            curDoubleJumpCheckTime = 0f;
        }
        // Slide
        else if(Global.IsSameLayer(collidedObj, "Wall"))
        {
            player.SetVelocity(slideSpeed);
            isSliding = true;
        }
    }
}