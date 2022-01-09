using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public Sprite weaponSprite;
    public enum FireMode
    {
        single,
        auto,
        burst,
        multiple,
        laser
    }
    public FireMode activeFireMode;
    public enum Holster
    {
        small,
        big
    }
    public Holster fittingHolster;
    public bool isOneHandedWeapon;
    public float numberOfProjectilesPerShot;
    public float spreadOfMultiProjectileWeapons;
    public float damage;
    public float bulletSpeed;
    public float bulletLifeTime;
    public float rateOfFire;
    public float reloadTime;
    public int magazineSize;
}
