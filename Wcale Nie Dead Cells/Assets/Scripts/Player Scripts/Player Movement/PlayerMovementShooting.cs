using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementBase))]
public class PlayerMovementShooting : MonoBehaviour
{
    internal PlayerMovementBase baseMovementScript;
    public GameObject bulletPrefab;
    private GameObject uziGameObject;
    private GameObject pistolGameObject;
    private GameObject rifleGameObject;
    private GameObject shotgunGameObject;
    private GameObject mp5GameObject;
    private GameObject grenadeLauncherGameObject;
    private GameObject railGunGameObject;
    private GameObject activeWeapon;
    private GameObject smallWeaponHolster;
    private GameObject bigWeaponHolster;
    private bool isShooting;
    private bool reloadWeapon;
    private bool isReloading;
    private bool enableMuzzleFlash;
    private float rateOfFireCounter;
    private float reloadTimeCounter;
    private float muzzleFlashLifeTime;
    private float muzzleFalshLifeTimeCounter;
    private int currentMagazine;
    private void Start()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
        reloadTimeCounter = 0;
        rateOfFireCounter = 0;
        muzzleFlashLifeTime = 0.15f;
        muzzleFalshLifeTimeCounter = 0;
        uziGameObject = GameObject.Find("Uzi").gameObject;
        pistolGameObject = GameObject.Find("Pistol").gameObject;
        rifleGameObject = GameObject.Find("Rifle").gameObject;
        shotgunGameObject = GameObject.Find("Shotgun").gameObject;
        mp5GameObject = GameObject.Find("MP5").gameObject;
        grenadeLauncherGameObject = GameObject.Find("GrenadeLauncher").gameObject;
        railGunGameObject = GameObject.Find("RailGun").gameObject;
        smallWeaponHolster = GameObject.Find("SmallWeaponHolster").gameObject;
        bigWeaponHolster = GameObject.Find("BigWeaponHolster").gameObject;
        EquipWeapon();
    }
    internal void EquipWeapon()
    {
        isReloading = false;
        reloadTimeCounter = 0;
        if(activeWeapon != null)
        {
            activeWeapon.GetComponent<SpriteRenderer>().enabled = false;
            activeWeapon.transform.Find("MuzzleFlashPoint").gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (baseMovementScript.mainPlayerScript.inventory.weapons[0].name == "Shotgun")
        {
            activeWeapon = shotgunGameObject;
        }
        else if (baseMovementScript.mainPlayerScript.inventory.weapons[0].name == "Rifle")
        {
            activeWeapon = rifleGameObject;
        }
        else if (baseMovementScript.mainPlayerScript.inventory.weapons[0].name == "Uzi")
        {
            activeWeapon = uziGameObject;
        }
        else if (baseMovementScript.mainPlayerScript.inventory.weapons[0].name == "Pistol")
        {
            activeWeapon = pistolGameObject;
        }
        else if (baseMovementScript.mainPlayerScript.inventory.weapons[0].name == "MP5")
        {
            activeWeapon = mp5GameObject;
        }
        else if (baseMovementScript.mainPlayerScript.inventory.weapons[0].name == "GrenadeLauncher")
        {
            activeWeapon = grenadeLauncherGameObject;
        }
        else if (baseMovementScript.mainPlayerScript.inventory.weapons[0].name == "RailGun")
        {
            activeWeapon = railGunGameObject;
        }
        if (activeWeapon != null)
        {
            activeWeapon.GetComponent<SpriteRenderer>().enabled = true;
        }
        currentMagazine = baseMovementScript.mainPlayerScript.inventory.weapons[0].magazineSize;
        if (baseMovementScript.mainPlayerScript.inventory.weapons.Count == 2 && baseMovementScript.mainPlayerScript.inventory.weapons[1] != null)
        {
            if (baseMovementScript.mainPlayerScript.inventory.weapons[1].isOneHandedWeapon)
            {
                smallWeaponHolster.GetComponent<SpriteRenderer>().sprite = baseMovementScript.mainPlayerScript.inventory.weapons[1].weaponSprite;
            }
            else
            {
                bigWeaponHolster.GetComponent<SpriteRenderer>().sprite = baseMovementScript.mainPlayerScript.inventory.weapons[1].weaponSprite;
            }
            ClearHolsters();
        }
    }
    internal void ClearHolsters()
    {
        if (baseMovementScript.mainPlayerScript.inventory.weapons[0].isOneHandedWeapon && !baseMovementScript.mainPlayerScript.inventory.weapons[1].isOneHandedWeapon)
        {
            smallWeaponHolster.GetComponent<SpriteRenderer>().sprite = null;
        }
        else if (!baseMovementScript.mainPlayerScript.inventory.weapons[0].isOneHandedWeapon && baseMovementScript.mainPlayerScript.inventory.weapons[1].isOneHandedWeapon)
        {
            bigWeaponHolster.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
    private void ReloadWeapon()
    {
        if (currentMagazine <= 0 || currentMagazine < baseMovementScript.mainPlayerScript.inventory.weapons[0].magazineSize)
        {
            if (reloadTimeCounter < baseMovementScript.mainPlayerScript.inventory.weapons[0].reloadTime)
            {
                reloadTimeCounter += Time.deltaTime;
                isReloading = true;
            }
            else if (reloadTimeCounter >= baseMovementScript.mainPlayerScript.inventory.weapons[0].reloadTime)
            {
                rateOfFireCounter = 0;
                reloadTimeCounter = 0;
                currentMagazine = baseMovementScript.mainPlayerScript.inventory.weapons[0].magazineSize;
                baseMovementScript.canShoot = true;
                isReloading = false;
            }
        }
    }
    //private void RotateGun()
    //{
    //    Vector3 difference = baseMovementScript.inputScript.mousePosition - weaponHolder.transform.position;
    //    float rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
    //    if (baseMovementScript.surroundingsCheckerScript.isFacingRight)
    //    {
    //        weaponHolder.transform.rotation = Quaternion.Euler(0, 0, rotation);
    //    }
    //    else
    //    {
    //        weaponHolder.transform.rotation = Quaternion.Euler(180, 0, -rotation);
    //    }
    //}
    private void ControlRateOfFire()
    {
        if (rateOfFireCounter < baseMovementScript.mainPlayerScript.inventory.weapons[0].rateOfFire)
        {
            rateOfFireCounter += Time.deltaTime;
        }
        else
        {
            rateOfFireCounter = 0;
            baseMovementScript.canShoot = true;
        }
    }
    void Update()
    {
        GameObject shootingPoint = activeWeapon.transform.Find("ShootingPoint").gameObject;
        GameObject emptyCaseEjectorPoint = activeWeapon.transform.Find("EmptyCaseEjectorPoint").gameObject;
        GameObject muzzleFlashPoint = activeWeapon.transform.Find("MuzzleFlashPoint").gameObject;
        isShooting = Input.GetButton("Fire1");
        reloadWeapon = Input.GetButton("Reload");
        if (baseMovementScript.canShoot && isShooting && !isReloading && !reloadWeapon)
        {
            enableMuzzleFlash = true;
            muzzleFlashPoint.GetComponent<SpriteRenderer>().enabled = true;
            baseMovementScript.canShoot = false;
            currentMagazine -= 1;
            if(baseMovementScript.surroundingsCheckerScript.isFacingRight)
            {
                emptyCaseEjectorPoint.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                emptyCaseEjectorPoint.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            emptyCaseEjectorPoint.GetComponent<ParticleSystem>().Emit(1);
            float facingRotation = Mathf.Atan2(shootingPoint.transform.right.y, shootingPoint.transform.right.x) * Mathf.Rad2Deg;
            float startRotation = facingRotation + baseMovementScript.mainPlayerScript.inventory.weapons[0].spreadOfMultiProjectileWeapons / 2f;
            float angleIncrease = baseMovementScript.mainPlayerScript.inventory.weapons[0].spreadOfMultiProjectileWeapons / (float)baseMovementScript.mainPlayerScript.inventory.weapons[0].numberOfProjectilesPerShot - 1f;
            for (int i = 0; i < baseMovementScript.mainPlayerScript.inventory.weapons[0].numberOfProjectilesPerShot; i++)
            {
                float tempRotation = startRotation - angleIncrease * i;
                GameObject myBullet = Instantiate(bulletPrefab, shootingPoint.transform.position, Quaternion.Euler(0f, 0f, tempRotation));
                myBullet.GetComponent<Bullet>().Setup(new Vector2(Mathf.Cos(tempRotation * Mathf.Deg2Rad), Mathf.Sin(tempRotation * Mathf.Deg2Rad)),
                baseMovementScript.mainPlayerScript.inventory.weapons[0].damage, baseMovementScript.mainPlayerScript.inventory.weapons[0].bulletLifeTime, baseMovementScript.mainPlayerScript.inventory.weapons[0].bulletSpeed);
            }
        }
        else if (baseMovementScript.canShoot && ((reloadWeapon && !isReloading) || isReloading))
        {
            ReloadWeapon();
        }
        if (!baseMovementScript.canShoot)
        {
            if (currentMagazine > 0)
            {
                ControlRateOfFire();
            }
            else if (currentMagazine <= 0)
            {
                ReloadWeapon();
            }
        }
        if (enableMuzzleFlash)
        {
            muzzleFalshLifeTimeCounter += Time.deltaTime;
            if (muzzleFalshLifeTimeCounter >= muzzleFlashLifeTime)
            {
                muzzleFlashPoint.GetComponent<SpriteRenderer>().enabled = false;
                muzzleFalshLifeTimeCounter = 0;
                enableMuzzleFlash = false;
            }
        }
    }
}
