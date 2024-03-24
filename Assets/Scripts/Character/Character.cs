using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;

    [Tooltip("免伤时间")]
    public float invulnerableTime=2f;
    protected bool invulnerable = false;//受伤免疫锁

    [HideInInspector]public UnityEvent OnHurt;
    
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        OnHurt.AddListener(PlayHurtAnimation);
    }

    public virtual void TackDamage(AttackSettings attackter)
    {
        if (invulnerable) return;
        if (currentHealth > 0)
        {
            OnHurt?.Invoke();
            currentHealth -= attackter.damage;
            invulnerable = true;
            
            StartCoroutine(nameof(InvulnerableIEnumerator));
        }
        if (currentHealth<=0)
        {
            currentHealth = 0;
            Die();
        }
    }
    
    protected virtual void PlayHurtAnimation()
    {
        Debug.Log($"{gameObject.name}:处理受伤事件");
    }
    
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name}:is dead");
    }
    
    //受伤免疫计时器
    protected virtual IEnumerator InvulnerableIEnumerator()
    {
        yield return new WaitForSeconds(invulnerableTime);
        invulnerable = false;
    }
    
    protected virtual void Move()
    {
        
    }
    
    private void PlayerFlip(float faceTo)
    {
        //无法一起修改Collider
        //playerFlip.flipX = faceTo;
        transform.localScale = new Vector3(faceTo, 1, 1);
        
    }
}
