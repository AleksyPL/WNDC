using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementBase))]
public class PlayerMovementAnimator : MonoBehaviour
{
    private PlayerMovementBase baseMovementScript;
    private GameObject rightArm;
    private GameObject leftArm;
    private GameObject rightArmIKSolver;
    private GameObject leftArmIKSolver;
    private GameObject rightLegIKSolver;
    private GameObject leftLegIKSolver;
    internal bool aimWithLeftArm;
    internal bool aimWithRightArm;
    internal Animator animator;
    public GameObject[] bonesAndLimbs;
    void Start()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
        rightArm = GameObject.Find("RightArmUp");
        leftArm = GameObject.Find("LeftArmUp");
        leftArmIKSolver = GameObject.Find("LeftArmSolver");
        rightArmIKSolver = GameObject.Find("RightArmSolver");
        leftLegIKSolver = GameObject.Find("LeftLegSolver");
        rightLegIKSolver = GameObject.Find("RightLegSolver");
        aimWithLeftArm = false;
        aimWithRightArm = true;
        animator = GetComponent<Animator>();
        ToggleRagdoll(baseMovementScript.surroundingsCheckerScript.isDead);
    }
    private void CheckIKSolversAndAnimationLayers()
    {
        if(aimWithLeftArm)
        {
            leftArmIKSolver.SetActive(false);
            animator.SetLayerWeight(animator.GetLayerIndex("Left Arm"), 0);
        }
        else
        {
            leftArmIKSolver.SetActive(true);
            animator.SetLayerWeight(animator.GetLayerIndex("Left Arm"), 1);
        }
        if (aimWithRightArm)
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
    internal void ToggleRagdoll(bool argument)
    {
        leftArmIKSolver.SetActive(!argument);
        rightArmIKSolver.SetActive(!argument);
        leftLegIKSolver.SetActive(!argument);
        rightLegIKSolver.SetActive(!argument);
        baseMovementScript.myRigidBody.simulated = !argument;
        baseMovementScript.boxCollider.enabled = !argument;
        animator.enabled = !argument;
        for (int i = 0; i < bonesAndLimbs.Length; i++)
        {
            if(bonesAndLimbs[i].GetComponent<Rigidbody2D>())
            {
                bonesAndLimbs[i].GetComponent<Rigidbody2D>().simulated = argument;
            }
            if(bonesAndLimbs[i].GetComponent<CapsuleCollider2D>())
            {
                bonesAndLimbs[i].GetComponent<CapsuleCollider2D>().enabled = argument;
            }
            if(bonesAndLimbs[i].GetComponent<HingeJoint2D>())
            {
                bonesAndLimbs[i].GetComponent<HingeJoint2D>().enabled = argument;
            }
        }
    }
    void Update()
    {
        CheckIKSolversAndAnimationLayers();
        if(aimWithRightArm)
            CalculateAiming(rightArm);
        if(aimWithLeftArm)
            CalculateAiming(leftArm);
        if (baseMovementScript.surroundingsCheckerScript.isDead)
            ToggleRagdoll(true);
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
