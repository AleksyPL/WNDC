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

    public Camera camera;
    internal Rigidbody2D myRigidBody;
    internal BoxCollider2D boxCollider;
    internal Vector3 position;
    internal Vector3 mousePosition;
    internal bool LPM_hold;
    internal bool RPM_hold;
    internal bool canMove;
    internal bool canFlip;
    internal bool canJump;
    internal bool canGrapple;
    internal bool canClimbLedge;
    internal bool canClimbChain;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        LPM_hold = false;
        RPM_hold = false;
        canFlip = true;
        canMove = false;
        canJump = false;
        canGrapple = false;
        canClimbLedge = false;
        canClimbChain = false;
    }
    void FixedUpdate()
    {
        position.x = Input.GetAxisRaw("Horizontal");
        position.y = Input.GetAxisRaw("Vertical");
        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        LPM_hold = Input.GetButton("Fire1");
        RPM_hold = Input.GetButton("Fire2");
        surroundingsCheckerScript.CheckSurroundings();
        MoveCharacter();
        animatorScript.AnimateCharacter();
    }
    internal void FlipCharacter()
    {
        if (canFlip && (position.x < 0 && surroundingsCheckerScript.isFacingRight || position.x > 0 && !surroundingsCheckerScript.isFacingRight))
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
        position.Normalize();
        if (canMove && surroundingsCheckerScript.isGrounded)
        {
            if (position == Vector3.zero && mainPlayerScript.currentState != Player.StateMachine.ledgeClimbing && mainPlayerScript.currentState != Player.StateMachine.ropeGrappling)
            {
                mainPlayerScript.currentState = Player.StateMachine.idle;
                myRigidBody.velocity = Vector2.zero;
            }
            if (position.x != 0 && position.y == 0)
            {
                mainPlayerScript.currentState = Player.StateMachine.walk;
                myRigidBody.velocity = new Vector2(Mathf.Round(position.x) * groundMovementScript.walkSpeed, myRigidBody.velocity.y);
                FlipCharacter();
            }
            if ((position.y > 0 || Input.GetButton("Jump")) && canJump && !surroundingsCheckerScript.isTouchingChain && !canClimbChain)
            {
                mainPlayerScript.currentState = Player.StateMachine.jump;
                airMovementScript.Jump(myRigidBody.velocity.x, airMovementScript.jumpForce);
            }
            if (position.y < 0 && !surroundingsCheckerScript.isTouchingChain)
            {
                mainPlayerScript.currentState = Player.StateMachine.slide;
                //TODO Slide
            }
            if(position.y != 0 && mainPlayerScript.currentState != Player.StateMachine.chainClimbing && canClimbChain)
            {
                mainPlayerScript.currentState = Player.StateMachine.chainClimbing;
                if(surroundingsCheckerScript.isTouchingChain && !canJump)
                {
                    airMovementScript.AttachToTheChain(0.35f, 0.2f, 0.1f);
                }
                else if (surroundingsCheckerScript.isTouchingActivePlatform && canJump)
                {
                    airMovementScript.AttachToTheChain(0.35f, -(boxCollider.bounds.size.y + 1f - surroundingsCheckerScript.chainCheckDistance + 0.1f), 1.1f);
                }
            }
        }
        else if (canMove && !surroundingsCheckerScript.isGrounded)
        {
            if(mainPlayerScript.currentState != Player.StateMachine.jump && mainPlayerScript.currentState != Player.StateMachine.ledgeClimbing && mainPlayerScript.currentState != Player.StateMachine.chainClimbing && mainPlayerScript.currentState != Player.StateMachine.ropeGrappling)
            {
                airMovementScript.Falling();
            }
            if (mainPlayerScript.currentState != Player.StateMachine.ledgeClimbing && mainPlayerScript.currentState != Player.StateMachine.chainClimbing && (mainPlayerScript.currentState == Player.StateMachine.jump || mainPlayerScript.currentState == Player.StateMachine.fall))
            {
                if (surroundingsCheckerScript.isTouchingChain && position.y != 0)
                {
                    mainPlayerScript.currentState = Player.StateMachine.chainClimbing;
                    airMovementScript.AttachToTheChain(0.35f, 0.2f, 0.1f);
                }
                if (surroundingsCheckerScript.isFacingRight && myRigidBody.velocity.x <= 3f || !surroundingsCheckerScript.isFacingRight && myRigidBody.velocity.x >= -3f)
                {
                    myRigidBody.AddForce(new Vector2(position.x * 2f * airMovementScript.jumpForce, 0));
                }
                if ((surroundingsCheckerScript.isFacingRight && myRigidBody.velocity.x > 3f && position.x < 0) || (position.x > 0 && !surroundingsCheckerScript.isFacingRight && myRigidBody.velocity.x < -3f))
                {
                    myRigidBody.AddForce(new Vector2(position.x * 20f * airMovementScript.jumpForce, 0));
                }
                FlipCharacter();
            }
            if (surroundingsCheckerScript.isTouchingChain && mainPlayerScript.currentState == Player.StateMachine.chainClimbing)
            {
                airMovementScript.ChainClimb();
            }
        }
        if (mainPlayerScript.currentState != Player.StateMachine.ropeGrappling && canGrapple && !airMovementScript.isGrappling && RPM_hold)
        {
            mainPlayerScript.currentState = Player.StateMachine.ropeGrappling;
            airMovementScript.SetGrapplePoint();
            airMovementScript.Grapple();
        }
        else if (mainPlayerScript.currentState == Player.StateMachine.ropeGrappling && !RPM_hold)
        {
            airMovementScript.isGrappling = false;
            airMovementScript.springJoint.enabled = false;
            airMovementScript.grappleRopeScript.enabled = false;
            airMovementScript.Falling();
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(GetComponent<BoxCollider2D>().bounds.center - new Vector3(0, GetComponent<BoxCollider2D>().bounds.extents.y + 0.05f), new Vector3(GetComponent<BoxCollider2D>().bounds.extents.x, 0.1f, 0));
        Gizmos.DrawWireCube(GetComponent<BoxCollider2D>().bounds.center - new Vector3(0, GetComponent<BoxCollider2D>().bounds.extents.y + 0.05f), new Vector3(2 * GetComponent<BoxCollider2D>().bounds.extents.x, 0.1f, 0));
        Gizmos.DrawWireCube(GetComponent<BoxCollider2D>().bounds.center + new Vector3(GetComponent<BoxCollider2D>().bounds.extents.x + 0.05f, 0), new Vector3(0.1f, GetComponent<BoxCollider2D>().bounds.extents.y, 0));
        Gizmos.DrawLine(airMovementScript.baseMovementScript.mousePosition, airMovementScript.grapplingFirePoint.position);
        if (surroundingsCheckerScript.isFacingRight)
        {
            Gizmos.DrawLine(surroundingsCheckerScript.wallCheck.transform.position, new Vector2(surroundingsCheckerScript.wallCheck.transform.position.x + surroundingsCheckerScript.wallCheckDistance, surroundingsCheckerScript.wallCheck.transform.position.y));
            Gizmos.DrawLine(surroundingsCheckerScript.LedgeCheck.transform.position, new Vector2(surroundingsCheckerScript.LedgeCheck.transform.position.x + surroundingsCheckerScript.wallCheckDistance, surroundingsCheckerScript.LedgeCheck.transform.position.y));
        }
        else
        {
            Gizmos.DrawLine(surroundingsCheckerScript.wallCheck.transform.position, new Vector2(surroundingsCheckerScript.wallCheck.transform.position.x - surroundingsCheckerScript.wallCheckDistance, surroundingsCheckerScript.wallCheck.transform.position.y));
            Gizmos.DrawLine(surroundingsCheckerScript.LedgeCheck.transform.position, new Vector2(surroundingsCheckerScript.LedgeCheck.transform.position.x - surroundingsCheckerScript.wallCheckDistance, surroundingsCheckerScript.LedgeCheck.transform.position.y));
        }
    }
}
