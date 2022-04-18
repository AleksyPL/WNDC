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
    public void CheckWhatHasBeenHitted(GameObject receivingDamageObject, float recivedDamage, Vector3 launchDirection, float launchForce)
    {
        if (receivingDamageObject.layer == Mathf.Log(enemy, 2) || receivingDamageObject.layer == Mathf.Log(destroyable, 2))
        {
            DoDamage(receivingDamageObject, recivedDamage);
            Destroy(gameObject);
        }
        else if(receivingDamageObject.layer == Mathf.Log(destroyedElements, 2))
        {
            DoForcePush(receivingDamageObject, launchDirection, launchForce);
            Destroy(gameObject);
        }
        else if(receivingDamageObject.layer == Mathf.Log(interactablePlatforms, 2) || receivingDamageObject.layer == Mathf.Log(nonInteractablePlatforms, 2))
        {
            if(!receivingDamageObject.CompareTag("Chain"))
                Destroy(gameObject);
        }
    }
    public void DoDamage(GameObject receivingDamageObject, float recivedDamage = 0)
    {
        if (receivingDamageObject.GetComponent<Enemy>())
        {
            Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);
            receivingDamageObject.GetComponent<Enemy>().DealDamage(recivedDamage);
        }
        else if (receivingDamageObject.GetComponent<Destroyable>())
        {
            receivingDamageObject.GetComponent<Destroyable>().DestroyObject();
        }
    }
    public void DoForcePush(GameObject receivingDamageObject, Vector3 launchDirection, float launchForce)
    {
        if (receivingDamageObject.GetComponent<Rigidbody2D>())
        {
            receivingDamageObject.GetComponent<Rigidbody2D>().AddForce(launchDirection * launchForce);
        }
        if (receivingDamageObject.GetComponent<Enemy>())
        {
            Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);
        }
    }
}
