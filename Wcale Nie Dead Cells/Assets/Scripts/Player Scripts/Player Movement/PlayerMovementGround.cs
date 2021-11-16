using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementGround : MonoBehaviour
{
    [SerializeField]
    internal PlayerMovementBase baseMovementScript;
    public float walkSpeed;
    public float slideSpped;
    public float dashSpeed;
    public float dashDuration;
    private float currentDashTime;
    private bool isDashing;
    // Start is called before the first frame update
    void Start()
    {
        currentDashTime = 0;
        isDashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    internal void Dash()
    {
        if(baseMovementScript.canDash && baseMovementScript.mainPlayerScript.cooldownScript.dashReady && !isDashing && baseMovementScript.position.x !=0 && !baseMovementScript.surroundingsCheckerScript.isTouchingWall)
        {
            isDashing = true;
            currentDashTime = dashDuration;
        }
        if (isDashing && currentDashTime > 0)
        {
            currentDashTime -= Time.deltaTime;
            //Debug.Log("Dashing");
            baseMovementScript.myRigidBody.velocity = new Vector2(baseMovementScript.myRigidBody.velocity.x * dashSpeed, baseMovementScript.myRigidBody.velocity.y);
        }
        if (isDashing && currentDashTime <= 0)
        {
            isDashing = false;
            baseMovementScript.mainPlayerScript.cooldownScript.ApplyDashCooldown();
        }
    }
}
