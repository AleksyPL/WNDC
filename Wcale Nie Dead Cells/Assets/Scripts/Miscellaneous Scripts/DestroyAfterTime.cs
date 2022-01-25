using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifeTime;
    private float lifeTimeCounter;
    void Start()
    {
        lifeTimeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimeCounter += Time.deltaTime;
        if (lifeTimeCounter >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
