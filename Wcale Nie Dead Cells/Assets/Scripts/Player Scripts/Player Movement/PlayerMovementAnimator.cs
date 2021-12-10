using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAnimator : MonoBehaviour
{
    [SerializeField]
    internal PlayerMovementBase baseMovementScript;
    internal Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    internal void AnimateCharacter()
    {
        if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.walk)
        {
            animator.SetBool("Moving", true);
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("LedgeClimbing", false);
            animator.SetBool("LadderClimbing", false);
            animator.SetBool("RopeSwinging", false);
            animator.SetBool("Dashing", false);
        }
        else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.jump)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Jumping", true);
            animator.SetBool("Falling", false);
            animator.SetBool("LedgeClimbing", false);
            animator.SetBool("LadderClimbing", false);
            animator.SetBool("RopeSwinging", false);
            animator.SetBool("Dashing", false);
        }
        else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.fall)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", true);
            animator.SetBool("LedgeClimbing", false);
            animator.SetBool("LadderClimbing", false);
            animator.SetBool("RopeSwinging", false);
            animator.SetBool("Dashing", false);
        }
        else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.ledgeClimbing)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("LedgeClimbing", true);
            animator.SetBool("LadderClimbing", false);
            animator.SetBool("RopeSwinging", false);
            animator.SetBool("Dashing", false);
        }
        else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.chainClimbing)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("LedgeClimbing", false);
            animator.SetBool("LadderClimbing", true);
            animator.SetBool("RopeSwinging", false);
            animator.SetBool("Dashing", false);
        }
        else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.idle)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("LedgeClimbing", false);
            animator.SetBool("LadderClimbing", false);
            animator.SetBool("RopeSwinging", false);
            animator.SetBool("Dashing", false);
        }
        else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.ropeGrappling)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("LedgeClimbing", false);
            animator.SetBool("LadderClimbing", false);
            animator.SetBool("RopeSwinging", true);
            animator.SetBool("Dashing", false);
        }
        else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.dash)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("LedgeClimbing", false);
            animator.SetBool("LadderClimbing", false);
            animator.SetBool("RopeSwinging", false);
            animator.SetBool("Dashing", true);
        }
    }
}
