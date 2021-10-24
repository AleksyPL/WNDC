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
        slide,
        dead
    }
    [SerializeField]
    internal PlayerMovement movement;
    internal StateMachine currentState;
    public float walkSpeed;
    public float jumpForce;
    public float slideForce;
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
