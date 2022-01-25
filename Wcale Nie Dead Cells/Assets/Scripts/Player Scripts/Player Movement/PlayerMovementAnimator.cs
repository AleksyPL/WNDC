using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(PlayerMovementBase))]
public class PlayerMovementAnimator : MonoBehaviour
{
    private PlayerMovementBase baseMovementScript;
    private Ragdoll ragdollScript;
    public GameObject rightArm;
    public GameObject leftArm;
    public GameObject rightArmIKSolver;
    public GameObject leftArmIKSolver;
    internal Animator animator;
    void Start()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
        animator = GetComponent<Animator>();
        ragdollScript = GetComponent<Ragdoll>();
    }
    private void CheckIKSolversAndAnimationLayers()
    {
        if(baseMovementScript.shootingScript1.aimWithLeftArm)
        {
            leftArmIKSolver.SetActive(false);
            animator.SetLayerWeight(animator.GetLayerIndex("Left Arm"), 0);
        }
        else
        {
            leftArmIKSolver.SetActive(true);
            animator.SetLayerWeight(animator.GetLayerIndex("Left Arm"), 1);
        }
        if (baseMovementScript.shootingScript1.aimWithRightArm)
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Right Arm"), 0);
            rightArmIKSolver.SetActive(false);
        } 
        else
        {
            rightArmIKSolver.SetActive(true);
            animator.SetLayerWeight(animator.GetLayerIndex("Right Arm"), 1);
        }  
    }
    void Update()
    {
        CheckIKSolversAndAnimationLayers();
        if(baseMovementScript.shootingScript1.aimWithRightArm)
            CalculateAiming(rightArm);
        if (baseMovementScript.shootingScript1.aimWithLeftArm)
            CalculateAiming(leftArm);
        if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.dead)
            ragdollScript.ToggleRagdoll(true);
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
    private void CalculateAiming(GameObject arm)
    {
        Vector3 perendicular = arm.transform.position - baseMovementScript.inputScript.mousePosition;
        Quaternion value = Quaternion.LookRotation(Vector3.forward, perendicular);
        if (baseMovementScript.surroundingsCheckerScript.isFacingRight)
        {
            value *= Quaternion.Euler(0, 0, -90f);
        }
        else
        {
            value *= Quaternion.Euler(0, 180, -90f);
        }
        arm.transform.rotation = value;
    }
}
