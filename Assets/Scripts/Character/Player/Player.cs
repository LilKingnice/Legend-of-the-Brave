using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : Character
{
    private LegendBrave _inputSystem;
    private Rigidbody2D player;
    private SpriteRenderer playerFlip;
    private Vector2 moveDirection;
    [SerializeField]private float hurtForce;
    [SerializeField]private float speed;

    [Header("材质")] 
    [SerializeField]private PhysicsMaterial2D normal;
    [SerializeField]private PhysicsMaterial2D smooth;
    
    
    private PhysicsCheck physicsCheck;
    [SerializeField] private float jumpForce;
    [SerializeField]private int maxJumpCount=2;
    [SerializeField]private int currentJumpCount=0;

    private bool isGround;//地面检测
    
    public bool IsHurting { get; set; }//表示受伤状态
    public bool IsAttacking { get; set; }//表示攻击状态
    
    

    private Animator playerAnimator;
    
    private void Awake()
    {
        _inputSystem = new LegendBrave();
        player = GetComponent<Rigidbody2D>();
        playerFlip = GetComponent<SpriteRenderer>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimator = GetComponent<Animator>();
    }
    
    protected override void Start()
    {
        base.Start();
        OnHurt.AddListener(PlayHurtAnimation);
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
        _inputSystem.GamePlay.Jump.started -= OnJump;
    }

    private void DisableAllInputs()
    {
        _inputSystem.Disable();
        
    }

    private void Update()
    {
        moveDirection.x=_inputSystem.GamePlay.Move.ReadValue<Vector2>().x;
        moveDirection.y = player.velocity.y;
        AnimationCheck();
    }

    private void FixedUpdate()
    {
        if (!IsHurting&&!IsAttacking)
        {
            PlayerMove();
        }
        isGround=physicsCheck.playerGroundCheck;
        
        if (isGround&&player.velocity.y<0.1)
        {
            playerAnimator.SetBool("isGround",true);
            currentJumpCount = 0;
            player.sharedMaterial = normal;
        }
    }
    
    #region 移动逻辑
    private void PlayerMove()
    {
        // if (moveDirection.x > 0) PlayerFlip(false);
        // if (moveDirection.x < 0) PlayerFlip(true);

        if (moveDirection.x > 0) PlayerFlip(1);
        if (moveDirection.x < 0) PlayerFlip(-1);
        player.velocity = new Vector2(moveDirection.x * speed *Time.deltaTime,player.velocity.y);
    }

    private void PlayerFlip(float faceTo)
    {
        //无法一起修改Collider
        //playerFlip.flipX = faceTo;
        transform.localScale = new Vector3(faceTo, 1, 1);
        
    }
    #endregion
    
    
    #region 跳跃逻辑
    public void OnJump(InputAction.CallbackContext obj)
    {
        //多段跳
        if (isGround||currentJumpCount<maxJumpCount)
        {
            player.sharedMaterial = smooth;
            currentJumpCount++;
            player.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    #endregion
    
    
    #region 攻击逻辑
    private void OnAttack(InputAction.CallbackContext obj)
    {
        
        PlayAttackAnimation();
    }
    #endregion

    #region 受伤逻辑
    
    protected override void Die()
    {
        base.Die();
        playerAnimator.SetBool("isDead", true);
        DisableAllInputs();
    }
    
    
    #endregion
    
    #region 动画处理
    public void AnimationCheck()
    {
        playerAnimator.SetFloat("Velocity",Mathf.Abs(moveDirection.x));
        playerAnimator.SetFloat("JumpHeight",moveDirection.y);
        if (!isGround)
        {
            playerAnimator.SetBool("isGround",false);
        }
    }
    
    protected override void PlayHurtAnimation()
    {
        playerAnimator.SetTrigger("Hurting");
        IsHurting = true;//暂停移动
        player.velocity = Vector2.zero;
        //Vector2 dir = new Vector2((transform.position.x - others.position.x), 0).normalized;
        Vector2 dir = new Vector2((transform.localScale.x *-1), 0).normalized;
        player.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        Debug.Log("player重写");
    }

    private void PlayAttackAnimation()
    {
        playerAnimator.SetTrigger("Attacking");
        IsAttacking = true;//暂停移动
    }

    #endregion
}
