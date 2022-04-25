using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(PlayerMovementBase))]
public class PlayerMovementAnimator : MonoBehaviour
{
    private PlayerMovementBase baseMovementScript;
    public GameObject rightArm;
    public GameObject leftArm;
    public GameObject rightArmIKSolver;
    public GameObject leftArmIKSolver;
    internal Ragdoll ragdollScript;
    internal Animator animator;
    void Start()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
        animator = GetComponent<Animator>();
        ragdollScript = GetComponent<Ragdoll>();
        CheckIKSolversAndAnimationLayers();
    }
    private void CheckIKSolversAndAnimationLayers()
    {
        if(baseMovementScript.shootingScript.aimWithLeftArm)
        {
            leftArmIKSolver.SetActive(false);
            //animator.SetFloat("RunDirection", 0);
            animator.SetLayerWeight(animator.GetLayerIndex("Left Arm"), 0);
        }
        else
        {
            leftArmIKSolver.SetActive(true);
            animator.SetLayerWeight(animator.GetLayerIndex("Left Arm"), 1);
        }
        if (baseMovementScript.shootingScript.aimWithRightArm)
        {
            rightArmIKSolver.SetActive(false);
            //animator.SetFloat("RunDirection", 0);
            animator.SetLayerWeight(animator.GetLayerIndex("Right Arm"), 0);
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
        if (baseMovementScript.shootingScript.aimWithRightArm)
            CalculateAiming(rightArm);
        if (baseMovementScript.shootingScript.aimWithLeftArm)
            CalculateAiming(leftArm);
        AnimateCharacter();
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
        Vector3 weaponOffset = Vector3.zero;
       
        if (baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].activeWeaponType == Weapon.WeaponType.melee)
        {
            weaponOffset = new Vector3(0, baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].localOffsetGrip.y, 0);
        }
        else if (baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].activeWeaponType == Weapon.WeaponType.firearm)
        {
            //float weaponOffsetF = baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].localOffsetShootingPoint.y
            //     + baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].localOffsetGrip.y;
            //Instantiate(null, arm.transform, arm);
            //Debug.DrawLine(baseMovementScript.inputScript.mousePosition, new Vector3(0, baseMovementScript.inputScript.mousePosition.y + weaponOffsetF, 0));
           //if (baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].localOffsetShootingPoint.y >= baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].localOffsetGrip.y)
           //{
           //    //weaponOffset = new Vector3(0, baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].localOffsetShootingPoint.y, 0);
           //    //weaponOffset = baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].localOffsetShootingPoint;
           //    weaponOffset = baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].localOffsetShootingPoint * baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].localOffsetGrip;
           //    weaponOffset = new Vector3(weaponOffset.x * 0.69f, weaponOffset.y * 0.69f);
           //}
           //else
           //{
           //    //weaponOffset = new Vector3(0, baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].localOffsetGrip.y, 0);
           //    weaponOffset = baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].localOffsetGrip;
           //}
            weaponOffset = baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].localOffsetShootingPoint
                + baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].localOffsetGrip;
            weaponOffset = new Vector3(weaponOffset.x * 0.69f, weaponOffset.y * 0.69f);
        }
        Vector3 perendicular = arm.transform.position - baseMovementScript.inputScript.mousePosition + weaponOffset;
        //Vector3 perendicular = arm.transform.position - baseMovementScript.inputScript.mousePosition;
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
