using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Weapon : ScriptableObject
{
    public Sprite weaponSprite;
    public GameObject muzzleFlash;
    public enum WeaponType
    {
        firearm,
        melee,
        grenade
    }
    public enum FireMode
    {
        single,
        auto,
        burst
    }
    public enum ProjectileMode
    {
        single,
        multiple,
        explosive,
        laser
    }
    public enum HitDetectionMode
    {
        projectile,
        hitScan
    }
    public enum HitscanPenetrationMode
    {
        firstHit,
        fullPenetration
    }
    public enum FittingHolster
    {
        small,
        big
    }
    public enum AmmoType
    {
        pistol,
        rifle,
        shotgun,
        battery,
        grenadeLauncher
    }
    public enum DualWeildingShooting
    {
        notAvailable,
        alternately,
        simultaneously
    }
    public WeaponType activeWeaponType;
    public FireMode activeFireMode;
    public ProjectileMode activeProjectileMode;
    public HitDetectionMode activeHitDetectionMode;
    public HitscanPenetrationMode activeHitscanPenetrationMode;
    public FittingHolster activeHolster;
    public AmmoType activeBulletCase;
    public DualWeildingShooting DualWeildingShootingStyle;
    public Vector2 localOffsetGrip;
    public Quaternion localGripRotation;
    public Vector2 localOffsetShootingPoint;
    public Vector2 localOffsetMuzzleFlashPoint;
    public Vector2 localOffsetCaseEjector;
    //public bool dualWieldingAvailable;
    //public bool isOneHandedWeapon;
    public float damage;
    public float bulletSpeed;
    public float bulletLifeTime;
    public float rateOfFire;
    public float reloadTime;
    public int magazineSize;
    public GameObject projectilePrefab;
    [Header("Shotgun Only")]
    public int numberOfProjectilesPerShot;
    public float spreadOfMultiProjectileWeapons;
    [Header("Burst Only")]
    public int numberOfShotsInBurst;
    public float rateOfFireInBurst;
}
