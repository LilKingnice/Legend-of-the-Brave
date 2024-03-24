using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    protected Rigidbody2D enemyRig;
    protected Animator enemyAnimator;
    [Header("基本属性")] 
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float chaseSpeed;
    [SerializeField] protected float currentSpeed;
    [SerializeField] protected Transform leftDestination;
    [SerializeField] protected Transform RightDestination;
    
    private Vector2 faceDir;

    private void Awake()
    {
        enemyRig = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
    }
    protected override void Start()
    {
        base.Start();
        moveSpeed = currentSpeed;
    }
    private void Update()
    {
        faceDir = new Vector2(-transform.localScale.x, 0);
    }

    private void FixedUpdate()
    {
        //AutoMove();
    }

    private bool isWeapon;


    protected override void PlayHurtAnimation()
    {
        Debug.Log("敌人重写");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerWeapon"))
        {
            isWeapon = true;
        }
    }

    public override void TackDamage(AttackSettings attackter)
    {
        if (isWeapon)
        {
            isWeapon = false;
            base.TackDamage(attackter);
        }
    }
    
    protected override void Die()
    {
        base.Die();
    }

    // protected virtual void AutoMove()
    // {
    //     if (transform.position==leftDestination.position)
    //     {
    //         Vector2 dir = new Vector2((transform.localScale.x *-1), 0).normalized;
    //         enemyRig.velocity = new Vector2(-faceDir.x * currentSpeed, enemyRig.velocity.y);
    //     }
    //     enemyRig.velocity = new Vector2(faceDir.x * currentSpeed, enemyRig.velocity.y);
    //     
    //     
    // }
}
