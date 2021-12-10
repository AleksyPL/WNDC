using System.Collections;
using UnityEngine;

public class PlayerMovementBase : MonoBehaviour
{
    [SerializeField]
    internal Player mainPlayerScript;
    [SerializeField]
    internal PlayerMovementAir airMovementScript;
    [SerializeField]
    internal PlayerMovementGround groundMovementScript;
    [SerializeField]
    internal PlayerMovementSurroundingsChecker surroundingsCheckerScript;
    [SerializeField]
    internal PlayerMovementAnimator animatorScript;
    [SerializeField]
    internal PlayerMovementInputManager inputScript;

    internal Rigidbody2D myRigidBody;
    internal BoxCollider2D boxCollider;
    internal bool canMove;
    internal bool canFlip;
    internal bool canJump;
    internal bool canGrapple;
    internal bool canDash;
    internal bool canClimbLedge;
    internal bool canClimbChain;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        canFlip = true;
        canMove = false;
        canJump = false;
        canGrapple = false;
        canDash = false;
        canClimbLedge = false;
        canClimbChain = false;
    }
    void Update()
    {
        inputScript.GetInputs();
        surroundingsCheckerScript.CheckSurroundings();
        MoveCharacter();
        animatorScript.AnimateCharacter();
    }
    internal void FlipCharacter()
    {
        if (canFlip && (inputScript.position.x < 0 && surroundingsCheckerScript.isFacingRight || inputScript.position.x > 0 && !surroundingsCheckerScript.isFacingRight))
        {
            surroundingsCheckerScript.isFacingRight = !surroundingsCheckerScript.isFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));
            if (mainPlayerScript.currentState == Player.StateMachine.chainClimbing)
            {
                RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector3(2 * boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.zero, 0, surroundingsCheckerScript.whatIsChain);
                if (surroundingsCheckerScript.isFacingRight)
                {
                    transform.position = new Vector2(hit.collider.bounds.center.x - 0.35f, transform.position.y);
                }
                else
                {
                    transform.position = new Vector2(hit.collider.bounds.center.x + 0.35f, transform.position.y);
                }
            }
        }
        else if(canFlip && mainPlayerScript.currentState == Player.StateMachine.ropeGrappling)
        {
            if(airMovementScript.grapplingFirePoint.transform.position.x > airMovementScript.grapplePoint.x && surroundingsCheckerScript.isFacingRight || airMovementScript.grapplingFirePoint.transform.position.x < airMovementScript.grapplePoint.x && !surroundingsCheckerScript.isFacingRight)
            {
                surroundingsCheckerScript.isFacingRight = !surroundingsCheckerScript.isFacingRight;
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }
    }
    private void MoveCharacter()
    {
        if(canMove)
        {
            if (mainPlayerScript.currentState != Player.StateMachine.ropeGrappling && canGrapple && !airMovementScript.isGrappling && inputScript.RPM_hold)
            {
                airMovementScript.SetGrapplePoint();
            }
            else if (mainPlayerScript.currentState == Player.StateMachine.ropeGrappling && !inputScript.RPM_hold)
            {
                airMovementScript.DetachGrapplingHook();
                airMovementScript.Falling();
            }
            else if (inputScript.shiftHold && canDash && groundMovementScript.dashReady && mainPlayerScript.currentState != Player.StateMachine.ropeGrappling && mainPlayerScript.currentState != Player.StateMachine.chainClimbing && mainPlayerScript.currentState != Player.StateMachine.ledgeClimbing)
            {
                groundMovementScript.Dash();
            }
            else if (surroundingsCheckerScript.isGrounded)
            {
                if (inputScript.position == Vector3.zero && mainPlayerScript.currentState != Player.StateMachine.ledgeClimbing && mainPlayerScript.currentState != Player.StateMachine.ropeGrappling)
                {
                    mainPlayerScript.currentState = Player.StateMachine.idle;
                    myRigidBody.velocity = Vector2.zero;
                }
                if (inputScript.position.x != 0 && mainPlayerScript.currentState != Player.StateMachine.dash && !groundMovementScript.isDashing)
                {
                    groundMovementScript.Run();
                }
                if (Input.GetButton("Jump") && canJump && !surroundingsCheckerScript.isTouchingChain)
                {
                    mainPlayerScript.currentState = Player.StateMachine.jump;
                    airMovementScript.Jump(myRigidBody.velocity.x, airMovementScript.jumpForce);
                }
                //if (position.y < 0 && !surroundingsCheckerScript.isTouchingChain)
                //{
                //    mainPlayerScript.currentState = Player.StateMachine.slide;
                //    //TODO Slideasd
                //}
                if (mainPlayerScript.currentState != Player.StateMachine.chainClimbing && canClimbChain)
                {
                    if (inputScript.position.y > 0 && surroundingsCheckerScript.isTouchingChain && !canJump)
                    {
                        airMovementScript.AttachToTheChain(0.35f, 0.2f, 0.1f);
                        mainPlayerScript.currentState = Player.StateMachine.chainClimbing;
                    }
                    else if (inputScript.position.y < 0 && !surroundingsCheckerScript.isTouchingChain && canJump)
                    {
                        airMovementScript.AttachToTheChain(0.35f, -(boxCollider.bounds.size.y + 1f - surroundingsCheckerScript.chainCheckDistance + 0.1f), 1.1f);
                        mainPlayerScript.currentState = Player.StateMachine.chainClimbing;
                    }
                }
            }
            else if (!surroundingsCheckerScript.isGrounded)
            {
                if (mainPlayerScript.currentState != Player.StateMachine.jump && mainPlayerScript.currentState != Player.StateMachine.ledgeClimbing && mainPlayerScript.currentState != Player.StateMachine.chainClimbing && mainPlayerScript.currentState != Player.StateMachine.ropeGrappling && mainPlayerScript.currentState != Player.StateMachine.dash)
                {
                    airMovementScript.Falling();
                }
                if (mainPlayerScript.currentState != Player.StateMachine.ledgeClimbing && mainPlayerScript.currentState != Player.StateMachine.chainClimbing && (mainPlayerScript.currentState == Player.StateMachine.jump || mainPlayerScript.currentState == Player.StateMachine.fall))
                {
                    if (surroundingsCheckerScript.isTouchingChain && inputScript.position.y != 0)
                    {
                        mainPlayerScript.currentState = Player.StateMachine.chainClimbing;
                        airMovementScript.AttachToTheChain(0.35f, 0.2f, 0.1f);
                    }
                    if (surroundingsCheckerScript.isFacingRight && myRigidBody.velocity.x <= 3f || !surroundingsCheckerScript.isFacingRight && myRigidBody.velocity.x >= -3f)
                    {
                        myRigidBody.AddForce(new Vector2(inputScript.position.x * 2f * airMovementScript.jumpForce, 0));
                    }
                    if ((surroundingsCheckerScript.isFacingRight && myRigidBody.velocity.x > 3f && inputScript.position.x < 0) || (inputScript.position.x > 0 && !surroundingsCheckerScript.isFacingRight && myRigidBody.velocity.x < -3f))
                    {
                        myRigidBody.AddForce(new Vector2(inputScript.position.x * 20f * airMovementScript.jumpForce, 0));
                    }
                    FlipCharacter();
                }
                if (surroundingsCheckerScript.isTouchingChain && mainPlayerScript.currentState == Player.StateMachine.chainClimbing)
                {
                    airMovementScript.ChainClimb();
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(GetComponent<BoxCollider2D>().bounds.center + new Vector3(0, GetComponent<BoxCollider2D>().bounds.extents.y + 0.05f), new Vector3(2 * GetComponent<BoxCollider2D>().bounds.extents.x, 0.1f, 0));
        Gizmos.DrawWireCube(GetComponent<BoxCollider2D>().bounds.center - new Vector3(0, GetComponent<BoxCollider2D>().bounds.extents.y + 0.05f), new Vector3(2 * GetComponent<BoxCollider2D>().bounds.extents.x, 0.1f, 0));
        if (surroundingsCheckerScript.isFacingRight)
        {
            Gizmos.DrawLine(surroundingsCheckerScript.wallCheck.transform.position, new Vector2(surroundingsCheckerScript.wallCheck.transform.position.x + surroundingsCheckerScript.wallCheckDistance, surroundingsCheckerScript.wallCheck.transform.position.y));
            Gizmos.DrawLine(surroundingsCheckerScript.LedgeCheck.transform.position, new Vector2(surroundingsCheckerScript.LedgeCheck.transform.position.x + surroundingsCheckerScript.wallCheckDistance, surroundingsCheckerScript.LedgeCheck.transform.position.y));
            Gizmos.DrawWireCube(GetComponent<BoxCollider2D>().bounds.center + new Vector3(GetComponent<BoxCollider2D>().bounds.extents.x + 0.05f, 0), new Vector3(0.1f, GetComponent<BoxCollider2D>().bounds.extents.y, 0));
        }
        else
        {
            Gizmos.DrawLine(surroundingsCheckerScript.wallCheck.transform.position, new Vector2(surroundingsCheckerScript.wallCheck.transform.position.x - surroundingsCheckerScript.wallCheckDistance, surroundingsCheckerScript.wallCheck.transform.position.y));
            Gizmos.DrawLine(surroundingsCheckerScript.LedgeCheck.transform.position, new Vector2(surroundingsCheckerScript.LedgeCheck.transform.position.x - surroundingsCheckerScript.wallCheckDistance, surroundingsCheckerScript.LedgeCheck.transform.position.y));
            Gizmos.DrawWireCube(GetComponent<BoxCollider2D>().bounds.center - new Vector3(GetComponent<BoxCollider2D>().bounds.extents.x + 0.05f, 0), new Vector3(0.1f, GetComponent<BoxCollider2D>().bounds.extents.y, 0));
        }
    }
}
