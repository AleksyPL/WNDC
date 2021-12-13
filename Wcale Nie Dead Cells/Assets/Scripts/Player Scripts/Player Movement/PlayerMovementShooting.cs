using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementBase))]
public class PlayerMovementShooting : MonoBehaviour
{
    private PlayerMovementBase baseMovementScript;
    public GameObject weaponHolder;
    public GameObject smallWeaponHolster;
    public GameObject bigWeaponHolster;
    //public Weapon equipedWeapon;
    //public Weapon secondWeapon;
    public GameObject bulletPrefab;
    private GameObject shootingPoint;
    private GameObject emptyCaseEjectorPoint;
    private GameObject muzzleFlashPoint;
    private GameObject hand1Object;
    private GameObject hand2Object;
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
        shootingPoint = weaponHolder.transform.Find("ShootingPoint").gameObject;
        emptyCaseEjectorPoint = weaponHolder.transform.Find("EmptyCaseEjectorPoint").gameObject;
        muzzleFlashPoint = weaponHolder.transform.Find("MuzzleFlashPoint").gameObject;
        hand1Object = weaponHolder.transform.Find("Hand1").gameObject;
        hand2Object = weaponHolder.transform.Find("Hand2").gameObject;
        EquipWeapon();
    }
    internal void EquipWeapon()
    {
        hand1Object.GetComponent<SpriteRenderer>().enabled = false;
        hand2Object.GetComponent<SpriteRenderer>().enabled = false;
        isReloading = false;
        reloadTimeCounter = 0;
        weaponHolder.transform.localPosition = new Vector3(baseMovementScript.mainPlayerScript.inventory.weapons[0].weaponHolderOffset.x, baseMovementScript.mainPlayerScript.inventory.weapons[0].weaponHolderOffset.y, 0);
        shootingPoint.transform.localPosition = new Vector3(baseMovementScript.mainPlayerScript.inventory.weapons[0].shootingPointOffset.x, baseMovementScript.mainPlayerScript.inventory.weapons[0].shootingPointOffset.y, 0);
        emptyCaseEjectorPoint.transform.localPosition = new Vector3(baseMovementScript.mainPlayerScript.inventory.weapons[0].emptyCaseEjectorPointOffset.x, baseMovementScript.mainPlayerScript.inventory.weapons[0].emptyCaseEjectorPointOffset.y, 0);
        muzzleFlashPoint.transform.localPosition = new Vector3(baseMovementScript.mainPlayerScript.inventory.weapons[0].muzzleFlashPointOffset.x, baseMovementScript.mainPlayerScript.inventory.weapons[0].muzzleFlashPointOffset.y, 0);
        weaponHolder.GetComponent<SpriteRenderer>().sprite = baseMovementScript.mainPlayerScript.inventory.weapons[0].weaponSprite;
        emptyCaseEjectorPoint.GetComponent<ParticleSystemRenderer>().material = baseMovementScript.mainPlayerScript.inventory.weapons[0].bulletCaseMaterial;
        currentMagazine = baseMovementScript.mainPlayerScript.inventory.weapons[0].magazineSize;
        if (baseMovementScript.mainPlayerScript.inventory.weapons[0].isOneHandedWeapon)
        {
            hand1Object.GetComponent<SpriteRenderer>().enabled = true;
            hand1Object.transform.localPosition = baseMovementScript.mainPlayerScript.inventory.weapons[0].Hand1Position;
        }
        else
        {
            hand1Object.GetComponent<SpriteRenderer>().enabled = true;
            hand2Object.GetComponent<SpriteRenderer>().enabled = true;
            hand1Object.transform.localPosition = baseMovementScript.mainPlayerScript.inventory.weapons[0].Hand1Position;
            hand2Object.transform.localPosition = baseMovementScript.mainPlayerScript.inventory.weapons[0].Hand2Position;
        }
        if (baseMovementScript.mainPlayerScript.inventory.weapons.Length == 2 && baseMovementScript.mainPlayerScript.inventory.weapons[1] != null)
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
    private void RotateGun()
    {
        Vector3 difference = baseMovementScript.inputScript.mousePosition - weaponHolder.transform.position;
        float rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        if (baseMovementScript.surroundingsCheckerScript.isFacingRight)
        {
            weaponHolder.transform.rotation = Quaternion.Euler(0, 0, rotation);
        }
        else
        {
            weaponHolder.transform.rotation = Quaternion.Euler(180, 0, -rotation);
        }
    }
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
        RotateGun();
        isShooting = Input.GetButton("Fire1");
        reloadWeapon = Input.GetButton("Reload");
        if (baseMovementScript.canShoot && isShooting && !isReloading && !reloadWeapon)
        {
            enableMuzzleFlash = true;
            muzzleFlashPoint.GetComponent<SpriteRenderer>().enabled = true;
            muzzleFlashPoint.GetComponent<SpriteRenderer>().sprite = baseMovementScript.mainPlayerScript.inventory.weapons[0].muzzleFlashSprite;
            baseMovementScript.canShoot = false;
            currentMagazine -= 1;
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
