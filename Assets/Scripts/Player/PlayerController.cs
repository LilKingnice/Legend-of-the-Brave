using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private LegendBrave _inputSystem;
    private Rigidbody2D player;
    private SpriteRenderer playerFlip;
    private Vector2 moveDirection;
    [SerializeField]private float speed;
    
    
    private PhysicsCheck playergroundcheck;
    [SerializeField] private float jumpForce;
    [SerializeField]private int maxJumpCount=2;
    [SerializeField]private int currentJumpCount=0;
    
    private bool isGround;
    private bool doubleJump;

    private bool canResetJumpCount;

    
    
    private void Awake()
    {
        _inputSystem = new LegendBrave();
        player = GetComponent<Rigidbody2D>();
        playerFlip = GetComponent<SpriteRenderer>();
        playergroundcheck = GetComponent<PhysicsCheck>();
    }

    private void OnEnable()
    {
        _inputSystem.Enable();
        _inputSystem.GamePlay.Jump.started += OnJump;
    }



    private void OnDisable()
    {
        DisableAllInputs();
    }

    private void DisableAllInputs()
    {
        _inputSystem.Disable();
    }

    private void Update()
    {
        moveDirection=_inputSystem.GamePlay.Move.ReadValue<Vector2>();

    }

    private void FixedUpdate()
    {
        PlayerMove();
        isGround=playergroundcheck.IsGround();
        if (isGround&&canResetJumpCount)
        {
            currentJumpCount = 0;
        }
    }

    private void PlayerMove()
    {
        if (moveDirection.x > 0) PlayerFlip(false);
        if (moveDirection.x < 0) PlayerFlip(true);
        
        player.velocity = new Vector2(moveDirection.x * speed *Time.deltaTime,player.velocity.y);
    }

    private void PlayerFlip(bool faceTo)
    {
        playerFlip.flipX = faceTo;
    }
    
    public void OnJump(InputAction.CallbackContext obj)
    {
        //简单判断二段跳
        // if (isGround)
        // {
        //     player.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        //     doubleJump = true;
        // }
        // else if (doubleJump)
        // {
        //     player.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        //     doubleJump = false;
        // }
        
        //多段跳
        //PhysicsCheck.playerGroundCheck = false;
        //Debug.LogWarning($"按键按下{currentJumpCount}");
        if (currentJumpCount<maxJumpCount)
        {
            currentJumpCount++;
            canResetJumpCount = false;
            player.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(nameof(EnableJumpReset));
        }
    }

    IEnumerator EnableJumpReset()
    {
        yield return new WaitForSeconds(0.5f);
        canResetJumpCount = true;
    }
}


