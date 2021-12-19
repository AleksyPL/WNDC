using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 launchDirection;
    private TrailRenderer trailRenderer;
    //[SerializeField]
    //private LayerMask doDamage;
    //[SerializeField]
    //private LayerMask destroyBullet;

    private float speed;
    private float damage;
    private float lifeTime;
    private float lifeTimeCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }
    void Update()
    {
        this.lifeTimeCounter += Time.deltaTime;
        this.rb.velocity = launchDirection * speed * Time.deltaTime;
        if (this.lifeTimeCounter >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
    public void Setup(Vector2 newDirection, float newDamage, float newLifeTime, float newSpeed)
    {
        this.launchDirection = newDirection;
        this.speed = newSpeed;
        this.damage = newDamage;
        this.lifeTime = newLifeTime;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        //this.trailRenderer.time = newLifeTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Platforms")
        {
            Destroy(gameObject);
        }
    }
}
