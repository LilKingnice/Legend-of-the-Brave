using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("基础属性")]
    private LegendBrave _inputSystem;
    private Rigidbody2D player;
    private SpriteRenderer playerFlip;
    private Vector2 moveDirection;
    [SerializeField]private float speed;
    
    
    private PhysicsCheck playergroundcheck;
    [SerializeField] private float jumpForce;
    [SerializeField]private int maxJumpCount=2;
    [SerializeField]private int currentJumpCount=0;

    private bool isGround;//地面检测
    private bool canResetJumpCount;//多段跳检测
    public bool IsHurting { get; private set; }

    private Animator playerAnimator;
    
    private void Awake()
    {
        _inputSystem = new LegendBrave();
        player = GetComponent<Rigidbody2D>();
        playerFlip = GetComponent<SpriteRenderer>();
        playergroundcheck = GetComponent<PhysicsCheck>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _inputSystem.Enable();
        _inputSystem.GamePlay.Jump.started += OnJump;
        _inputSystem.GamePlay.Attack.started+= OnAttack;
    }
    
    private void OnDisable()
    {
        DisableAllInputs();
    }

    private void DisableAllInputs()
    {
        _inputSystem.Disable();
        _inputSystem.GamePlay.Jump.started -= OnJump;
    }

    private void Update()
    {
        moveDirection=_inputSystem.GamePlay.Move.ReadValue<Vector2>();
        moveDirection.y = player.velocity.y;
        AnimationCheck();
    }

    private void FixedUpdate()
    {
        if (!IsHurting)
        {
            PlayerMove();
        }
        isGround=playergroundcheck.IsGround();
        
        if (isGround&&canResetJumpCount)
        {
            playerAnimator.SetBool("isGround",true);
            currentJumpCount = 0;
        }
    }

    #region 动画处理

    public void AnimationCheck()
    {
        playerAnimator.SetFloat("Velocity",Mathf.Abs(moveDirection.x));
        playerAnimator.SetFloat("JumpHeight",moveDirection.y);
        
    }

    #endregion

    #region 移动逻辑
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
    #endregion
    
    
    #region 跳跃逻辑
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
        if (currentJumpCount<maxJumpCount)
        {
            playerAnimator.SetBool("isGround",false);
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
    #endregion
    
    
    #region 攻击逻辑
    private void OnAttack(InputAction.CallbackContext obj)
    {
        playerAnimator.SetTrigger("Attacking");
    }
    #endregion
    
    
}


