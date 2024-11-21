using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    [SerializeField] private Animator animator;

    public void Walking()
    {
        animator.SetBool("isRun", true);
        animator.SetBool("isIdle", false);
    }
    
    public void Idle()
    {
        animator.SetBool("isRun", false);
        animator.SetBool("isIdle", true);
    }
}
