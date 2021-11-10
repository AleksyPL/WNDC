using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSurroundingsChecker : MonoBehaviour
{
    [SerializeField]
    internal PlayerMovementBase baseMovementScript;

    //public Transform groundCheck;
    public Transform wallCheck;
    public Transform LedgeCheck;
    public LayerMask whatIsBackground;
    public LayerMask whatIsGround;
    public LayerMask whatIsChain;
    public LayerMask whatIsOneSidePlatform;
    internal bool isFacingRight;
    internal bool isGrounded;
    internal bool isTouchingLedge;
    internal bool isTouchingWall;
    internal bool isTouchingChain;
    internal bool isTouchingActivePlatform;
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
        isTouchingActivePlatform = false;
    }

    void Update()
    {
        
    }
    internal void CheckSurroundings()
    {
        isGrounded = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 0.1f, whatIsGround);
        if (!isGrounded)
        {
            isGrounded = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 0.1f, whatIsOneSidePlatform);
        }
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
            }
            else
            {
                isTouchingActivePlatform = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 0.1f, whatIsOneSidePlatform);
                baseMovementScript.airMovementScript.ledgeDetected = false;
                baseMovementScript.myRigidBody.gravityScale = 1;
                baseMovementScript.canMove = true;
                baseMovementScript.canFlip = true;
                baseMovementScript.canGrapple = true;
                if (isTouchingChain)
                {
                    baseMovementScript.canJump = false;
                    baseMovementScript.canClimbChain = true;
                }
                else if (isTouchingActivePlatform)
                {
                    baseMovementScript.canClimbChain = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 1.1f, whatIsChain);
                    baseMovementScript.canJump = true;
                }
                else
                {
                    baseMovementScript.canJump = true;
                    baseMovementScript.canClimbChain = false;
                }
            }
        }
        else if (!isGrounded)
        {
            baseMovementScript.canMove = true;
            baseMovementScript.canJump = false;
            baseMovementScript.canFlip = true;
            baseMovementScript.canGrapple = true;
            baseMovementScript.myRigidBody.gravityScale = 1;
            if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.ledgeClimbing || baseMovementScript.canClimbLedge)
            {
                baseMovementScript.canMove = false;
                baseMovementScript.canGrapple = false;
                baseMovementScript.myRigidBody.gravityScale = 0;
                baseMovementScript.myRigidBody.velocity = Vector2.zero;
            }
            else if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.chainClimbing)
            {
                baseMovementScript.canGrapple = false;
                baseMovementScript.myRigidBody.gravityScale = 0;
                baseMovementScript.myRigidBody.velocity = Vector2.zero;
                isTouchingActivePlatform = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center + new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.up, 0.1f, whatIsOneSidePlatform);
            }
            if (isTouchingWall && !isTouchingLedge && !baseMovementScript.airMovementScript.ledgeDetected && !isTouchingChain)
            {
                //RaycastHit2D boxCastDebug = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center + new Vector3(baseMovementScript.boxCollider.bounds.extents.x, 0), new Vector3(0.1f, baseMovementScript.boxCollider.bounds.extents.y, 0), 0f, Vector2.right, 0.1f, whatIsGround);
                baseMovementScript.canGrapple = false;
                baseMovementScript.airMovementScript.ledgeDetected = true;
                baseMovementScript.airMovementScript.ledgePositionBottom = wallCheck.position;
                baseMovementScript.airMovementScript.FindLedgeToClimb();
            }
        }
    }
}
