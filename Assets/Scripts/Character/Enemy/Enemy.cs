using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    protected override void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerStay2D(other);
    }

    protected override void TackDamage(float damage, Transform other)
    {
        base.TackDamage(damage, other);
    }

    protected override void Die()
    {
        base.Die();
    }
}
