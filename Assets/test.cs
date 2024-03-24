using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private Rigidbody2D rig;
    public int canJump;
    public int currentJump;
    public LayerMask ground;
    public float jumpForce;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.gravityScale = 3;
    }

    void Update()
    {
        if (rig.IsTouchingLayers(ground)&&rig.velocity.y==0)
        {
            currentJump = canJump;
        }

        if (Input.GetKeyDown(KeyCode.Space)&&currentJump-->0)
        {
            rig.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
        
    }
}
