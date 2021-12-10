using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public Sprite weaponSprite;
    public Material bulletCaseMaterial;
    public Vector2 weaponHolderOffset;
    public Vector2 shootingPointOffset;
    public Vector2 emptyCaseEjectorPointOffset;
    public bool isASmallWeapon;
    public float numberOfProjectilesPerShot;
    public float spreadOfMultiProjectileWeapons;
    public float damage;
    public float bulletSpeed;
    public float bulletLifeTime;
    public float rateOfFire;
    public float reloadTime;
    public int magazineSize;
}
