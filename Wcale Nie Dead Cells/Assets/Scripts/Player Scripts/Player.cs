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
    // Start is called before the first frame update
    void Start()
    {
        //currentState = StateMachine.idle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
