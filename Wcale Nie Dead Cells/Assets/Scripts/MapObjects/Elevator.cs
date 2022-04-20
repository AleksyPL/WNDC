using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Interactable
{
    public LayerMask whatIsGround;
    public float elevatorTravelDistance;
    public ElevatorDirections elevatorDirection;
    public float elevatorTravelSpeed;
    private bool elevatorActive;
    private bool playerCanActivateElevator;
    //private bool playerIsStandingOnTheElevator;
    private Vector3 elevatorFinalPosition;
    public enum ElevatorDirections
    {
        up,
        down
    }
    private void Start()
    {
        elevatorActive = false;
        playerCanActivateElevator = false;
        //playerIsStandingOnTheElevator = false;
        elevatorFinalPosition = Vector3.zero;
        CreateRopes();
    }

    private void Update()
    {
        if (playerCanActivateElevator && player.baseMovementScript.inputScript.ePresses)
        {
            InteractWithPlayer();
        }
        if(elevatorActive)
        {
            if (transform.position == elevatorFinalPosition)
            {
                elevatorActive = false;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, elevatorFinalPosition, elevatorTravelSpeed * Time.deltaTime);
                //if (elevatorDirection == ElevatorDirections.up)
                //{
                //    transform.position += new Vector3(0, 0.05f, 0);
                //}
                //else if (elevatorDirection == ElevatorDirections.down)
                //{
                //    transform.position -= new Vector3(0, 0.05f, 0);
                //}
            }
        }
    }
    private float CheckElevatorDirection()
    {
        bool isTouchingCeiling = Physics2D.BoxCast(gameObject.GetComponent<BoxCollider2D>().bounds.center + new Vector3(0, gameObject.GetComponent<BoxCollider2D>().bounds.extents.y + 2), new Vector3(2 * gameObject.GetComponent<BoxCollider2D>().bounds.extents.x, 0.1f, 0), 0f, Vector2.up, 0.1f, whatIsGround);
        bool isTouchingGround = Physics2D.BoxCast(transform.position, new Vector3(2 * gameObject.GetComponent<BoxCollider2D>().bounds.extents.x, 0.5f, 0), 0f, Vector2.up, 0.5f, whatIsGround);
        if (elevatorDirection == ElevatorDirections.up)
        {
            //if(!isTouchingCeiling)
            //{
            //    for (float i = 0; i < elevatorTravelDistance; i++)
            //    {
            //        bool willTouchingCeiling = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Vector2.up, i, whatIsGround);
            //        if (willTouchingCeiling)
            //        {
            //            return i;
            //        }
            //    }
            //}
            //else
            //{
            //    elevatorDirection = ElevatorDirections.down;
            //    return elevatorTravelDistance;
            //}
            //TODO
            bool willTouchingCeiling = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Vector2.up, elevatorTravelDistance, whatIsGround);
            if (!isTouchingCeiling && !willTouchingCeiling)
            {
                return elevatorTravelDistance;
            }
            else
            {
                elevatorDirection = ElevatorDirections.down;
                //return elevatorTravelDistance;
            }
        }
        if (elevatorDirection == ElevatorDirections.down)
        {
            //if (!isTouchingGround)
            //{
            //    for (float i = 0; i < elevatorTravelDistance; i++)
            //    {
            //        bool willTouchingGround = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Vector2.down, i, whatIsGround);
            //        if (willTouchingGround)
            //        {
            //            return i;
            //        }
            //    }
            //}
            //else
            //{
            //    elevatorDirection = ElevatorDirections.up;
            //    return elevatorTravelDistance;
            //}
            if (!isTouchingGround)
            {
                for (float i = 0; i <= elevatorTravelDistance; i++)
                {
                    bool willTouchingGround = Physics2D.Raycast(transform.position, Vector2.down, i, whatIsGround);
                    if (willTouchingGround)
                        return i;
                }
            }
            else
            {
                elevatorDirection = ElevatorDirections.up;
                return elevatorTravelDistance;
            }
        }
        return 0;
    }
    public void CreateRopes()
    {
        GameObject leftSide = GameObject.Find("LeftSide").gameObject;
        GameObject rightSide = GameObject.Find("RightSide").gameObject;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector3.up, Mathf.Infinity, whatIsGround);
        if (ray.collider != null)
        {
            if (Vector3.Distance(transform.position, ray.point) < 0.51f)
            {
                RaycastHit2D ray1 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 0.51f, transform.position.z), Vector3.up, Mathf.Infinity, whatIsGround);
                if(ray1.collider !=null)
                {
                    leftSide.GetComponent<LineRenderer>().SetPosition(0, new Vector3(leftSide.transform.position.x, ray.point.y));
                    leftSide.GetComponent<LineRenderer>().SetPosition(1, new Vector3(leftSide.transform.position.x, ray1.point.y));
                    rightSide.GetComponent<LineRenderer>().SetPosition(0, new Vector3(rightSide.transform.position.x, ray.point.y));
                    rightSide.GetComponent<LineRenderer>().SetPosition(1, new Vector3(rightSide.transform.position.x, ray1.point.y));

                    //point per one grid cell
                    //double distance2 = Math.Round(Vector3.Distance(ray.point, ray1.point), 1);
                    //leftSide.GetComponent<LineRenderer>().positionCount = (int)distance2 + 1;
                    //rightSide.GetComponent<LineRenderer>().positionCount = (int)distance2 + 1;
                    //for (int i = 0; i <= distance2; i++)
                    //{
                    //    leftSide.GetComponent<LineRenderer>().SetPosition(i, new Vector3(leftSide.transform.position.x, leftSide.transform.position.y + i + 0.5f));
                    //    rightSide.GetComponent<LineRenderer>().SetPosition(i, new Vector3(rightSide.transform.position.x, rightSide.transform.position.y + i + 0.5f));
                    //}
                }
            }
            else
            {
                RaycastHit2D ray1 = Physics2D.Raycast(transform.position, Vector3.down, Mathf.Infinity, whatIsGround);
                if(ray1.collider != null)
                {
                    leftSide.GetComponent<LineRenderer>().SetPosition(0, new Vector3(leftSide.transform.position.x, ray.point.y));
                    leftSide.GetComponent<LineRenderer>().SetPosition(1, new Vector3(leftSide.transform.position.x, ray1.point.y));
                    rightSide.GetComponent<LineRenderer>().SetPosition(0, new Vector3(rightSide.transform.position.x, ray.point.y));
                    rightSide.GetComponent<LineRenderer>().SetPosition(1, new Vector3(rightSide.transform.position.x, ray1.point.y));
                }
            }
        }
    }
    public override void InteractWithPlayer()
    {
        if (playerCanActivateElevator && !elevatorActive)
        {
            elevatorActive = true;
            float elevatorTravelDistanceTemp = CheckElevatorDirection();
            if (elevatorDirection == ElevatorDirections.up)
            {
                elevatorFinalPosition = new Vector3(transform.position.x, transform.position.y + elevatorTravelDistanceTemp, transform.position.z);
            }
            else if (elevatorDirection == ElevatorDirections.down)
            {
                elevatorFinalPosition = new Vector3(transform.position.x, transform.position.y - elevatorTravelDistanceTemp, transform.position.z);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            playerCanActivateElevator = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerCanActivateElevator = false;
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        playerIsStandingOnTheElevator = true;
    //    }
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        playerIsStandingOnTheElevator = false;
    //    }
    //}
}
