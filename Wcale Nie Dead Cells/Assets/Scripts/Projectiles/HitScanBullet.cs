using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanBullet : MonoBehaviour
{
    //internal Vector3 startPoint;
    internal Vector3 endPoint;
    //internal float lifeTime;
    internal float bulletSpeed;
    //private float lifeTimeCounter;
    public void Setup(Vector3 newEndPoint, float newBulletSpeed)
    {
        endPoint = newEndPoint;
        //lifeTime = newLifeTime;
        bulletSpeed = newBulletSpeed;
    }
    void Update()
    {
        //lifeTimeCounter += Time.deltaTime;
        //transform.position = Vector3.Lerp(startPoint, endPoint, lifeTimeCounter / lifeTime);
        Vector3.MoveTowards(transform.position, endPoint, bulletSpeed * Time.deltaTime);
        if (transform.position == endPoint)
            Destroy(gameObject);
    }
}
