using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementBase))]
public class PlayerMovementSurroundingsChecker : MonoBehaviour
{
    internal PlayerMovementBase baseMovementScript;
    public LayerMask whatIsBackground;
    public LayerMask whatIsGround;
    public LayerMask whatIsChain;

    internal bool isFacingRight;
    internal bool isGrounded;
    internal bool isTouchingWall;
    internal bool isTouchingChain;
    internal bool isTouchingCeiling;
    public float wallCheckDistance;
    public float chainCheckDistance;
    public float ceilingCheckDistance;
    void Start()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
        isFacingRight = true;
        isGrounded = false;
        isTouchingWall = false;
        isTouchingChain = false;
        isTouchingCeiling = false;
    }
    internal void CheckSurroundings()
    {
        isGrounded = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 0.1f, whatIsGround);
        isTouchingCeiling = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center + new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.up, 0.1f, whatIsGround);
        isTouchingChain = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 0.1f, whatIsChain);
        isTouchingWall = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center + new Vector3(baseMovementScript.boxCollider.bounds.extents.x - 0.1f, 0), new Vector3(0.1f, 2 * baseMovementScript.boxCollider.bounds.extents.y, 0), 0f, Vector2.right, 0.1f, whatIsGround);
        if(!isTouchingWall)
        {
            isTouchingWall = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(baseMovementScript.boxCollider.bounds.extents.x - 0.1f, 0), new Vector3(0.1f, 2 * baseMovementScript.boxCollider.bounds.extents.y, 0), 0f, Vector2.left, 0.1f, whatIsGround);
        }
        if (isGrounded)
        {
            //else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.dash)
            //{
            //    baseMovementScript.canJump = false;
            //    baseMovementScript.canFlip = false;
            //    baseMovementScript.canGrapple = false;
            //}
            //else
            //{
                baseMovementScript.airMovementScript.ledgeDetected = false;
                baseMovementScript.myRigidBody.gravityScale = 1;
                baseMovementScript.canMove = true;
                baseMovementScript.canFlip = true;
                baseMovementScript.canGrapple = true;
                baseMovementScript.canDash = true;
                baseMovementScript.canJump = true;
                if(baseMovementScript.airMovementScript.numberOfAvailableJumps != baseMovementScript.airMovementScript.numberOfJumps)
                {
                    baseMovementScript.airMovementScript.numberOfAvailableJumps = baseMovementScript.airMovementScript.numberOfJumps;
                }
                if (isTouchingChain)
                {
                    baseMovementScript.canJump = false;
                    baseMovementScript.canClimbChain = true;
                }
                else
                {
                    baseMovementScript.canClimbChain = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 1.1f, whatIsChain);
                }
            //}
        }
        else if (!isGrounded)
        {
            baseMovementScript.canMove = true;
            //baseMovementScript.canJump = false;
            baseMovementScript.canFlip = true;
            baseMovementScript.canGrapple = true;
            baseMovementScript.canDash = true;
            baseMovementScript.myRigidBody.gravityScale = 1;
            //if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.chainClimbing)
            //{
            //    baseMovementScript.canGrapple = false;
            //    baseMovementScript.canDash = false;
            //    baseMovementScript.myRigidBody.gravityScale = 0;
            //    baseMovementScript.myRigidBody.velocity = Vector2.zero;
            //}
            //else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.dashing)
            //{
            //    baseMovementScript.canJump = false;
            //    baseMovementScript.canFlip = false;
            //    baseMovementScript.canGrapple = false;
            //}
        }
    }
}
