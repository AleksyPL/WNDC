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
        this.currentState = StateMachine.idle;
        this.animatorScript = GetComponent<EnemyAnimator>();
        this.surroundingsCheckerScript = GetComponent<EnemySurroundingsChecker>();
        this.destroyingScript = GetComponent<DestroyAfterTime>();
        this.rb = GetComponent<Rigidbody2D>();
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.animatorScript = GetComponent<EnemyAnimator>();
    }
    void Update()
    {
        
    }
    public void DealDamage(float projectileDamage)
    {
        this.HP -= projectileDamage;
        if(this.HP <= 0)
        {
            this.currentState = StateMachine.dead;
            this.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            foreach (Transform child in transform.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            }
            destroyingScript.enabled = true;
        }
    }
}
