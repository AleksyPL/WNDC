using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public LayerMask interactablePlatforms;
    public LayerMask nonInteractablePlatforms;
    public LayerMask player;
    public LayerMask enemy;
    public LayerMask destroyable;
    public LayerMask destroyedElements;
    public GameObject bloodParticlesPrefab;

    public bool CheckWhatHasBeenHitBullet(GameObject receivingDamageObject, float recivedDamage, Vector3 launchDirection, Vector3 hitPoint)
    {
        if (receivingDamageObject.layer == Mathf.Log(enemy, 2) || receivingDamageObject.layer == Mathf.Log(destroyable, 2))
        {
            DoDamage(receivingDamageObject, recivedDamage, hitPoint);
            return true;
        }
        else if (receivingDamageObject.layer == Mathf.Log(destroyedElements, 2))
        {
            DoForcePush(receivingDamageObject, launchDirection, 5, hitPoint);
            return true;
        }
        else if (receivingDamageObject.layer == Mathf.Log(interactablePlatforms, 2) || receivingDamageObject.layer == Mathf.Log(nonInteractablePlatforms, 2))
        {
            if (!receivingDamageObject.CompareTag("Chain"))
                return true;
        }
        return false;
    }
    public bool CheckWhatHasBeenHitGrenade(GameObject whatCausedAnExplosion, float recivedDamage, Vector3 hitPoint, float explosionRadius, GameObject explosionEffect)
    {
        if (whatCausedAnExplosion.layer == Mathf.Log(enemy, 2) || whatCausedAnExplosion.layer == Mathf.Log(destroyable, 2) || whatCausedAnExplosion.layer == Mathf.Log(destroyedElements, 2) || whatCausedAnExplosion.layer == Mathf.Log(nonInteractablePlatforms, 2) || (whatCausedAnExplosion.layer == Mathf.Log(interactablePlatforms, 2) && !whatCausedAnExplosion.gameObject.CompareTag("Chain")))
        {
            SpawnParticleEffect(hitPoint, 0.35f, explosionEffect);
            List<GameObject> hitObjects = ReturnObjectsInRadius(hitPoint, explosionRadius);
            if (hitObjects.Count != 0)
            {
                for (int i = 0; i < hitObjects.Count; i++)
                {
                    if (hitObjects[i].layer == Mathf.Log(enemy, 2) || hitObjects[i].layer == Mathf.Log(destroyable, 2))
                    {
                        DoDamage(hitObjects[i], recivedDamage, hitPoint);
                    }
                }
            }
            else
                return true;
            hitObjects = ReturnObjectsInRadius(hitPoint, explosionRadius);
            if (hitObjects.Count != 0)
            {
                for (int i = 0; i < hitObjects.Count; i++)
                {
                    if (hitObjects[i].layer == Mathf.Log(destroyedElements, 2))
                    {
                        Vector3 launchDirection = hitObjects[i].transform.position - hitPoint;
                        DoForcePush(hitObjects[i], launchDirection, 3000, hitPoint);
                    }
                }
            }
            return true;
        }
        return false;
    }
    private List<GameObject> ReturnObjectsInRadius(Vector3 hitPoint, float explosionRadius)
    {
        Collider2D[] hitObjectsTemp = Physics2D.OverlapCircleAll(hitPoint, explosionRadius);
        List<GameObject> hitObjects = new List<GameObject>();
        if (hitObjectsTemp.Length != 0)
        {
            for (int i = 0; i < hitObjectsTemp.Length; i++)
            {
                if (hitObjectsTemp[i].gameObject.layer == Mathf.Log(enemy, 2) || hitObjectsTemp[i].gameObject.layer == Mathf.Log(destroyable, 2) || hitObjectsTemp[i].gameObject.layer == Mathf.Log(destroyedElements, 2))
                {
                    hitObjects.Add(hitObjectsTemp[i].gameObject);
                }
            }
        }
        return hitObjects;
    }
    private void ShootOffTheLimb(GameObject receivingDamageObject)
    {
        if(receivingDamageObject.GetComponent<HingeJoint2D>() && receivingDamageObject.GetComponentInParent<Ragdoll>())
        {
            if(!receivingDamageObject.transform.name.Contains("Spine"))
            {
                receivingDamageObject.GetComponent<HingeJoint2D>().connectedBody = null;
                receivingDamageObject.GetComponent<HingeJoint2D>().enabled = false;
            }
        }
    }
    private void SpawnParticleEffect(Vector3 hitPoint, float lifeTime, GameObject particleEffect)
    {
        GameObject blood = Instantiate(particleEffect, hitPoint, Quaternion.identity);
        blood.GetComponent<DestroyAfterTime>().lifeTime = lifeTime;
    }
    private void DoDamage(GameObject receivingDamageObject, float recivedDamage, Vector3 hitPoint)
    {
        if (receivingDamageObject.GetComponent<Enemy>())
        {
            SpawnParticleEffect(hitPoint, 1f, bloodParticlesPrefab);
            receivingDamageObject.GetComponent<Enemy>().DealDamage(recivedDamage);
        }
        else if (receivingDamageObject.GetComponent<Destroyable>())
        {
            receivingDamageObject.GetComponent<Destroyable>().DestroyObject();
        }
    }
    private void DoForcePush(GameObject receivingDamageObject, Vector3 launchDirection, float launchForce, Vector3 hitPoint)
    {
        if (receivingDamageObject.CompareTag("Enemy"))
        {
            SpawnParticleEffect(hitPoint, 1f, bloodParticlesPrefab);
            ShootOffTheLimb(receivingDamageObject);
        }
        if (receivingDamageObject.GetComponent<Rigidbody2D>())
        {
            receivingDamageObject.GetComponent<Rigidbody2D>().AddForce(launchDirection * launchForce);
        }
    }
}
