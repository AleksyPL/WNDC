using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    internal Rigidbody2D rb;
    internal Vector2 launchDirection;
    internal float damage;
    internal float lifeTime;
    internal float speed;
    private float lifeTimeCounter;
    public LayerMask doDamage;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        lifeTimeCounter += Time.deltaTime;
        if (lifeTimeCounter >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
