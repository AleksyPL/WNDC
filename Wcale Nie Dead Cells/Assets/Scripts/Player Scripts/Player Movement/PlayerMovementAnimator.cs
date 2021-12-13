using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementBase))]
public class PlayerMovementAnimator : MonoBehaviour
{
    private PlayerMovementBase baseMovementScript;
    internal Animator animator;
    //public GameObject AimingCircle;
    void Start()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
        animator = GetComponent<Animator>();
    }
    internal void AnimateCharacter()
    {
        if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.running)
        {
            if((baseMovementScript.surroundingsCheckerScript.isFacingRight && baseMovementScript.inputScript.position.x > 0) || (!baseMovementScript.surroundingsCheckerScript.isFacingRight && baseMovementScript.inputScript.position.x < 0))
            {
                animator.SetFloat("RunDirection", 1);
            }
            else if ((!baseMovementScript.surroundingsCheckerScript.isFacingRight && baseMovementScript.inputScript.position.x > 0) || (baseMovementScript.surroundingsCheckerScript.isFacingRight && baseMovementScript.inputScript.position.x < 0))
            {
                animator.SetFloat("RunDirection", -1);
            }
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
        }
        else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.jumping)
        {
            animator.SetBool("Jumping", true);
            animator.SetBool("Falling", false);
        }
        else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.falling)
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", true);
        }
        //else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.chainClimbing)
        //{
        //    animator.SetBool("Moving", false);
        //    animator.SetBool("Jumping", false);
        //    animator.SetBool("Falling", false);
        //    animator.SetBool("LedgeClimbing", false);
        //    animator.SetBool("LadderClimbing", true);
        //    animator.SetBool("RopeSwinging", false);
        //    animator.SetBool("Dashing", false);
        //}
        else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.idle)
        {
            animator.SetFloat("RunDirection", 0);
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
        }
        else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.ropeGrappling)
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", true);
        }
    }
}
