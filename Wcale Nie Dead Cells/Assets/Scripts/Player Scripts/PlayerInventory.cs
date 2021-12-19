using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInventory : MonoBehaviour
{
    internal Player mainPlayerScript;
    public List<Weapon> weapons;
    //public Weapon[] weapons;
    private GameObject objectToSpawn;
    private Vector2 placeToSpawnObject;
    private void Start()
    {
        mainPlayerScript = GetComponent<Player>();
        objectToSpawn = null;
        placeToSpawnObject = Vector2.zero;
        //Weapon[] weapons = new Weapon[2];
    }
    public virtual void PickUpObject()
    {
        Destroy(gameObject);
    }
    public virtual void SpawnPickUpObject()
    {
        Instantiate(objectToSpawn, placeToSpawnObject, Quaternion.identity);
    }
    internal void AddWeapon(Weapon newWeapon)
    {
        if(weapons.Count == 0)
        {
            weapons[0] = newWeapon;
        }
        else if (weapons.Count== 1 && weapons[0] != newWeapon)
        {
            weapons[1] = newWeapon;
        }
    }
    internal void SwapWeapons(Weapon newWeapon)
    {
        if(weapons.Count == 2 && weapons[1] != null)
        {
            weapons[1] = weapons[0];
            weapons[0] = newWeapon;
        }
        mainPlayerScript.baseMovementScript.shootingScript.EquipWeapon();
    }
}
