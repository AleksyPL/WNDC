using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    internal Animator animator;
    internal Enemy baseEnemyScript;
    private Ragdoll ragdollScript;
    void Start()
    {
        animator = GetComponent<Animator>();
        baseEnemyScript = GetComponent<Enemy>();
        ragdollScript = GetComponent<Ragdoll>();
    }
    void Update()
    {
        if(baseEnemyScript.currentState == Enemy.StateMachine.dead)
        {
            ragdollScript.ToggleRagdoll(true);
        }
    }
}
