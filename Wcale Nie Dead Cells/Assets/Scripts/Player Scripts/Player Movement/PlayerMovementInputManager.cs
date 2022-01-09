using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementBase))]
public class PlayerMovementInputManager : MonoBehaviour
{
    internal PlayerMovementBase baseMovementScript;
    public GameObject cameraGameObject;
    //public Camera camera;
    internal Vector3 position;
    internal Vector3 mousePosition;
    internal bool LPM_hold;
    internal bool RPM_hold;
    internal bool shiftHold;
    internal float scrollwheel;
    internal bool spacePressed;
    internal bool ePresses;

    void Start()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
        mousePosition = Vector3.zero;
        LPM_hold = false;
        RPM_hold = false;
        shiftHold = false;
        spacePressed = false;
        ePresses = false;
    }
    internal void GetInputs()
    {
        position.x = Input.GetAxisRaw("Horizontal");
        position.y = Input.GetAxisRaw("Vertical");
        scrollwheel = Input.GetAxisRaw("Mouse ScrollWheel");
        position.Normalize();
        //mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = cameraGameObject.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        LPM_hold = Input.GetButton("Fire1");
        RPM_hold = Input.GetButton("Fire2");
        shiftHold = Input.GetButton("Fire3");
        spacePressed = Input.GetButtonDown("Jump");
        ePresses = Input.GetButtonDown("Interact");
    }
    internal void FlipCharacter()
    {
        if(!baseMovementScript.surroundingsCheckerScript.isDead)
        {
            if ((baseMovementScript.inputScript.mousePosition.x > baseMovementScript.GetComponent<Collider2D>().bounds.center.x && !baseMovementScript.surroundingsCheckerScript.isFacingRight) || (baseMovementScript.inputScript.mousePosition.x < baseMovementScript.GetComponent<Collider2D>().bounds.center.x && baseMovementScript.surroundingsCheckerScript.isFacingRight))
            {
                baseMovementScript.surroundingsCheckerScript.isFacingRight = !baseMovementScript.surroundingsCheckerScript.isFacingRight;
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }  
    }
}
