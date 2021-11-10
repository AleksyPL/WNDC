using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementGrapplingRope : MonoBehaviour
{
    public LineRenderer lineRenderer;
    [SerializeField]
    internal PlayerMovementAir airMovementScript;
    private void OnEnable()
    {
        lineRenderer.enabled = true;
    }
    private void OnDisable()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
        airMovementScript.isGrappling = false;
    }
    private void DrawRope()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, airMovementScript.grapplingFirePoint.position);
        lineRenderer.SetPosition(1, airMovementScript.grapplePoint);
    }
    private void FixedUpdate()
    {
        DrawRope();
    }
}
