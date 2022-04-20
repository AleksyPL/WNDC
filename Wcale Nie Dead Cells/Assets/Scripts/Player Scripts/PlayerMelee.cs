using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    internal PlayerMovementBase baseMovementScript;
    void Start()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
    }
    void Update()
    {
        if(baseMovementScript.mainPlayerScript.inventory.weapons[baseMovementScript.shootingScript.activeWeaponIndex].activeWeaponType == Weapon.WeaponType.melee && baseMovementScript.inputScript.LMB_pressed)
        {
            baseMovementScript.animatorScript.animator.SetBool("MeleeAttack", true);
        }
    }
    public void FinishMeleeAttack()
    {
        baseMovementScript.animatorScript.animator.SetBool("MeleeAttack", false);
    }
}
