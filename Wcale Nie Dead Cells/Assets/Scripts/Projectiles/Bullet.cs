using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        destroyingScript = GetComponent<DestroyAfterTime>();
    }
    void Update()
    {
        rb.velocity = speed * Time.deltaTime * launchDirection;   
    }
    public void Setup(Vector2 newDirection, float newDamage, float newLifeTime, float newSpeed)
    {
        launchDirection = newDirection;
        speed = newSpeed;
        damage = newDamage;
        destroyingScript.lifeTime = newLifeTime;
        transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        destroyingScript.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Mathf.Log(platformsLayerMask, 2) || collision.gameObject.layer == Mathf.Log(elevatorsLayerMask, 2))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == Mathf.Log(enemyLayerMask, 2))
        {
            if (collision.GetComponent<Enemy>())
            {
                Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);
                collision.gameObject.GetComponent<Enemy>().DealDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == Mathf.Log(destroyableLayerMask, 2))
        {
            if (collision.GetComponent<Destroyable>())
            {
                collision.gameObject.GetComponent<Destroyable>().DestroyObject();
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == Mathf.Log(deadEnemyLayerMask, 2))
        {
            //TODO add force to limbs
            Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        //if (collision.gameObject.name == "Platforms")
        //{
        //    Destroy(gameObject);
        //}
        //else if (collision.gameObject.layer == Mathf.Log(doDamage,2))
        //{
        //    if (collision.GetComponent<Enemy>())
        //    {
        //        collision.gameObject.GetComponent<Enemy>().DealDamage(this.damage);
        //    }
        //    else if (collision.GetComponent<Destroyable>())
        //    {
        //        collision.gameObject.GetComponent<Destroyable>().DestroyObject();
        //    }
        //    Destroy(gameObject);
        //}
    }
}
