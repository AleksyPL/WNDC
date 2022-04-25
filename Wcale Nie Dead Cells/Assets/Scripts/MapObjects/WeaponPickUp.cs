using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : Interactable
{
    public Weapon weaponPickUp;
    public override void InteractWithPlayer()
    {
        player.inventory.AddWeapon(weaponPickUp);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            InteractWithPlayer();
            Destroy(gameObject);
        }
    }
}
