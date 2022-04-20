using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        destroyingScript = GetComponent<DestroyAfterTime>();
        damageSystemSript = GetComponent<DamageSystem>();
    }
    void Update()
    {
        rb.velocity = speed * Time.deltaTime * transform.right;   
    }
    public void Setup(float newDamage, float newLifeTime, float newSpeed)
    {
        speed = newSpeed;
        damage = newDamage;
        destroyingScript.lifeTime = newLifeTime;
        destroyingScript.enabled = true;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(damageSystemSript.CheckWhatHasBeenHitBullet(collision.gameObject, damage, transform.rotation.eulerAngles, transform.position))
            Destroy(gameObject);
    }
}
