using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementBase))]
[RequireComponent(typeof(PlayerMovementGrapplingRope))]
[RequireComponent(typeof(SpringJoint2D))]
public class PlayerMovementAir : MonoBehaviour
{
    
    private PlayerMovementBase baseMovementScript;
    private PlayerMovementGrapplingRope grappleRopeScript;
    private SpringJoint2D springJoint;
    internal bool ledgeDetected;
    internal bool isGrappling;
    internal Vector2 grapplePoint;
    public float airControlSpeedLimit;
    public float airControlSpeed;
    public float jumpForce;
    public Transform grapplingFirePoint;
    public LayerMask grapplableLayers;
    public float grappleDistance;
    readonly internal int numberOfJumps = 2;
    internal int numberOfAvailableJumps;

    void Start()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
        grappleRopeScript = GetComponent<PlayerMovementGrapplingRope>();
        springJoint = GetComponent<SpringJoint2D>();
        grappleRopeScript.enabled = false;
        isGrappling = false;
        numberOfAvailableJumps = numberOfJumps;
    }

    void Update()
    {
        LimitAirSpeed();   
    }
    internal void LimitAirSpeed()
    {
        if (!baseMovementScript.surroundingsCheckerScript.isGrounded)
        {
            if (baseMovementScript.myRigidBody.velocity.x > airControlSpeedLimit)
            {
                baseMovementScript.myRigidBody.velocity = new Vector2(airControlSpeedLimit, baseMovementScript.myRigidBody.velocity.y);
            }
            else if (baseMovementScript.myRigidBody.velocity.x < -airControlSpeedLimit)
            {
                baseMovementScript.myRigidBody.velocity = new Vector2(-airControlSpeedLimit, baseMovementScript.myRigidBody.velocity.y);
            }
            if (baseMovementScript.myRigidBody.velocity.y > airControlSpeedLimit)
            {
                baseMovementScript.myRigidBody.velocity = new Vector2(baseMovementScript.myRigidBody.velocity.x, airControlSpeedLimit);
            }
            else if (baseMovementScript.myRigidBody.velocity.y < -airControlSpeedLimit)
            {
                baseMovementScript.myRigidBody.velocity = new Vector2(baseMovementScript.myRigidBody.velocity.x, -airControlSpeedLimit);
            }
        }
    }
    internal void ControlCharacterInAir()
    {
        if (!baseMovementScript.surroundingsCheckerScript.isTouchingWall && baseMovementScript.mainPlayerScript.currentState != Player.StateMachine.ropeGrappling)
        {
            if (baseMovementScript.surroundingsCheckerScript.isFacingRight && baseMovementScript.myRigidBody.velocity.x <= 3f || !baseMovementScript.surroundingsCheckerScript.isFacingRight && baseMovementScript.myRigidBody.velocity.x >= -3f)
            {
                baseMovementScript.myRigidBody.AddForce(new Vector2(baseMovementScript.inputScript.position.x * 2f * airControlSpeed, 0));
            }
            if ((baseMovementScript.surroundingsCheckerScript.isFacingRight && baseMovementScript.myRigidBody.velocity.x > 3f && baseMovementScript.inputScript.position.x < 0) || (baseMovementScript.inputScript.position.x > 0 && !baseMovementScript.surroundingsCheckerScript.isFacingRight && baseMovementScript.myRigidBody.velocity.x < -3f))
            {
                baseMovementScript.myRigidBody.AddForce(new Vector2(baseMovementScript.inputScript.position.x * 20f * airControlSpeed, 0));
            }
        }
    }
    internal void Falling()
    {
        baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.falling;
    }
    internal void Jump(float vectorX, float vectorY)
    {
        baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.jumping;
        baseMovementScript.myRigidBody.AddForce(new Vector2(vectorX, vectorY));
        //baseMovementScript.myRigidBody.velocity = new Vector2(vectorX, vectorY);
    }
    internal void ChainClimb()
    {
        //if (Input.GetButton("Jump"))
        //{
        //    baseMovementScript.animatorScript.animator.speed = 1;
        //    baseMovementScript.surroundingsCheckerScript.isTouchingChain = false;
        //    if (baseMovementScript.inputScript.position.x != 0)
        //    {
        //        Jump(baseMovementScript.inputScript.position.x * jumpForce * 0.75f, jumpForce / 2);
        //    }
        //    else
        //    {
        //        baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.fall;
        //    }
        //}
        //else if (baseMovementScript.inputScript.position.x != 0)
        //{
        //    //baseMovementScript.mainPlayerScript.FlipCharacter();
        //}
        //else if (baseMovementScript.inputScript.position.y == 0)
        //{
        //    baseMovementScript.animatorScript.animator.speed = 0;
        //}
        //else
        //{
        //    baseMovementScript.animatorScript.animator.speed = 1;
        //    transform.position = new Vector2(transform.position.x, transform.position.y + (baseMovementScript.inputScript.position.y * 0.06f));
        //    if (baseMovementScript.surroundingsCheckerScript.isTouchingCeiling && baseMovementScript.inputScript.position.y > 0)
        //    {
        //        float chainOffsetY = baseMovementScript.boxCollider.bounds.extents.y + 1f + baseMovementScript.surroundingsCheckerScript.chainCheckDistance + 0.2f;
        //        RaycastHit2D hit = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center + new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.up, chainOffsetY);
        //        if (hit.collider.gameObject.layer != baseMovementScript.surroundingsCheckerScript.whatIsGround)
        //        {
        //            transform.position = new Vector2(transform.position.x, transform.position.y + chainOffsetY);
        //            baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.idle;
        //        }
        //    }
        //}
    }
    public void AttachToTheChain(float chainOffsetX, float chainOffsetY, float raycastOffsetY)
    {
        //RaycastHit2D hit = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, raycastOffsetY, baseMovementScript.surroundingsCheckerScript.whatIsChain);
        //if(hit.collider !=null)
        //{
        //    if (baseMovementScript.surroundingsCheckerScript.isFacingRight)
        //    {
        //        transform.position = new Vector2(hit.collider.bounds.center.x - chainOffsetX, transform.position.y + chainOffsetY);
        //    }
        //    else
        //    {
        //        transform.position = new Vector2(hit.collider.bounds.center.x + chainOffsetX, transform.position.y + chainOffsetY);
        //    }
        //}
    }
    internal void SetGrapplePoint()
    {
        Vector3 distanceVector = baseMovementScript.inputScript.mousePosition - grapplingFirePoint.position;
        RaycastHit2D hit = Physics2D.Raycast(grapplingFirePoint.position, distanceVector.normalized, Mathf.Infinity, grapplableLayers);
        if (hit.collider != null)
        {
            grapplePoint = hit.point;
            isGrappling = true;
            grappleRopeScript.enabled = true;
            Grapple();
        }
    }
    internal void Grapple()
    {
        baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.ropeGrappling;
        springJoint.autoConfigureDistance = false;
        springJoint.connectedAnchor = grapplePoint;
        springJoint.distance = 0.2f;
        springJoint.frequency = 1f;
        springJoint.enabled = true;
    }
    internal void DetachGrapplingHook()
    {
        isGrappling = false;
        springJoint.enabled = false;
        grappleRopeScript.enabled = false;
    }
}
