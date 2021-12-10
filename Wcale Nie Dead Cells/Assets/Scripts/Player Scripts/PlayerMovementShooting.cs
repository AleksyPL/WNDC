using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementShooting : MonoBehaviour
{
    //[SerializeField]
    //internal Movment baseMovementScript;
    public GameObject weaponHolder;
    public GameObject smallWeaponHolster;
    public GameObject bigWeaponHolster;
    public Weapon equipedWeapon;
    public Weapon previouslyEquipedWeapon;
    public GameObject bulletPrefab;
    private GameObject shootingPoint;
    private GameObject emptyCaseEjectorPoint;
    private bool isShooting;
    private bool reloadWeapon;
    private bool isReloading;
    private bool canShoot;
    private float rateOfFireCounter;
    private float reloadTimeCounter;
    private int currentMagazine;
    private void Start()
    {
        canShoot = false;
        rateOfFireCounter = 0;
        shootingPoint = weaponHolder.transform.Find("ShootingPoint").gameObject;
        emptyCaseEjectorPoint = weaponHolder.transform.Find("EmptyCaseEjectorPoint").gameObject;
        WeaponChange();
    }
    public void SwapWeapons(Weapon newWeapon)
    {
        previouslyEquipedWeapon = equipedWeapon;
        equipedWeapon = newWeapon;
        WeaponChange();
    }
    public void WeaponChange()
    {
        weaponHolder.transform.localPosition = new Vector3(equipedWeapon.weaponHolderOffset.x, equipedWeapon.weaponHolderOffset.y, 0);
        shootingPoint.transform.localPosition = new Vector3(equipedWeapon.shootingPointOffset.x, equipedWeapon.shootingPointOffset.y, 0);
        emptyCaseEjectorPoint.transform.localPosition = new Vector3(equipedWeapon.emptyCaseEjectorPointOffset.x, equipedWeapon.emptyCaseEjectorPointOffset.y, 0);
        weaponHolder.GetComponent<SpriteRenderer>().sprite = equipedWeapon.weaponSprite;
        emptyCaseEjectorPoint.GetComponent<ParticleSystemRenderer>().material = equipedWeapon.bulletCaseMaterial;
        currentMagazine = equipedWeapon.magazineSize;
        if (previouslyEquipedWeapon != null)
        {
            if(previouslyEquipedWeapon.isASmallWeapon)
            {
                smallWeaponHolster.GetComponent<SpriteRenderer>().sprite = previouslyEquipedWeapon.weaponSprite;
            }
            else
            {
                bigWeaponHolster.GetComponent<SpriteRenderer>().sprite = previouslyEquipedWeapon.weaponSprite;
            }
        }
    }
    private void ReloadWeapon()
    {
        if(currentMagazine <= 0 || currentMagazine < equipedWeapon.magazineSize)
        { 
            if(reloadTimeCounter < equipedWeapon.reloadTime)
            {
                reloadTimeCounter += Time.deltaTime;
                isReloading = true;
            }
            else if (reloadTimeCounter >= equipedWeapon.reloadTime)
            {
                rateOfFireCounter = 0;
                reloadTimeCounter = 0;
                currentMagazine = equipedWeapon.magazineSize;
                canShoot = true;
                isReloading = false;
            }
        }
    }
    private void ControlRateOfFire()
    {
        if(rateOfFireCounter < equipedWeapon.rateOfFire)
        {
            rateOfFireCounter += Time.deltaTime;
        }
        else
        {
            rateOfFireCounter = 0;
            canShoot = true;
        }
    }
    void Update()
    {
        isShooting = Input.GetButton("Fire1");
        reloadWeapon = Input.GetButton("Fire2");
        //WeaponChange();
        if (canShoot && isShooting && !isReloading && !reloadWeapon)
        {
            canShoot = false;
            currentMagazine -= 1;
            emptyCaseEjectorPoint.GetComponent<ParticleSystem>().Emit(1);
            float facingRotation = Mathf.Atan2(shootingPoint.transform.right.y, shootingPoint.transform.right.x) * Mathf.Rad2Deg;
            float startRotation = facingRotation + equipedWeapon.spreadOfMultiProjectileWeapons / 2f;
            float angleIncrease = equipedWeapon.spreadOfMultiProjectileWeapons / (float)equipedWeapon.numberOfProjectilesPerShot - 1f;
            for(int i=0; i < equipedWeapon.numberOfProjectilesPerShot; i++)
            {
                float tempRotation = startRotation - angleIncrease * i;
                GameObject myBullet = Instantiate(bulletPrefab, shootingPoint.transform.position, Quaternion.Euler(0f, 0f, tempRotation));
                myBullet.GetComponent<Bullet>().Setup(new Vector2(Mathf.Cos(tempRotation * Mathf.Deg2Rad), Mathf.Sin(tempRotation * Mathf.Deg2Rad)),equipedWeapon.damage, equipedWeapon.bulletLifeTime, equipedWeapon.bulletSpeed);
            }
        }
        else if (canShoot && ((reloadWeapon && !isReloading) || isReloading))
        {
            ReloadWeapon();
        }
        if (!canShoot)
        {
            if(currentMagazine > 0)
            {
                ControlRateOfFire();
            }
            else if (currentMagazine <= 0)
            {
                ReloadWeapon();
            }
        }
    }
}
