using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    void Update()
    {
        this.rb.velocity = speed * Time.deltaTime * launchDirection;   
    }
    public void Setup(Vector2 newDirection, float newDamage, float newLifeTime, float newSpeed)
    {
        this.launchDirection = newDirection;
        this.speed = newSpeed;
        this.damage = newDamage;
        this.lifeTime = newLifeTime;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Platforms")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == Mathf.Log(doDamage,2))
        {
            collision.gameObject.GetComponent<Destroyable>().DestroyObject();
            Destroy(gameObject);
        }
    }
}
