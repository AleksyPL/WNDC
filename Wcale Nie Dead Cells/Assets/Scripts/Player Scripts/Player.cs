using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMovementBase))]
[RequireComponent(typeof(PlayerInventory))]
public class Player : MonoBehaviour
{
    public enum StateMachine
    {
        idle,
        running,
        //attack,
        falling,
        jumping,
        //ledgeClimbing,
        chainClimbing,
        ropeGrappling,
        sliding,
        dashing,
        //dead
    }
    internal PlayerMovementBase baseMovementScript;
    internal PlayerInventory inventory;
    internal StateMachine currentState;
    private void OnEnable()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
        inventory = GetComponent<PlayerInventory>();
    }
}
