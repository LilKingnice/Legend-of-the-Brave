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
    [SerializeField] protected float damage;

    protected bool invulnerable = false;//受伤免疫锁

    public UnityEvent<Transform> OnHurt;
    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log(other.transform.name);
        TackDamage(damage,other.transform);
    }

    protected virtual void TackDamage(float damage,Transform other)
    {
        if (invulnerable) return;
        if (currentHealth != 0)
        {
            OnHurt?.Invoke(other);
            
            currentHealth -= damage;
            invulnerable = true;
            StartCoroutine(nameof(InvulnerableIEnumerator));
        }
        if (currentHealth<=0)
        {
            currentHealth = 0;
            Die();
        }
        
    }
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name}:is dead");
    }
    
    //受伤免疫计时器
    protected virtual IEnumerator InvulnerableIEnumerator()
    {
        yield return new WaitForSeconds(2f);
        invulnerable = false;
    }
}
