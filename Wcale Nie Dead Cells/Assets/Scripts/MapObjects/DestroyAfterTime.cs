using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifeTime;
    private float lifeTimeCounter;
    void Start()
    {
        this.lifeTimeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.lifeTimeCounter<lifeTime)
        {
            this.lifeTimeCounter += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
