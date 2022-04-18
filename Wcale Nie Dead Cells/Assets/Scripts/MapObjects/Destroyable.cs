using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public GameObject crackedVersion;
    public GameObject itemToSpawn;
    public float chancesToSpawnItem;
    //public float HP = 0.0001f;
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
        Instantiate(crackedVersion, transform.position, Quaternion.identity);
        if(DecideToSpawnItem() && itemToSpawn != null)
        {
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
