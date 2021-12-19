using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public Sprite weaponSprite;
    public Sprite muzzleFlashSprite;
    public Material bulletCaseMaterial;
    public Vector2 weaponHolderOffset;
    public Vector2 shootingPointOffset;
    public Vector2 emptyCaseEjectorPointOffset;
    //public Vector2 muzzleFlashPointOffset;
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
