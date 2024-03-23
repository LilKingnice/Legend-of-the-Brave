using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PhysicsCheck : MonoBehaviour
{
    public Vector2 BottomOffset;
    //public bool playerGroundCheck;
    public float checkRadius;
    public LayerMask chackingMask;
    

    //玩家地面检测
    public bool IsGround()
    {
        // playerGroundCheck= Physics2D.OverlapCircle((Vector2)transform.position+BottomOffset, checkRadius, chackingMask);
        // Debug.Log("Checking...:"+playerGroundCheck);
        //return playerGroundCheck;
        
        return Physics2D.OverlapCircle((Vector2)transform.position+BottomOffset, checkRadius, chackingMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position+BottomOffset, checkRadius);
    }
    
}

