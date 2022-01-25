using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerNewShooting : MonoBehaviour
{
    //public enum DualWeildingShooting
    //{
    //    alternately,
    //    simultaneously
    //}
    [Header("Hands")]
    internal bool aimWithLeftArm;
    internal bool aimWithRightArm;
    internal int activeWeaponIndex;
    internal PlayerMovementBase baseMovementScript;
    public GameObject neutralWeaponHolder;
    public GameObject rightHandWeaponHolder;
    public GameObject leftHandWeaponHolder;
    public GameObject rightHand;
    public GameObject leftHand;
    [Header("General")]
    //public DualWeildingShooting DualWeildingShootingStyle;
    private bool dualWeildingFiredFromRightSide;
    private bool isReloading;
    private float reloadCounter;
    private float rateOfFireCounter;
    private float currentMagazine;
    private float rateOfFireInBurstCounter;
    private float bulletsFiredInBurst;
    //private bool canFireInBurst;
    void Start()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
        rateOfFireCounter = 0;
        currentMagazine = 0;
        reloadCounter = 0;
        rateOfFireInBurstCounter = 0;
        bulletsFiredInBurst = 0;
        dualWeildingFiredFromRightSide = false;
        SetParents();
    }
    void Update()
    {
        if((baseMovementScript.inputScript.rPressed && currentMagazine != baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].magazineSize * baseMovementScript.mainPlayerScript.inventory.weaponsAmount[activeWeaponIndex]) || currentMagazine <= 0)
        {
            ReloadWeapon();
        }
        else if(!isReloading && baseMovementScript.canShoot && currentMagazine > 0)
        {
            if((baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeFireMode == Weapon.FireMode.single && baseMovementScript.inputScript.LMB_pressed)
                || (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeFireMode == Weapon.FireMode.auto && baseMovementScript.inputScript.LMB_hold)
                || (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeFireMode == Weapon.FireMode.burst && bulletsFiredInBurst == 0 && baseMovementScript.inputScript.LMB_pressed)
                || (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeFireMode == Weapon.FireMode.burst && bulletsFiredInBurst > 0 && bulletsFiredInBurst < baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].numberOfShotsInBurst))
            {
                if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].DualWeildingShootingStyle == Weapon.DualWeildingShooting.alternately && baseMovementScript.mainPlayerScript.inventory.weaponsAmount[activeWeaponIndex] == 2)
                {
                    if(dualWeildingFiredFromRightSide)
                        Shoot(leftHandWeaponHolder);
                    else
                        Shoot(rightHandWeaponHolder);
                }
                else if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].DualWeildingShootingStyle == Weapon.DualWeildingShooting.simultaneously || baseMovementScript.mainPlayerScript.inventory.weaponsAmount[activeWeaponIndex] == 1)
                {
                    if (aimWithLeftArm)
                        Shoot(leftHandWeaponHolder);
                    if (aimWithRightArm)
                        Shoot(rightHandWeaponHolder);
                }
            }
        }
        ControlRateOfFire();
    }
    internal void SetParents()
    {
        aimWithRightArm = false;
        aimWithLeftArm = false;
        rateOfFireCounter = 0;
        currentMagazine = 0;
        isReloading = false;
        if (baseMovementScript.mainPlayerScript.inventory.weapons.Count != 0)
        {
            if (baseMovementScript.mainPlayerScript.inventory.weaponsAmount[activeWeaponIndex] == 1)
            {
                rightHandWeaponHolder.transform.SetParent(rightHand.transform);
                aimWithRightArm = true;
                EquipWeapon(rightHandWeaponHolder);
                UnequipWeapon(leftHandWeaponHolder);
            }
            else if (baseMovementScript.mainPlayerScript.inventory.weaponsAmount[activeWeaponIndex] == 2)
            {
                rightHandWeaponHolder.transform.SetParent(rightHand.transform);
                aimWithRightArm = true;
                EquipWeapon(rightHandWeaponHolder);
                leftHandWeaponHolder.transform.SetParent(leftHand.transform);
                aimWithLeftArm = true;
                EquipWeapon(leftHandWeaponHolder);
            }
        }
        else
        {
            activeWeaponIndex = -1;
        }
    }
    private void EquipWeapon(GameObject weaponHolder)
    {
        if(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeWeaponType == Weapon.WeaponType.firearm)
        {
            //weapon sprite and grip
            weaponHolder.transform.localPosition = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].localOffsetGrip;
            weaponHolder.GetComponent<SpriteRenderer>().sprite = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].weaponSprite;
            weaponHolder.transform.localRotation = Quaternion.identity;
            //shooting point
            GameObject shootingPoint = weaponHolder.transform.Find("ShootingPoint").gameObject;
            shootingPoint.transform.localPosition = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].localOffsetShootingPoint;
            //empty case ejection point
            GameObject pistolCaseEjectionPoint = weaponHolder.transform.Find("EmptyCasePistol").gameObject;
            pistolCaseEjectionPoint.transform.localPosition = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].localOffsetCaseEjector;
            GameObject rifleCaseEjectionPoint = weaponHolder.transform.Find("EmptyCaseRifle").gameObject;
            rifleCaseEjectionPoint.transform.localPosition = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].localOffsetCaseEjector;
            GameObject batteryCaseEjectionPoint = weaponHolder.transform.Find("EmptyCaseBattery").gameObject;
            batteryCaseEjectionPoint.transform.localPosition = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].localOffsetCaseEjector;
            GameObject shotgunCaseEjectionPoint = weaponHolder.transform.Find("EmptyCaseShotgun").gameObject;
            shotgunCaseEjectionPoint.transform.localPosition = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].localOffsetCaseEjector;
            GameObject grenadeLauncherCaseEjectionPoint = weaponHolder.transform.Find("EmptyCaseGrenadeLauncher").gameObject;
            grenadeLauncherCaseEjectionPoint.transform.localPosition = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].localOffsetCaseEjector;
            currentMagazine = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].magazineSize * baseMovementScript.mainPlayerScript.inventory.weaponsAmount[activeWeaponIndex];
        }
        else if(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeWeaponType == Weapon.WeaponType.melee || baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeWeaponType == Weapon.WeaponType.grenade)
        {
            //weapon sprite and grip
            weaponHolder.transform.localPosition = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].localOffsetGrip;
            weaponHolder.GetComponent<SpriteRenderer>().sprite = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].weaponSprite;
            weaponHolder.transform.localRotation = Quaternion.identity;
        }
    }
    private void UnequipWeapon(GameObject weaponHolder)
    {
        weaponHolder.transform.SetParent(neutralWeaponHolder.transform);
        //weapon sprite and grip
        weaponHolder.transform.localPosition = Vector3.zero;
        weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
        //shooting point
        GameObject shootingPoint = weaponHolder.transform.Find("ShootingPoint").gameObject;
        shootingPoint.transform.localPosition = Vector3.zero;
        //empty case ejection point
        GameObject pistolCaseEjectionPoint = weaponHolder.transform.Find("EmptyCasePistol").gameObject;
        pistolCaseEjectionPoint.transform.localPosition = Vector3.zero;
        GameObject rifleCaseEjectionPoint = weaponHolder.transform.Find("EmptyCaseRifle").gameObject;
        rifleCaseEjectionPoint.transform.localPosition = Vector3.zero;
        GameObject batteryCaseEjectionPoint = weaponHolder.transform.Find("EmptyCaseBattery").gameObject;
        batteryCaseEjectionPoint.transform.localPosition = Vector3.zero;
        GameObject shotgunCaseEjectionPoint = weaponHolder.transform.Find("EmptyCaseShotgun").gameObject;
        shotgunCaseEjectionPoint.transform.localPosition = Vector3.zero;
        GameObject grenadeLauncherCaseEjectionPoint = weaponHolder.transform.Find("EmptyCaseGrenadeLauncher").gameObject;
        grenadeLauncherCaseEjectionPoint.transform.localPosition = Vector3.zero;
    }
    internal void ChangeActiveWeapon()
    {
        if(activeWeaponIndex != -1 && baseMovementScript.inputScript.scrollwheel !=0)
        {
            if (baseMovementScript.inputScript.scrollwheel < 0)
            {
                if (activeWeaponIndex < baseMovementScript.mainPlayerScript.inventory.weapons.Count - 1)
                {
                    activeWeaponIndex++;
                    SetParents();
                }
            }
            else if (baseMovementScript.inputScript.scrollwheel > 0)
            {
                if(activeWeaponIndex > 0)
                {
                    activeWeaponIndex--;
                    SetParents();
                }
            }
        }
    }
    private void RotateCaseEjectorPoint(GameObject emptyCaseEjectorPoint)
    {
        if (baseMovementScript.surroundingsCheckerScript.isFacingRight)
        {
            emptyCaseEjectorPoint.transform.rotation = Quaternion.Euler(0, 180, 0);
            //emptyCaseEjectorPoint.transform.Rotate(0, 180, 0);
        }
        else
        {
            emptyCaseEjectorPoint.transform.rotation = Quaternion.Euler(0, 180, 0);
            //emptyCaseEjectorPoint.transform.Rotate(0, 0, 0);
        }
    }
    private void ThrowCorrectEmptyCase(GameObject weaponHolder)
    {
        if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeBulletCase == Weapon.AmmoType.pistol)
        {
            GameObject pistolCaseEjectionPoint = weaponHolder.transform.Find("EmptyCasePistol").gameObject;
            RotateCaseEjectorPoint(pistolCaseEjectionPoint);
            pistolCaseEjectionPoint.GetComponent<ParticleSystem>().Emit(1);
        }
        else if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeBulletCase == Weapon.AmmoType.rifle)
        {
            GameObject rifleCaseEjectionPoint = weaponHolder.transform.Find("EmptyCaseRifle").gameObject;
            RotateCaseEjectorPoint(rifleCaseEjectionPoint);
            rifleCaseEjectionPoint.GetComponent<ParticleSystem>().Emit(1);
        }
        else if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeBulletCase == Weapon.AmmoType.shotgun)
        {
            GameObject shotgunCaseEjectionPoint = weaponHolder.transform.Find("EmptyCaseShotgun").gameObject;
            RotateCaseEjectorPoint(shotgunCaseEjectionPoint);
            shotgunCaseEjectionPoint.GetComponent<ParticleSystem>().Emit(1);
        }
        else if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeBulletCase == Weapon.AmmoType.battery)
        {
            GameObject batteryCaseEjectionPoint = weaponHolder.transform.Find("EmptyCaseBattery").gameObject;
            RotateCaseEjectorPoint(batteryCaseEjectionPoint);
            batteryCaseEjectionPoint.GetComponent<ParticleSystem>().Emit(1);
        }
        if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeBulletCase == Weapon.AmmoType.grenadeLauncher)
        {
            GameObject grenadeLauncherCaseEjectionPoint = weaponHolder.transform.Find("EmptyCaseGrenadeLauncher").gameObject;
            RotateCaseEjectorPoint(grenadeLauncherCaseEjectionPoint);
            grenadeLauncherCaseEjectionPoint.GetComponent<ParticleSystem>().Emit(1);
        }
    }
    private void Shoot(GameObject weaponHolder)
    {
        if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeProjectileMode == Weapon.ProjectileMode.laser)
        {
            GameObject shootingPoint = weaponHolder.transform.Find("ShootingPoint").gameObject;
            //TODO Rail Gun
        }
        else if(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeProjectileMode == Weapon.ProjectileMode.multiple)
        {
            GameObject shootingPoint = weaponHolder.transform.Find("ShootingPoint").gameObject;
            float facingRotation = Mathf.Atan2(shootingPoint.transform.right.y, shootingPoint.transform.right.x) * Mathf.Rad2Deg;
            float startRotation = facingRotation + baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].spreadOfMultiProjectileWeapons / 2f;
            float angleIncrease = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].spreadOfMultiProjectileWeapons / (float)baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].numberOfProjectilesPerShot - 1f;
            for (int j = 0; j < baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].numberOfProjectilesPerShot; j++)
            {
                float tempRotation = startRotation - angleIncrease * j;
                Instantiate(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].muzzleFlash, shootingPoint.transform.position, Quaternion.identity);
                GameObject myBullet = Instantiate(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].projectilePrefab, shootingPoint.transform.position, Quaternion.Euler(0f, 0f, tempRotation));
                myBullet.GetComponent<Bullet>().Setup(new Vector2(Mathf.Cos(tempRotation * Mathf.Deg2Rad), Mathf.Sin(tempRotation * Mathf.Deg2Rad)),
                baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].damage, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletLifeTime, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletSpeed);
            }
        }
        else if(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeProjectileMode == Weapon.ProjectileMode.explosive)
        {
            GameObject shootingPoint = weaponHolder.transform.Find("ShootingPoint").gameObject;
            //TODO Grenade Launcher
        }
        else if(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeProjectileMode == Weapon.ProjectileMode.single)
        {
            GameObject shootingPoint = weaponHolder.transform.Find("ShootingPoint").gameObject;
            Instantiate(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].muzzleFlash, shootingPoint.transform.position, weaponHolder.transform.rotation);
            GameObject myBullet = Instantiate(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].projectilePrefab, shootingPoint.transform.position, weaponHolder.transform.rotation);
            myBullet.GetComponent<Bullet>().Setup(shootingPoint.transform.right, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].damage,
                baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletLifeTime, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletSpeed);
        }
        baseMovementScript.canShoot = false;
        ThrowCorrectEmptyCase(weaponHolder);
        currentMagazine--;
        if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeFireMode == Weapon.FireMode.burst)
            bulletsFiredInBurst++;
    }
    private void ReloadWeapon()
    {
        if (!isReloading)
        {
            currentMagazine = 0;
            isReloading = true;
            baseMovementScript.canShoot = false;
        }
        if(isReloading)
        {
            reloadCounter += Time.deltaTime;
            if (reloadCounter >= baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].reloadTime * baseMovementScript.mainPlayerScript.inventory.weaponsAmount[activeWeaponIndex])
            {
                reloadCounter = 0;
                isReloading = false;
                baseMovementScript.canShoot = true;
                currentMagazine = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].magazineSize * baseMovementScript.mainPlayerScript.inventory.weaponsAmount[activeWeaponIndex];
            }
        }
    }
    private void ControlRateOfFire()
    {
        if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeFireMode == Weapon.FireMode.burst && bulletsFiredInBurst < baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].numberOfShotsInBurst)
        {
            //if(bulletsFiredInBurst < baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].numberOfShotsInBurst)
            //{
            //    baseMovementScript.canShoot = false;
            //    if (rateOfFireInBurstCounter < baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].rateOfFireInBurst && !canFireInBurst)
            //    {
            //        rateOfFireInBurstCounter += Time.deltaTime;
            //    }
            //    else if(rateOfFireInBurstCounter >= baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].rateOfFireInBurst && !canFireInBurst)
            //    {
            //        canFireInBurst = true;
            //        rateOfFireInBurstCounter = 0;
            //    }
            //}
            //if (bulletsFiredInBurst >= baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].numberOfShotsInBurst)
            //{
            //    canFireInBurst = false;
            //    if (rateOfFireCounter < baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].rateOfFire / baseMovementScript.mainPlayerScript.inventory.weaponsAmount[activeWeaponIndex] && !baseMovementScript.canShoot)
            //    {
            //        rateOfFireCounter += Time.deltaTime;
            //    }
            //    else if (rateOfFireCounter >= baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].rateOfFire / baseMovementScript.mainPlayerScript.inventory.weaponsAmount[activeWeaponIndex] && !baseMovementScript.canShoot)
            //    {
            //        rateOfFireCounter = 0;
            //        baseMovementScript.canShoot = true;
            //        canFireInBurst = true;
            //        if (DualWeildingShootingStyle == DualWeildingShooting.alternately && baseMovementScript.mainPlayerScript.inventory.weaponsAmount[activeWeaponIndex] == 2)
            //            dualWeildingFiredFromRightSide = !dualWeildingFiredFromRightSide;
            //    }
            //}
            //baseMovementScript.canShoot = false;
            if (rateOfFireInBurstCounter < baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].rateOfFireInBurst && !baseMovementScript.canShoot)
            {
                rateOfFireInBurstCounter += Time.deltaTime;
            }
            else if (rateOfFireInBurstCounter >= baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].rateOfFireInBurst && !baseMovementScript.canShoot)
            {
                baseMovementScript.canShoot = true;
                rateOfFireInBurstCounter = 0;
            }
        }
        else if ((baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeFireMode == Weapon.FireMode.burst && bulletsFiredInBurst >= baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].numberOfShotsInBurst) || baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeFireMode != Weapon.FireMode.burst)
        {
            //baseMovementScript.canShoot = false;
            if (rateOfFireCounter < baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].rateOfFire / baseMovementScript.mainPlayerScript.inventory.weaponsAmount[activeWeaponIndex] && !baseMovementScript.canShoot)
            {
                rateOfFireCounter += Time.deltaTime;
            }
            else if (rateOfFireCounter >= baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].rateOfFire / baseMovementScript.mainPlayerScript.inventory.weaponsAmount[activeWeaponIndex] && !baseMovementScript.canShoot)
            {
                rateOfFireCounter = 0;
                baseMovementScript.canShoot = true;
                if(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].DualWeildingShootingStyle == Weapon.DualWeildingShooting.alternately && baseMovementScript.mainPlayerScript.inventory.weaponsAmount[activeWeaponIndex] == 2)
                    dualWeildingFiredFromRightSide = !dualWeildingFiredFromRightSide;
                if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeFireMode == Weapon.FireMode.burst)
                    bulletsFiredInBurst = 0;
            }
        }
    }
}
