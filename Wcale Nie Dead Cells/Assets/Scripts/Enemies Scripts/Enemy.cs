using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    internal enum StateMachine
    {
        idle,
        atack,
        dead
    }
    internal StateMachine currentState;
    internal BoxCollider2D boxCollider;
    internal Rigidbody2D rb;
    internal EnemyAnimator animatorScript;
    internal EnemySurroundingsChecker surroundingsCheckerScript;
    internal DestroyAfterTime destroyingScript;
    public float HP;
    internal float damage;
    // Start is called before the first frame update
    void Start()
    {
        currentState = StateMachine.idle;
        animatorScript = GetComponent<EnemyAnimator>();
        surroundingsCheckerScript = GetComponent<EnemySurroundingsChecker>();
        destroyingScript = GetComponent<DestroyAfterTime>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animatorScript = GetComponent<EnemyAnimator>();
        destroyingScript.lifeTime = 5f;
        //destroyingScript.colorFadeTick = destroyingScript.lifeTime / 255;
    }
    void Update()
    {
        
    }
    public void DealDamage(float projectileDamage)
    {
        HP -= projectileDamage;
        if(HP <= 0)
        {
            currentState = StateMachine.dead;
            gameObject.layer = LayerMask.NameToLayer("DestroyedElements");
            foreach (Transform child in transform.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = LayerMask.NameToLayer("DestroyedElements");
            }
            destroyingScript.enabled = true;
            animatorScript.ragdollScript.ToggleRagdoll(true);
        }
    }
}
