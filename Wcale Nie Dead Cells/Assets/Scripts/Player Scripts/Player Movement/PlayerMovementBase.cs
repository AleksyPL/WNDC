using System.Collections;
using UnityEngine;


//[RequireComponent(typeof(Player))]
//[RequireComponent(typeof(PlayerMovementAir))]
//[RequireComponent(typeof(PlayerMovementGround))]
//[RequireComponent(typeof(PlayerMovementSurroundingsChecker))]
//[RequireComponent(typeof(PlayerMovementAnimator))]
//[RequireComponent(typeof(PlayerMovementInputManager))]
//[RequireComponent(typeof(PlayerMovementShooting))]
public class PlayerMovementBase : MonoBehaviour
{
    internal Player mainPlayerScript;
    internal PlayerMovementAir airMovementScript;
    internal PlayerMovementGround groundMovementScript;
    internal PlayerMovementSurroundingsChecker surroundingsCheckerScript;
    internal PlayerMovementAnimator animatorScript;
    internal PlayerMovementInputManager inputScript;
    internal PlayerMovementShooting shootingScript;
    internal PlayerNewShooting shootingScript1;
    internal Rigidbody2D myRigidBody;
    internal BoxCollider2D boxCollider;
    internal bool canMoveSideways;
    internal bool canMoveUpAndDown;
    internal bool canJump;
    internal bool canGrapple;
    //internal bool canDash;
    internal bool canClimbLedge;
    internal bool canClimbChain;
    internal bool canHoldGun;
    internal bool canShoot;

