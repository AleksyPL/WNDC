using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAir : MonoBehaviour
{
    [SerializeField]
    internal PlayerMovementBase baseMovementScript;
    [SerializeField]
    internal PlayerMovementGrapplingRope grappleRopeScript;

    internal bool ledgeDetected;
    internal bool isGrappling;
    internal Vector2 ledgePositionBottom;
    private Vector2 ledgePosition1;
    private Vector2 ledgePosition2;
    internal Vector2 grapplePoint;
    private readonly float ledgeClimbXOffset1 = 0.245f;
    private readonly float ledgeClimbXOffset2 = 0.35f;
    private readonly float ledgeClimbYOffset1 = 0.23f;
    private readonly float ledgeClimbYOffset2 = 1.7f;
    public float jumpForce;
    public SpringJoint2D springJoint;
    public Transform grapplingFirePoint;
    public LayerMask grapplableLayers;
    public float grappleDistance;
    
    
    void Start()
    {
        springJoint = GetComponent<SpringJoint2D>();
        isGrappling = false;
    }

    void Update()
    {
        if(baseMovementScript.mainPlayerScript.currentState == Player.StateMachine.ropeGrappling && isGrappling)
        {
            if(baseMovementScript.myRigidBody.velocity.x > 10f)
            {
                baseMovementScript.myRigidBody.velocity = new Vector2(10f, baseMovementScript.myRigidBody.velocity.y);
            }
            else if (baseMovementScript.myRigidBody.velocity.x < -10f)
            {
                baseMovementScript.myRigidBody.velocity = new Vector2(-10f, baseMovementScript.myRigidBody.velocity.y);
            }
            if (baseMovementScript.myRigidBody.velocity.y > 10f)
            {
                baseMovementScript.myRigidBody.velocity = new Vector2(baseMovementScript.myRigidBody.velocity.x, 10f);
            }
            else if (baseMovementScript.myRigidBody.velocity.y < -10f)
            {
                baseMovementScript.myRigidBody.velocity = new Vector2(baseMovementScript.myRigidBody.velocity.x, -10f);
            }
            if ((baseMovementScript.surroundingsCheckerScript.isFacingRight && baseMovementScript.myRigidBody.velocity.x > 1f) || (!baseMovementScript.surroundingsCheckerScript.isFacingRight && baseMovementScript.myRigidBody.velocity.x < -1f))
            {
                baseMovementScript.FlipCharacter();
            }
        }
    }
    internal void Falling()
    {
        baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.fall;
    }
    internal void Jump(float vectorX, float vectorY)
    {
        baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.jump;
        //myRigidBody.velocity = new Vector2(Mathf.Round(position.x) * mainPlayerScript.walkSpeed, myRigidBody.velocity.y);
        //myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, mainPlayerScript.jumpForce);
        baseMovementScript.myRigidBody.velocity = new Vector2(vectorX, vectorY);
    }
    internal void ChainClimb()
    {
        if (Input.GetButton("Jump"))
        {
            baseMovementScript.animatorScript.animator.speed = 1;
            baseMovementScript.surroundingsCheckerScript.isTouchingChain = false;
            if (baseMovementScript.inputScript.position.x != 0)
            {
                Jump(baseMovementScript.inputScript.position.x * jumpForce * 0.75f, jumpForce / 2);
            }
            else
            {
                baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.fall;
            }
        }
        else if (baseMovementScript.inputScript.position.x != 0)
        {
            baseMovementScript.FlipCharacter();
        }
        else if (baseMovementScript.inputScript.position.y == 0)
        {
            baseMovementScript.animatorScript.animator.speed = 0;
        }
        else
        {
            baseMovementScript.animatorScript.animator.speed = 1;
            transform.position = new Vector2(transform.position.x, transform.position.y + (baseMovementScript.inputScript.position.y * 0.06f));
            if (baseMovementScript.surroundingsCheckerScript.isTouchingCeiling && baseMovementScript.inputScript.position.y > 0)
            {
                float chainOffsetY = baseMovementScript.boxCollider.bounds.extents.y + 1f + baseMovementScript.surroundingsCheckerScript.chainCheckDistance + 0.2f;
                RaycastHit2D hit = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center + new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.up, chainOffsetY);
                if (hit.collider.gameObject.layer != baseMovementScript.surroundingsCheckerScript.whatIsGround)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + chainOffsetY);
                    baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.idle;
                }
            }
        }
    }
    internal void FindLedgeToClimb()
    {
        if (ledgeDetected && !baseMovementScript.canClimbLedge)
        {
            if (baseMovementScript.surroundingsCheckerScript.isFacingRight)
            {
                ledgePosition1 = new Vector2(Mathf.Floor(ledgePositionBottom.x + baseMovementScript.surroundingsCheckerScript.wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePositionBottom.y) + ledgeClimbYOffset1);
                ledgePosition2 = new Vector2(Mathf.Floor(ledgePositionBottom.x + baseMovementScript.surroundingsCheckerScript.wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePositionBottom.y) + ledgeClimbYOffset2);
            }
            else
            {
                ledgePosition1 = new Vector2(Mathf.Ceil(ledgePositionBottom.x - baseMovementScript.surroundingsCheckerScript.wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePositionBottom.y) + ledgeClimbYOffset1);
                ledgePosition2 = new Vector2(Mathf.Ceil(ledgePositionBottom.x - baseMovementScript.surroundingsCheckerScript.wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePositionBottom.y) + ledgeClimbYOffset2);
            }
            baseMovementScript.canClimbLedge = true;
            transform.position = ledgePosition1;
        }
        if (baseMovementScript.canClimbLedge)
        {
            baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.ledgeClimbing;
            baseMovementScript.myRigidBody.velocity = Vector2.zero;
        }
    }
    public void StartLiftingCharacterDuringClimbing()
    {
        StartCoroutine(LedgeClimbCO());
    }
    private IEnumerator LedgeClimbCO()
    {
        for (int i = 0; i < 25; i++)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.06f);
            yield return new WaitForSeconds(.01f);
        }
    }
    public void FinishLedgeClimb()
    {
        baseMovementScript.canClimbLedge = false;
        ledgeDetected = false;
        transform.position = ledgePosition2;
        baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.idle;
    }
    public void AttachToTheChain(float chainOffsetX, float chainOffsetY, float raycastOffsetY)
    {
        RaycastHit2D hit = Physics2D.BoxCast(baseMovementScript.boxCollider.bounds.center - new Vector3(0, baseMovementScript.boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * baseMovementScript.boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, raycastOffsetY, baseMovementScript.surroundingsCheckerScript.whatIsChain);
        if(hit.collider !=null)
        {
            if (baseMovementScript.surroundingsCheckerScript.isFacingRight)
            {
                transform.position = new Vector2(hit.collider.bounds.center.x - chainOffsetX, transform.position.y + chainOffsetY);
            }
            else
            {
                transform.position = new Vector2(hit.collider.bounds.center.x + chainOffsetX, transform.position.y + chainOffsetY);
            }
        }
    }
    internal void SetGrapplePoint()
    {
        Vector3 distanceVector = baseMovementScript.inputScript.mousePosition - grapplingFirePoint.position;
        RaycastHit2D hit = Physics2D.Raycast(grapplingFirePoint.position, distanceVector.normalized, grappleDistance, grapplableLayers);
        if(hit.collider != null)
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
    private void OnDrawGizmos()
    {
        //grapplingHook
        Gizmos.DrawLine(baseMovementScript.inputScript.mousePosition, grapplingFirePoint.position);
    }
}
