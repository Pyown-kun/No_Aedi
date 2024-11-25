using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator animator;

    public void Idle()
    {
        animator.SetBool("isAttack", false);
        animator.SetBool("isRun", false);
    }

    public void Attack()
    {
        animator.SetBool("isAttack", true);
        animator.SetBool("isRun", false);
    }
    public void Running()
    {
        animator.SetBool("isRun", true);
        animator.SetBool("isAttack", false);
    }
}
