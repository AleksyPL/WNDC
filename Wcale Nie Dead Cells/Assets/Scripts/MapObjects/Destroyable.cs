using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    internal bool alreadyCracked = false;
    public GameObject crackedVersion;
    public GameObject itemToSpawn;
    public float chancesToSpawnItem;
    private bool DecideToSpawnItem()
    {
        int randomNumber = Random.Range(0, 100);
        if (randomNumber <= chancesToSpawnItem)
            return true;
        else
            return false;
    }
    public void DestroyObject()
    {
        if(!alreadyCracked)
        {
            alreadyCracked = true;
            GameObject destroyedElement = Instantiate(crackedVersion, transform.position, Quaternion.identity);
            destroyedElement.GetComponent<DestroyAfterTime>().lifeTime = 5;
            if (DecideToSpawnItem() && itemToSpawn != null)
                Instantiate(itemToSpawn, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
