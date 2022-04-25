using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(PlayerMovementBase))]
//[RequireComponent(typeof(PlayerInventory))]
public class Player : MonoBehaviour
{
    public enum StateMachine
    {
        idle,
        running,
        //attack,
        falling,
        jumping,
        chainClimbing,
        ropeGrappling,
        //sliding,
        dashing,
        dead
    }
    private float HP;
    internal PlayerMovementBase baseMovementScript;
    internal PlayerInventory inventory;
    internal StateMachine currentState;
    private void OnEnable()
    {
        LoadPlayerStats();
        baseMovementScript = GetComponent<PlayerMovementBase>();
        inventory = GetComponent<PlayerInventory>();
    }
    private void LoadPlayerStats()
    {
        HP = 10;
    }
    public void DoDamage(float receivedGiven)
    {
        HP -= receivedGiven;
        if(HP<=0)
        {
            currentState = StateMachine.dead;
            baseMovementScript.animatorScript.ragdollScript.ToggleRagdoll(true);
        }
    }
}
