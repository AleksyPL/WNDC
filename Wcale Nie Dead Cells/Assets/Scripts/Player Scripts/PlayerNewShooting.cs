using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerNewShooting : MonoBehaviour
{
    [Header("Hands")]
    internal bool aimWithLeftArm;
    internal bool aimWithRightArm;
    internal int activeWeaponIndex;
    internal PlayerMovementBase baseMovementScript;
    internal DamageSystem damageSystemScript;
    public GameObject neutralWeaponHolder;
    public GameObject rightHandWeaponHolder;
    public GameObject leftHandWeaponHolder;
    public GameObject rightHand;
    public GameObject leftHand;
    //[Header("General")]
    private bool dualWeildingFiredFromRightSide;
    private bool isReloading;
    private float reloadCounter;
    private float rateOfFireCounter;
    private float currentMagazine;
    private float rateOfFireInBurstCounter;
    private float bulletsFiredInBurst;
    void Start()
    {
        baseMovementScript = GetComponent<PlayerMovementBase>();
        damageSystemScript = GetComponent<DamageSystem>();
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
            ReloadWeapon();
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
                EquipWeapon(rightHandWeaponHolder);
                UnequipWeapon(leftHandWeaponHolder);
                if(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeWeaponType == Weapon.WeaponType.firearm)
                    aimWithRightArm = true;
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
            activeWeaponIndex = -1;
    }
    private void EquipWeapon(GameObject weaponHolder)
    {
        weaponHolder.transform.localPosition = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].localOffsetGrip;
        weaponHolder.GetComponent<SpriteRenderer>().sprite = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].weaponSprite;
        weaponHolder.transform.localRotation = Quaternion.identity;
        weaponHolder.transform.localRotation = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].localGripRotation;
        if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeWeaponType == Weapon.WeaponType.firearm)
        {
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
    }
    private void UnequipWeapon(GameObject weaponHolder)
    {
        weaponHolder.transform.SetParent(neutralWeaponHolder.transform);
        //weapon sprite and grip
        weaponHolder.transform.localPosition = Vector3.zero;
        weaponHolder.transform.localRotation = Quaternion.Euler(0, 0, 0);
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
            //scroll down
            if (baseMovementScript.inputScript.scrollwheel < 0)
            {
                if (activeWeaponIndex < baseMovementScript.mainPlayerScript.inventory.weapons.Count - 1)
                    activeWeaponIndex++;
                else
                    activeWeaponIndex = 0;
                SetParents();
            }
            //scroll up
            else if (baseMovementScript.inputScript.scrollwheel > 0)
            {
                if(activeWeaponIndex > 0)
                    activeWeaponIndex--;
                else
                    activeWeaponIndex = baseMovementScript.mainPlayerScript.inventory.weapons.Count - 1;
                SetParents();
            }
        }
    }
    private void RotateCaseEjectorPoint(GameObject emptyCaseEjectorPoint)
    {
        //TODO flip the left side
        if (baseMovementScript.surroundingsCheckerScript.isFacingRight)
        {
            emptyCaseEjectorPoint.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            emptyCaseEjectorPoint.transform.rotation = Quaternion.Euler(0, 180, 0);
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
        if(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeHitDetectionMode == Weapon.HitDetectionMode.projectile)
        {
            GameObject shootingPoint = weaponHolder.transform.Find("ShootingPoint").gameObject;
            if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].muzzleFlash != null)
            {
                GameObject muzzleFlash = Instantiate(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].muzzleFlash, shootingPoint.transform.position, weaponHolder.transform.rotation);
                muzzleFlash.transform.parent = shootingPoint.gameObject.transform;
                muzzleFlash.GetComponent<DestroyAfterTime>().lifeTime = 0.2f;
            }
            if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeProjectileMode == Weapon.ProjectileMode.multiple)
            {
                float facingRotation = Mathf.Atan2(shootingPoint.transform.right.y, shootingPoint.transform.right.x) * Mathf.Rad2Deg;
                float angle = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].spreadOfMultiProjectileWeapons / baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].numberOfProjectilesPerShot;
                float startingValue = 0.5f;
                if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].numberOfProjectilesPerShot % 2 == 1)
                {
                    GameObject myBullet = Instantiate(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].projectilePrefab, shootingPoint.transform.position, Quaternion.Euler(0f, 0f, facingRotation));
                    myBullet.GetComponent<Bullet>().Setup(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].damage / baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].numberOfProjectilesPerShot, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletLifeTime, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletSpeed);
                    startingValue = 1;
                }
                for (float i = startingValue; i < baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].numberOfProjectilesPerShot / 2 + startingValue; i++)
                {
                    GameObject myBullet = Instantiate(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].projectilePrefab, shootingPoint.transform.position, Quaternion.Euler(0f, 0f, facingRotation + (i * angle)));
                    myBullet.GetComponent<Bullet>().Setup(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].damage / baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].numberOfProjectilesPerShot, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletLifeTime, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletSpeed);
                    myBullet = Instantiate(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].projectilePrefab, shootingPoint.transform.position, Quaternion.Euler(0f, 0f, facingRotation - (i * angle)));
                    myBullet.GetComponent<Bullet>().Setup(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].damage / baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].numberOfProjectilesPerShot, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletLifeTime, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletSpeed);
                }
            }
            else if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeProjectileMode == Weapon.ProjectileMode.explosive)
            {
                GameObject myBullet = Instantiate(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].projectilePrefab, shootingPoint.transform.position, weaponHolder.transform.rotation);
                myBullet.GetComponent<Grenade>().Setup(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].damage,
                    baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletLifeTime, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletSpeed, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].explosionRadius, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].explosionEffect);
            }
            else if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeProjectileMode == Weapon.ProjectileMode.single)
            {
                GameObject myBullet = Instantiate(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].projectilePrefab, shootingPoint.transform.position, weaponHolder.transform.rotation);
                myBullet.GetComponent<Bullet>().Setup(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].damage,
                    baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletLifeTime, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletSpeed);
            }
            baseMovementScript.canShoot = false;
            ThrowCorrectEmptyCase(weaponHolder);
            currentMagazine--;
            if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeFireMode == Weapon.FireMode.burst)
                bulletsFiredInBurst++;
        }
        else if(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeHitDetectionMode == Weapon.HitDetectionMode.hitScan)
        {
            GameObject shootingPoint = weaponHolder.transform.Find("ShootingPoint").gameObject;
            RaycastHit2D[] hitTableTemp = Physics2D.RaycastAll(shootingPoint.transform.position, shootingPoint.transform.right, Mathf.Infinity);
            if(hitTableTemp.Length !=0)
            {
                List<RaycastHit2D> hitTable = new List<RaycastHit2D>();
                for (int i=0;i<hitTableTemp.Length;i++)
                {
                    if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeHitscanPenetrationMode == Weapon.HitscanPenetrationMode.firstHit)
                    {
                        if (hitTableTemp[i].collider.gameObject.name != "GameRoomBoundries" || hitTableTemp[i].collider.gameObject.CompareTag("Chain"))
                            continue;
                        else
                        {
                            hitTable.Add(hitTableTemp[i]);
                            break;
                        }
                    }
                    else
                    {
                        if (hitTableTemp[i].collider.gameObject.name != "GameRoomBoundries" || hitTableTemp[i].collider.gameObject.CompareTag("Chain"))
                            continue;
                        else if (hitTableTemp[i].collider.gameObject.layer == Mathf.Log(damageSystemScript.interactablePlatforms, 2) || hitTableTemp[i].collider.gameObject.layer == Mathf.Log(damageSystemScript.nonInteractablePlatforms, 2))
                        {
                            hitTable.Add(hitTableTemp[i]);
                            break;
                        }
                        else
                            hitTable.Add(hitTableTemp[i]);
                    }
                }
                for(int i=0;i<hitTable.Count;i++)
                {
                    damageSystemScript.CheckWhatHasBeenHitBullet(hitTable[i].collider.gameObject, baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].damage, shootingPoint.transform.right, hitTable[i].point);
                }
                if(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeProjectileMode == Weapon.ProjectileMode.laser)
                {
                    GameObject myLaser = Instantiate(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].projectilePrefab, shootingPoint.transform);
                    if (myLaser.GetComponent<LineRenderer>())
                    {
                        myLaser.GetComponent<LineRenderer>().SetPosition(0, shootingPoint.transform.position);
                        myLaser.GetComponent<LineRenderer>().SetPosition(1, hitTable[hitTable.Count-1].point);
                        myLaser.GetComponent<LineRenderer>().enabled = true;
                    }
                    if (myLaser.GetComponent<DestroyAfterTime>())
                        myLaser.GetComponent<DestroyAfterTime>().lifeTime = baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].bulletLifeTime;
                }
                if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].muzzleFlash != null)
                {
                    GameObject muzzleFlash = Instantiate(baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].muzzleFlash, shootingPoint.transform.position, weaponHolder.transform.rotation);
                    muzzleFlash.transform.parent = shootingPoint.gameObject.transform;
                    muzzleFlash.GetComponent<DestroyAfterTime>().lifeTime = 0.2f;
                }
                baseMovementScript.canShoot = false;
                ThrowCorrectEmptyCase(weaponHolder);
                currentMagazine--;
                if (baseMovementScript.mainPlayerScript.inventory.weapons[activeWeaponIndex].activeFireMode == Weapon.FireMode.burst)
                    bulletsFiredInBurst++;
            }
        }
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
