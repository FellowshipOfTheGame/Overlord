using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetVelocity(float vel)
    {
        anim.SetFloat("velocity", Mathf.Abs(vel));
    }

    public void SetGrounded(bool grounded)
    {
        anim.SetBool("grounded", grounded);
    }

    public void SetGrabbing(bool grabbing)
    {
        anim.SetBool("grabbing", grabbing);
    }

    internal void Attack()
    {
        anim.SetTrigger("attack");
    }
}
