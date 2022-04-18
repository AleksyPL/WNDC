using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    internal float lifeTime;
    private float lifeTimeCounter;
    internal float colorFadeTick;
    private float colorFadeCounter;
    private SpriteRenderer[] fadingElements;
    void Start()
    {
        lifeTimeCounter = 0;
        colorFadeCounter = 0;
        fadingElements = GetComponentsInChildren<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        lifeTimeCounter += Time.deltaTime;
        colorFadeCounter += Time.deltaTime;
        if(fadingElements.Length > 0 && colorFadeCounter >= colorFadeTick && transform.CompareTag("Enemy"))
        {
            FadeOut();
        }
        if (lifeTimeCounter >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
    private void FadeOut()
    {
        for(int i=0;i<fadingElements.Length;i++)
        {
            if(fadingElements[i].GetComponent<SpriteRenderer>().color.a > 0)
            {
                fadingElements[i].GetComponent<SpriteRenderer>().color = new Color(fadingElements[i].GetComponent<SpriteRenderer>().color.r, fadingElements[i].GetComponent<SpriteRenderer>().color.g, fadingElements[i].GetComponent<SpriteRenderer>().color.b, fadingElements[i].GetComponent<SpriteRenderer>().color.a - 1f / 255f);
            }
        }
        colorFadeCounter = 0;
    }
}
