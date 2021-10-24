using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    internal Player mainPlayerScript;

    //public Transform groundCheck;
    public Transform wallCheck;
    public Transform LedgeCheck;
    public LayerMask whatIsGround;
    public LayerMask whatIsChain;
    public LayerMask whatIsOneSidePlatform;

    private Rigidbody2D myRigidBody;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private Vector3 position;

    private bool canMove;
    private bool canFlip;
    private bool canJump;
    private bool canClimbLedge;
    private bool canClimbChain;

    private bool isFacingRight;
    private bool isGrounded;
    private bool isTouchingLedge;
    private bool isTouchingWall;
    private bool isTouchingChain;
    private bool isTouchingActivePlatform;

    //Ledge Climbing
    public float wallCheckDistance;
    private bool ledgeDetected;
    private Vector2 ledgePositionBottom;
    private Vector2 ledgePosition1;
    private Vector2 ledgePosition2;
    private readonly float ledgeClimbXOffset1 = 0.245f;
    private readonly float ledgeClimbXOffset2 = 0.35f;
    private readonly float ledgeClimbYOffset1 = 0.23f;
    private readonly float ledgeClimbYOffset2 = 1.7f;

    //Chain Climbing
    public float chainCheckDistance;
    public float ceilingCheckDistance;
    private readonly float chainOffsetX = 0.35f;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        canFlip = true;
        canMove = false;
        canJump = false;
        canClimbLedge = false;
        canClimbChain = false;
        isFacingRight = true;
        isGrounded = false;
        isTouchingWall = false;
        isTouchingLedge = false;
        isTouchingChain = false;
        isTouchingActivePlatform = false;
    }
    void FixedUpdate()
    {
        position.x = Input.GetAxisRaw("Horizontal");
        position.y = Input.GetAxisRaw("Vertical");
        CheckSurroundings();
        MoveCharacter();
        AnimateCharacter();
    }
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.BoxCast(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 0.1f, whatIsGround);
        isTouchingChain = Physics2D.BoxCast(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 0.1f, whatIsChain);
        if (!isGrounded)
        {
            isGrounded = Physics2D.BoxCast(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 0.1f, whatIsOneSidePlatform);
        }
        isTouchingWall = Physics2D.Raycast(wallCheck.transform.position, transform.right, wallCheckDistance, whatIsGround);
        isTouchingLedge = Physics2D.Raycast(LedgeCheck.transform.position, transform.right, wallCheckDistance, whatIsGround);
        if (isGrounded)
        {
            if (mainPlayerScript.currentState == Player.StateMachine.ledgeClimbing)
            {
                myRigidBody.gravityScale = 0;
                canMove = false;
                canFlip = false;
            }
            else
            {
                isTouchingActivePlatform = Physics2D.Raycast(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y), Vector2.down, 0.03f, whatIsOneSidePlatform);
                ledgeDetected = false;
                myRigidBody.gravityScale = 1;
                canMove = true;
                canFlip = true;
                if (isTouchingChain)
                {
                    canJump = false;
                    canClimbChain = true;
                }
                else if (isTouchingActivePlatform)
                {
                    canClimbChain = Physics2D.BoxCast(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 1.1f, whatIsChain);
                    canJump = true;
                }
                else
                {
                    canJump = true;
                    canClimbChain = false;
                }
            }
        }
        else if (!isGrounded)
        {
            if (mainPlayerScript.currentState == Player.StateMachine.chainClimbing || mainPlayerScript.currentState == Player.StateMachine.ledgeClimbing || canClimbLedge)
            {
                canMove = true;
                myRigidBody.gravityScale = 0;
                myRigidBody.velocity = Vector2.zero;
                isTouchingActivePlatform = Physics2D.Raycast(boxCollider.bounds.center + new Vector3(0, boxCollider.bounds.extents.y), Vector2.up, 0.03f, whatIsOneSidePlatform);
            }
            else
            {
                canMove = false;
                myRigidBody.gravityScale = 1;
            }
            canJump = false;
            canFlip = true;
            if (isTouchingWall && !isTouchingLedge && !ledgeDetected && !isTouchingChain)
            {
                ledgeDetected = true;
                ledgePositionBottom = wallCheck.position;
                FindLedgeToClimb();
            }
        }
    }
    private void FlipCharacter()
    {
        if (canFlip && (position.x < 0 && isFacingRight || position.x > 0 && !isFacingRight))
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }
    private void Falling()
    {
        mainPlayerScript.currentState = Player.StateMachine.fall;
        if (position.x < 0 && isFacingRight || position.x > 0 && !isFacingRight)
        {
            FlipCharacter();
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x * -0.75f, myRigidBody.velocity.y);
        }
    }
    private void Jump()
    {
        mainPlayerScript.currentState = Player.StateMachine.jump;
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, mainPlayerScript.jumpForce);
    }
    private void ChainClimb()
    {
        if (Input.GetButton("Jump"))
        {
            animator.speed = 1;
            if (position.x > 0)
            {
                //TODO skok w prawo
            }
            else if (position.x < 0)
            {
                //TODO skok w lewo
            }
            else
            {
                Falling();
            }
        }
        else if (position.x != 0)
        {
            FlipCharacter();
        }
        else if (position.y == 0)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
            transform.position = new Vector2(transform.position.x, transform.position.y + (position.y * 0.06f));
            if(isTouchingActivePlatform && position.y > 0)
            {
                float chainOffsetY = boxCollider.bounds.extents.y + 1f + chainCheckDistance + 0.1f;
                transform.position = new Vector2(transform.position.x, transform.position.y + chainOffsetY);
                mainPlayerScript.currentState = Player.StateMachine.idle;
            }
        }
    }
    private void FindLedgeToClimb()
    {
        if (ledgeDetected && !canClimbLedge)
        {
            if (isFacingRight)
            {
                ledgePosition1 = new Vector2(Mathf.Floor(ledgePositionBottom.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePositionBottom.y) + ledgeClimbYOffset1);
                ledgePosition2 = new Vector2(Mathf.Floor(ledgePositionBottom.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePositionBottom.y) + ledgeClimbYOffset2);
            }
            else
            {
                ledgePosition1 = new Vector2(Mathf.Ceil(ledgePositionBottom.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePositionBottom.y) + ledgeClimbYOffset1);
                ledgePosition2 = new Vector2(Mathf.Ceil(ledgePositionBottom.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePositionBottom.y) + ledgeClimbYOffset2);
            }
            canClimbLedge = true;
            transform.position = ledgePosition1;
        }
        if (canClimbLedge)
        {
            mainPlayerScript.currentState = Player.StateMachine.ledgeClimbing;
            myRigidBody.velocity = Vector2.zero;
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
        canClimbLedge = false;
        ledgeDetected = false;
        transform.position = ledgePosition2;
        mainPlayerScript.currentState = Player.StateMachine.idle;
    }
    private void MoveCharacter()
    {
        position.Normalize();
        if (canMove && isGrounded)
        {
            if (position == Vector3.zero && mainPlayerScript.currentState != Player.StateMachine.ledgeClimbing)
            {
                mainPlayerScript.currentState = Player.StateMachine.idle;
                myRigidBody.velocity = Vector2.zero;
            }
            if (position.x != 0 && position.y == 0)
            {
                mainPlayerScript.currentState = Player.StateMachine.walk;
                myRigidBody.velocity = new Vector2(Mathf.Round(position.x) * mainPlayerScript.walkSpeed, myRigidBody.velocity.y);
                FlipCharacter();
            }
            if (position.y > 0 && canJump && !isTouchingChain && !canClimbChain)
            {
                mainPlayerScript.currentState = Player.StateMachine.jump;
                Jump();
            }
            if (position.y < 0 && !isTouchingChain)
            {
                mainPlayerScript.currentState = Player.StateMachine.slide;
                //TODO Slide
            }
            if (position.y > 0 && isTouchingChain && mainPlayerScript.currentState != Player.StateMachine.chainClimbing && canClimbChain && !canJump)
            {
                mainPlayerScript.currentState = Player.StateMachine.chainClimbing;
                RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 0.1f, whatIsChain);
                if (isFacingRight)
                {
                    transform.position = new Vector2(hit.collider.bounds.center.x - chainOffsetX, transform.position.y + 0.2f);
                }
                else
                {
                    transform.position = new Vector2(hit.collider.bounds.center.x + chainOffsetX, transform.position.y + 0.2f);
                }
            }
            else if (position.y < 0 && isTouchingActivePlatform && canClimbChain && mainPlayerScript.currentState != Player.StateMachine.chainClimbing)
            {
                RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center - new Vector3(0, boxCollider.bounds.extents.y - 0.1f), new Vector3(2 * boxCollider.bounds.extents.x, 0.1f, 0), 0f, Vector2.down, 1.1f, whatIsChain);
                mainPlayerScript.currentState = Player.StateMachine.chainClimbing;
                float chainOffsetY = boxCollider.bounds.size.y + 1f - chainCheckDistance + 0.1f;
                if (isFacingRight)
                {
                    transform.position = new Vector2(hit.collider.bounds.center.x - chainOffsetX, transform.position.y - chainOffsetY);
                }
                else
                {
                    transform.position = new Vector2(hit.collider.bounds.center.x + chainOffsetX, transform.position.y - chainOffsetY);
                }
                ChainClimb();
            }
        }
        else if (canMove && !isGrounded && isTouchingChain && mainPlayerScript.currentState == Player.StateMachine.chainClimbing)
        {
            ChainClimb();
        }
        else if (mainPlayerScript.currentState != Player.StateMachine.jump && mainPlayerScript.currentState != Player.StateMachine.ledgeClimbing)
        {
            Falling();
        }
    }
    private void AnimateCharacter()
    {
        if (mainPlayerScript.currentState == Player.StateMachine.walk)
        {
            animator.SetBool("Moving", true);
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("LedgeClimbing", false);
            animator.SetBool("LadderClimbing", false);
        }
        else if (mainPlayerScript.currentState == Player.StateMachine.jump)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Jumping", true);
            animator.SetBool("Falling", false);
            animator.SetBool("LedgeClimbing", false);
            animator.SetBool("LadderClimbing", false);
        }
        else if (mainPlayerScript.currentState == Player.StateMachine.fall)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", true);
            animator.SetBool("LedgeClimbing", false);
            animator.SetBool("LadderClimbing", false);
        }
        else if (mainPlayerScript.currentState == Player.StateMachine.ledgeClimbing)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("LedgeClimbing", true);
            animator.SetBool("LadderClimbing", false);
        }
        else if (mainPlayerScript.currentState == Player.StateMachine.chainClimbing)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("LedgeClimbing", false);
            animator.SetBool("LadderClimbing", true);
        }
        else if (mainPlayerScript.currentState == Player.StateMachine.idle)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
            animator.SetBool("LedgeClimbing", false);
            animator.SetBool("LadderClimbing", false);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(GetComponent<BoxCollider2D>().bounds.center - new Vector3(0, GetComponent<BoxCollider2D>().bounds.extents.y + 0.05f), new Vector3(2 * GetComponent<BoxCollider2D>().bounds.extents.x, 0.1f, 0));
        if (isFacingRight)
        {
            Gizmos.DrawLine(wallCheck.transform.position, new Vector2(wallCheck.transform.position.x + wallCheckDistance, wallCheck.transform.position.y));
            Gizmos.DrawLine(LedgeCheck.transform.position, new Vector2(LedgeCheck.transform.position.x + wallCheckDistance, LedgeCheck.transform.position.y));
        }
        else
        {
            Gizmos.DrawLine(wallCheck.transform.position, new Vector2(wallCheck.transform.position.x - wallCheckDistance, wallCheck.transform.position.y));
            Gizmos.DrawLine(LedgeCheck.transform.position, new Vector2(LedgeCheck.transform.position.x - wallCheckDistance, LedgeCheck.transform.position.y));
        }
    }
}
