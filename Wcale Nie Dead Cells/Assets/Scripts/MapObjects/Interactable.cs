using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Player player;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public virtual void InteractWithPlayer()
    {

    }
}
