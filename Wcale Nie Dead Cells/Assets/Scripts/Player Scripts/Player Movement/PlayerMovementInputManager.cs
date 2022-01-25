using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(PlayerMovementBase))]
public class PlayerMovementInputManager : MonoBehaviour
{
    internal PlayerMovementBase baseMovementScript;
    public GameObject cameraGameObject;
    internal Vector3 position;
    internal Vector3 mousePosition;
    internal bool LMB_pressed;
    internal bool LMB_hold;
    internal bool RMB_hold;
    internal bool shiftHold;
    internal float scrollwheel;
    internal bool spacePressed;
    internal bool rPressed;
    internal bool ePresses;

    void Start()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
        mousePosition = Vector3.zero;
        LMB_pressed = false;
        LMB_hold = false;
        RMB_hold = false;
        shiftHold = false;
        spacePressed = false;
        rPressed = false;
        ePresses = false;
    }
    internal void GetInputs()
    {
        position.x = Input.GetAxisRaw("Horizontal");
        position.y = Input.GetAxisRaw("Vertical");
        scrollwheel = Input.GetAxisRaw("Mouse ScrollWheel");
        position.Normalize();
        mousePosition = cameraGameObject.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        LMB_pressed = Input.GetButtonDown("Fire1");
        LMB_hold = Input.GetButton("Fire1");
        RMB_hold = Input.GetButton("Fire2");
        shiftHold = Input.GetButton("Fire3");
        spacePressed = Input.GetButtonDown("Jump");
        rPressed = Input.GetButtonDown("Reload");
        ePresses = Input.GetButtonDown("Interact");
    }
    internal void FlipCharacter()
    {
        if(baseMovementScript.mainPlayerScript.currentState != Player.StateMachine.dead)
        {
            if ((baseMovementScript.inputScript.mousePosition.x > baseMovementScript.GetComponent<Collider2D>().bounds.center.x && !baseMovementScript.surroundingsCheckerScript.isFacingRight) || (baseMovementScript.inputScript.mousePosition.x < baseMovementScript.GetComponent<Collider2D>().bounds.center.x && baseMovementScript.surroundingsCheckerScript.isFacingRight))
            {
                baseMovementScript.surroundingsCheckerScript.isFacingRight = !baseMovementScript.surroundingsCheckerScript.isFacingRight;
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }  
    }
}
