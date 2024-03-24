using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSettings : MonoBehaviour
{
    [Header("伤害属性")]
    public float damage;
    public float attackRange;
    public float attackRate;
    
    public void OnTriggerStay2D(Collider2D other)
    {
        other.GetComponent<Character>().TackDamage(this);
    }
    
    // protected void OnTriggerEnter2D(Collider2D other)
    // {
    //     other.GetComponent<Player>().TackDamage(this);
    // }
}
