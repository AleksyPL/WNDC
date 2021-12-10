using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSurroundingsChecker : MonoBehaviour
{
    [SerializeField]
    internal PlayerMovementBase baseMovementScript;
    public Transform wallCheck;
    public Transform LedgeCheck;
    public LayerMask whatIsBackground;
    public LayerMask whatIsGround;
    public LayerMask whatIsChain;

    internal bool isFacingRight;
    internal bool isGrounded;
    internal bool isTouchingLedge;
    internal bool isTouchingWall;
    internal bool isTouchingChain;
    internal bool isTouchingCeiling;
    public float wallCheckDistance;
    public float chainCheckDistance;
    public float ceilingCheckDistance;
    void Start()
    {
        isFacingRight = true;
        isGrounded = false;
        isTouchingWall = false;
        isTouchingLedge = false;
        isTouchingChain = false;
        isTouchingCeiling = false;
    }
    internal void CheckSurroundings()
    {
        isGrounded = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 0.1f, whatIsGround);
        isTouchingCeiling = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center + new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.up, 0.1f, whatIsGround);
        isTouchingChain = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 0.1f, whatIsChain);
        isTouchingWall = Physics2D.Raycast(wallCheck.transform.position, transform.right, wallCheckDistance, whatIsGround);
        isTouchingLedge = Physics2D.Raycast(LedgeCheck.transform.position, transform.right, wallCheckDistance, whatIsGround);
        if (isGrounded)
        {
            if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.ledgeClimbing)
            {
                baseMovementScript.myRigidBody.gravityScale = 0;
                baseMovementScript.canMove = false;
                baseMovementScript.canFlip = false;
                baseMovementScript.canGrapple = false;
                baseMovementScript.canDash = false;
            }
            else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.dash)
            {
                baseMovementScript.canJump = false;
                baseMovementScript.canFlip = false;
                baseMovementScript.canGrapple = false;
            }
            else
            {
                baseMovementScript.airMovementScript.ledgeDetected = false;
                baseMovementScript.myRigidBody.gravityScale = 1;
                baseMovementScript.canMove = true;
                baseMovementScript.canFlip = true;
                baseMovementScript.canGrapple = true;
                baseMovementScript.canDash = true;
                if (isTouchingChain)
                {
                    baseMovementScript.canJump = false;
                    baseMovementScript.canClimbChain = true;
                }
                else
                {
                    baseMovementScript.canClimbChain = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 1.1f, whatIsChain);
                    baseMovementScript.canJump = true;
                }
            }
        }
        else if (!isGrounded)
        {
            baseMovementScript.canMove = true;
            baseMovementScript.canJump = false;
            baseMovementScript.canFlip = true;
            baseMovementScript.canGrapple = true;
            baseMovementScript.canDash = true;
            baseMovementScript.myRigidBody.gravityScale = 1;
            if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.ledgeClimbing || baseMovementScript.canClimbLedge)
            {
                baseMovementScript.canMove = false;
                baseMovementScript.canGrapple = false;
                baseMovementScript.canDash = false;
                baseMovementScript.myRigidBody.gravityScale = 0;
                baseMovementScript.myRigidBody.velocity = Vector2.zero;
            }
            else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.chainClimbing)
            {
                baseMovementScript.canGrapple = false;
                baseMovementScript.canDash = false;
                baseMovementScript.myRigidBody.gravityScale = 0;
                baseMovementScript.myRigidBody.velocity = Vector2.zero;
            }
            else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.dash)
            {
                baseMovementScript.canJump = false;
                baseMovementScript.canFlip = false;
                baseMovementScript.canGrapple = false;
            }
            if (isTouchingWall && !isTouchingLedge && !baseMovementScript.airMovementScript.ledgeDetected && !isTouchingChain && !baseMovementScript.airMovementScript.isGrappling)
            {
                //RaycastHit2D boxCastDebug = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center + new Vector3(baseMovementScript.boxCollider.bounds.extents.x, 0), new Vector3(0.1f, baseMovementScript.boxCollider.bounds.extents.y, 0), 0f, Vector2.right, 0.1f, whatIsGround);
                baseMovementScript.canGrapple = false;
                baseMovementScript.canDash = false;
                baseMovementScript.airMovementScript.ledgeDetected = true;
                baseMovementScript.airMovementScript.ledgePositionBottom = wallCheck.position;
                baseMovementScript.airMovementScript.FindLedgeToClimb();
            }
        }
    }
}
