using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum StateMachine
    {
        idle,
        walk,
        attack,
        fall,
        jump,
        ledgeClimbing,
        chainClimbing,
        ropeGrappling,
        slide,
        dash,
        dead
    }
    [SerializeField]
    internal PlayerMovementBase baseMovementScript;
    [SerializeField]
    internal AbilityCooldowns cooldownScript;
    internal StateMachine currentState;
    void Start()
    {

    }
    void Update()
    {
        Debug.Log(currentState);
    }
}
