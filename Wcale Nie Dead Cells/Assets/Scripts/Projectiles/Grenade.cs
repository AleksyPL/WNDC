using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Projectile
{
    internal float explosionRadius;
    internal GameObject explosionEffect;
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
    public void Setup(float newDamage, float newLifeTime, float newSpeed, float newExplosionRadius, GameObject newExplosionEffect)
    {
        speed = newSpeed;
        damage = newDamage;
        destroyingScript.lifeTime = newLifeTime;
        destroyingScript.enabled = true;
        explosionRadius = newExplosionRadius;
        explosionEffect = newExplosionEffect;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageSystemSript.CheckWhatHasBeenHitGrenade(collision.gameObject, damage, transform.position, explosionRadius, explosionEffect))
            Destroy(gameObject);
    }
}