    void OnEnable()
    {
        mainPlayerScript = GetComponent<Player>();
        airMovementScript = GetComponent<PlayerMovementAir>();
        groundMovementScript = GetComponent<PlayerMovementGround>();
        surroundingsCheckerScript = GetComponent<PlayerMovementSurroundingsChecker>();
        animatorScript = GetComponent<PlayerMovementAnimator>();
        inputScript = GetComponent<PlayerMovementInputManager>();
        shootingScript = GetComponent<PlayerMovementShooting>();
        shootingScript1 = GetComponent<PlayerNewShooting>();
        myRigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        canMoveSideways = false;
        canJump = false;
        canGrapple = false;
        //canDash = false;
        canClimbLedge = false;
        canClimbChain = false;
        canHoldGun = true;
        canShoot = false;
    }
    void Update()
    {
        inputScript.GetInputs();
        surroundingsCheckerScript.CheckSurroundings();
        MoveCharacter();
        inputScript.FlipCharacter();
        animatorScript.AnimateCharacter();
    }
    private void MoveCharacter()
    {
        if(canMoveSideways || canMoveUpAndDown)
        {
            if (mainPlayerScript.currentState != Player.StateMachine.ropeGrappling && canGrapple && !airMovementScript.isGrappling && inputScript.RMB_hold)
            {
                airMovementScript.SetGrapplePoint();
            }
            else if (airMovementScript.isGrappling && !inputScript.RMB_hold)
            {
                airMovementScript.DetachGrapplingHook();
                airMovementScript.Falling();
            }
            else if (inputScript.spacePressed)
            {
                airMovementScript.Jump();
            }
            else if (inputScript.scrollwheel != 0)
            {
                shootingScript1.ChangeActiveWeapon();
            }
            else if (surroundingsCheckerScript.isGrounded)
            {
                if (inputScript.position == Vector3.zero && mainPlayerScript.currentState != Player.StateMachine.ropeGrappling)
                {
                    mainPlayerScript.currentState = Player.StateMachine.idle;
                    myRigidBody.velocity = Vector2.zero;
                }
                if (inputScript.position.x != 0 && mainPlayerScript.currentState != Player.StateMachine.dashing && !groundMovementScript.isDashing)
                {
                    groundMovementScript.Run();
                }
                if (mainPlayerScript.currentState != Player.StateMachine.chainClimbing && canClimbChain)
                {
                    if (inputScript.position.y > 0 && surroundingsCheckerScript.isTouchingChain)
                    {
                        airMovementScript.AttachToTheChain(0.35f, 0.2f, 0.1f);
                        mainPlayerScript.currentState = Player.StateMachine.chainClimbing;
                    }
                    else if (inputScript.position.y < 0 && !surroundingsCheckerScript.isTouchingChain)
                    {
                        airMovementScript.AttachToTheChain(0.35f, -(boxCollider.bounds.size.y + 1f - surroundingsCheckerScript.chainCheckDistance + 0.1f), 1.1f);
                        mainPlayerScript.currentState = Player.StateMachine.chainClimbing;
                    }
                }
            }
            else if (!surroundingsCheckerScript.isGrounded)
            {
                if (mainPlayerScript.currentState != Player.StateMachine.jumping && mainPlayerScript.currentState != Player.StateMachine.chainClimbing && mainPlayerScript.currentState != Player.StateMachine.ropeGrappling && mainPlayerScript.currentState != Player.StateMachine.dashing)
                {
                    airMovementScript.Falling();
                }
                if (surroundingsCheckerScript.isTouchingChain && mainPlayerScript.currentState == Player.StateMachine.chainClimbing)
                {
                    airMovementScript.ChainClimb();
                }
            }
        }
        //if(canMove)
        //
        //    else if (inputScript.shiftHold && canDash && groundMovementScript.dashReady && mainPlayerScript.currentState != Player.StateMachine.ropeGrappling && mainPlayerScript.currentState != Player.StateMachine.chainClimbing && mainPlayerScript.currentState != Player.StateMachine.ledgeClimbing)
        //    {
        //        groundMovementScript.Dash();
        //    }
        //    else if (surroundingsCheckerScript.isGrounded)
        //    {
        //        //if (position.y < 0 && !surroundingsCheckerScript.isTouchingChain)
        //        //{
        //        //    mainPlayerScript.currentState = Player.StateMachine.slide;
        //        //    //TODO Slide
        //        //}
        //        if (mainPlayerScript.currentState != Player.StateMachine.chainClimbing && canClimbChain)
        //        {
        //            if (inputScript.position.y > 0 && surroundingsCheckerScript.isTouchingChain && !canJump)
        //            {
        //                airMovementScript.AttachToTheChain(0.35f, 0.2f, 0.1f);
        //                mainPlayerScript.currentState = Player.StateMachine.chainClimbing;
        //            }
        //            else if (inputScript.position.y < 0 && !surroundingsCheckerScript.isTouchingChain && canJump)
        //            {
        //                airMovementScript.AttachToTheChain(0.35f, -(boxCollider.bounds.size.y + 1f - surroundingsCheckerScript.chainCheckDistance + 0.1f), 1.1f);
        //                mainPlayerScript.currentState = Player.StateMachine.chainClimbing;
        //            }
        //        }
        //    }
        //    else if (!surroundingsCheckerScript.isGrounded)
        //    {
        //        if (mainPlayerScript.currentState != Player.StateMachine.ledgeClimbing && mainPlayerScript.currentState != Player.StateMachine.chainClimbing && (mainPlayerScript.currentState == Player.StateMachine.jump || mainPlayerScript.currentState == Player.StateMachine.fall))
        //        {
        //            if (surroundingsCheckerScript.isTouchingChain && inputScript.position.y != 0)
        //            {
        //                mainPlayerScript.currentState = Player.StateMachine.chainClimbing;
        //                airMovementScript.AttachToTheChain(0.35f, 0.2f, 0.1f);
        //            }
        //            if (surroundingsCheckerScript.isFacingRight && myRigidBody.velocity.x <= 3f || !surroundingsCheckerScript.isFacingRight && myRigidBody.velocity.x >= -3f)
        //            {
        //                myRigidBody.AddForce(new Vector2(inputScript.position.x * 2f * airMovementScript.jumpForce, 0));
        //            }
        //            if ((surroundingsCheckerScript.isFacingRight && myRigidBody.velocity.x > 3f && inputScript.position.x < 0) || (inputScript.position.x > 0 && !surroundingsCheckerScript.isFacingRight && myRigidBody.velocity.x < -3f))
        //            {
        //                myRigidBody.AddForce(new Vector2(inputScript.position.x * 20f * airMovementScript.jumpForce, 0));
        //            }
        //            //mainPlayerScript.FlipCharacter();
        //        }
        //        if (surroundingsCheckerScript.isTouchingChain && mainPlayerScript.currentState == Player.StateMachine.chainClimbing)
        //        {
        //            airMovementScript.ChainClimb();
        //        }
        //    }
        //}
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(GetComponent<BoxCollider2D>().bounds.center + new Vector3(0, GetComponent<BoxCollider2D>().bounds.extents.y + 0.05f), new Vector3(2 * GetComponent<BoxCollider2D>().bounds.extents.x, 0.1f, 0));
        Gizmos.DrawWireCube(GetComponent<BoxCollider2D>().bounds.center - new Vector3(0, GetComponent<BoxCollider2D>().bounds.extents.y + 0.05f), new Vector3(2 * GetComponent<BoxCollider2D>().bounds.extents.x, 0.1f, 0));
        Gizmos.DrawWireCube(GetComponent<BoxCollider2D>().bounds.center + new Vector3(GetComponent<BoxCollider2D>().bounds.extents.x + 0.05f, 0), new Vector3(0.1f, 2 * GetComponent<BoxCollider2D>().bounds.extents.y, 0));
        Gizmos.DrawWireCube(GetComponent<BoxCollider2D>().bounds.center - new Vector3(GetComponent<BoxCollider2D>().bounds.extents.x + 0.05f, 0), new Vector3(0.1f, 2 * GetComponent<BoxCollider2D>().bounds.extents.y, 0));
    }
}
