using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public GameObject[] bonesAndLimbs;
    public GameObject[] IKSolvers;
    public Collider2D mainCollider;
    public Rigidbody2D mainRigidBody;
    public Animator animator;
    private bool ragdollActive = false;
    void Start()
    {
        ToggleRagdoll(false);
    }
    internal void ToggleRagdoll(bool argument)
    {
        ragdollActive = argument;
        for(int i=0;i<IKSolvers.Length;i++)
        {
            IKSolvers[i].SetActive(!argument);
        }
        mainRigidBody.simulated = !argument;
        mainCollider.enabled = !argument;
        animator.enabled = !argument;
        for (int i = 0; i < bonesAndLimbs.Length; i++)
        {
            if (bonesAndLimbs[i].GetComponent<Rigidbody2D>())
            {
                if (!argument)
                    bonesAndLimbs[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                else
                    bonesAndLimbs[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
            if (bonesAndLimbs[i].GetComponent<HingeJoint2D>())
            {
                bonesAndLimbs[i].GetComponent<HingeJoint2D>().enabled = argument;
            }
        }
    }
}
