using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementBase))]
public class PlayerMovementGround : MonoBehaviour
{
    private PlayerMovementBase baseMovementScript;
    public float walkSpeed;
    public float slideSpped;
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;

    private float currentDashTime;
    private float currentDashCooldown;
    internal bool dashReady;
    internal bool isDashing;
    private bool startCooldown;
    void Start()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
        currentDashTime = 0;
        currentDashCooldown = 0;
        startCooldown = true;
        isDashing = false;   
    }
    private void Update()
    {
        if (startCooldown)
        {
            currentDashCooldown += Time.deltaTime;
            if (currentDashCooldown >= dashCooldown)
            {
                startCooldown = false;
                currentDashCooldown = 0;
                dashReady = true;
            }
        }
        if (isDashing)
        {
            currentDashTime += Time.deltaTime;
            if (currentDashTime >= dashDuration)
            {
                currentDashTime = 0;
                dashReady = false;
                startCooldown = true;
                isDashing = false;
            }
        }
    }
    internal void Run()
    {
        baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.running;
        baseMovementScript.myRigidBody.velocity = new Vector2(Mathf.Round(baseMovementScript.inputScript.position.x) * walkSpeed, baseMovementScript.myRigidBody.velocity.y);
        Debug.Log("Running");
    }
    internal void Dash()
    {
        if(currentDashTime < dashDuration && isDashing)
        {
            baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.dashing;
            if (baseMovementScript.surroundingsCheckerScript.isFacingRight)
            {
                baseMovementScript.myRigidBody.velocity = new Vector2(1f * dashSpeed, baseMovementScript.myRigidBody.velocity.y);
            }
            else
            {
                baseMovementScript.myRigidBody.velocity = new Vector2(-1f * dashSpeed, baseMovementScript.myRigidBody.velocity.y);
            }
        }
        //if (isDashing)
        //{

        //    if (currentDashTime > 0 && !baseMovementScript.surroundingsCheckerScript.isTouchingWall)
        //    {
        //        currentDashTime -= Time.deltaTime;
        //        baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.dash;
        //        baseMovementScript.myRigidBody.velocity = new Vector2(baseMovementScript.myRigidBody.velocity.x * dashSpeed, baseMovementScript.myRigidBody.velocity.y);
        //    }
        //    if (currentDashTime <= 0 || baseMovementScript.surroundingsCheckerScript.isTouchingWall)
        //    {
        //        isDashing = false;
        //        baseMovementScript.mainPlayerScript.currentState = Player.StateMachine.idle;
        //        baseMovementScript.mainPlayerScript.cooldownScript.ApplyDashCooldown();
        //    }
        //}
        //else if (!isDashing && baseMovementScript.mainPlayerScript.cooldownScript.dashReady && baseMovementScript.inputScript.position.x != 0 && !baseMovementScript.surroundingsCheckerScript.isTouchingWall && currentDashTime <= 0)
        //{
        //    isDashing = true;
        //    currentDashTime = dashDuration;
        //}
    }
}
