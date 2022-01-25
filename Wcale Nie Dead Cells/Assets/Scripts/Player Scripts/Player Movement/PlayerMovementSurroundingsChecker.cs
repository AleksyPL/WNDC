using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(PlayerMovementBase))]
public class PlayerMovementSurroundingsChecker : MonoBehaviour
{
    internal PlayerMovementBase baseMovementScript;
    public LayerMask whatIsBackground;
    public LayerMask whatIsGround;
    public LayerMask whatIsChain;
    public LayerMask whatIsElevator;
    internal bool isFacingRight;
    internal bool isGrounded;
    internal bool isTouchingWallRight;
    internal bool isTouchingWallLeft;
    internal bool isTouchingChain;
    internal bool isTouchingCeiling;
    internal bool isTouchingElevator;
    public float wallCheckDistance;
    public float chainCheckDistance;
    public float ceilingCheckDistance;
    private float coyoteTime;
    private float coyoteTimeTimer;
    void Start()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
        isFacingRight = true;
        isGrounded = false;
        isTouchingWallRight = false;
        isTouchingWallLeft = false;
        isTouchingChain = false;
        isTouchingCeiling = false;
        coyoteTime = 0.2f;
        coyoteTimeTimer = 0;
    }
    internal void CheckSurroundings()
    {
        isGrounded = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.01f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 0.01f, whatIsGround);
        isTouchingCeiling = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center + new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.up, 0.1f, whatIsGround);
        isTouchingChain = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 0.1f, whatIsChain);
        isTouchingWallLeft = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(baseMovementScript.boxCollider.bounds.extents.x - 0.1f,0), new Vector3(0.1f, 2 * baseMovementScript.boxCollider.bounds.extents.y,0), 0f, Vector2.left, 0.1f, whatIsGround);
        isTouchingWallRight = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center + new Vector3(baseMovementScript.boxCollider.bounds.extents.x - 0.1f, 0), new Vector3(0.1f, 2 * baseMovementScript.boxCollider.bounds.extents.y), 0f, Vector2.right, 0.1f, whatIsGround);
        isTouchingElevator = Physics2D.Raycast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y), Vector2.down, 0.1f, whatIsElevator);
        if (isGrounded)
        {

            baseMovementScript.airMovementScript.ledgeDetected = false;
            baseMovementScript.canMoveSideways = true;
            baseMovementScript.canGrapple = true;
            baseMovementScript.canMoveUpAndDown = true;
            coyoteTimeTimer = 0;
            baseMovementScript.canJump = true;
            baseMovementScript.myRigidBody.gravityScale = 1;
            if (isTouchingChain)
            {
                baseMovementScript.canClimbChain = true;
            }
            else
            {
                baseMovementScript.canClimbChain = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 1.1f, whatIsChain);
            }
        }
        else if (!isGrounded)
        {
            if (coyoteTimeTimer < coyoteTime)
            {
                coyoteTimeTimer += Time.deltaTime;
                baseMovementScript.canJump = true;
            }
            else
            {
                baseMovementScript.canJump = false;
            }
            baseMovementScript.canMoveSideways = true;
            baseMovementScript.canGrapple = true;
            baseMovementScript.myRigidBody.gravityScale = 1;
            baseMovementScript.canMoveUpAndDown = true;
            if (baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.chainClimbing)
            {
                baseMovementScript.canMoveSideways = false;
                baseMovementScript.canGrapple = false;
                baseMovementScript.myRigidBody.gravityScale = 0;
                baseMovementScript.myRigidBody.velocity = Vector2.zero;
            }
        }
    }
}
