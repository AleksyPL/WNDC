using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementAir))]
public class PlayerMovementGrapplingRope : MonoBehaviour
{
    public LineRenderer lineRenderer;
    internal PlayerMovementAir airMovementScript;
    void OnEnable()
    {
        airMovementScript = GetComponent<PlayerMovementAir>();
    }
    void OnDisable()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
        airMovementScript.isGrappling = false;
    }
    private void DrawRope()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, airMovementScript.grapplingFirePoint.position);
        lineRenderer.SetPosition(1, airMovementScript.grapplePoint);
    }
    void Update()
    {
        DrawRope();
    }
}
