using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementInputManager : MonoBehaviour
{
    [SerializeField]
    internal PlayerMovementBase baseMovementScript;
    public Camera camera;
    internal Vector3 position;
    internal Vector3 mousePosition;
    internal bool LPM_hold;
    internal bool RPM_hold;
    internal bool shiftHold;
    void Start()
    {
        LPM_hold = false;
        RPM_hold = false;
        shiftHold = false;
    }
    internal void GetInputs()
    {
        position.x = Input.GetAxisRaw("Horizontal");
        position.y = Input.GetAxisRaw("Vertical");
        position.Normalize();
        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        LPM_hold = Input.GetButton("Fire1");
        RPM_hold = Input.GetButton("Fire2");
        shiftHold = Input.GetButton("Fire3");
    }
}
