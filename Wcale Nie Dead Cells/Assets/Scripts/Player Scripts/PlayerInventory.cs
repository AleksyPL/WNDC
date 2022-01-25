using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Player))]
public class PlayerInventory : MonoBehaviour
{
    internal Player mainPlayerScript;
    public List<Weapon> weaponsOrder;
    public List<Weapon> weapons;
    public List<int> weaponsAmount;
    private void OnEnable()
    {
        mainPlayerScript = GetComponent<Player>();
        CheckWeaponsNumbers();
        SortWeapons();
    }
    private void CheckWeaponsNumbers()
    {
        if (weapons.Count != 0 && (weaponsAmount.Count < weapons.Count || weaponsAmount.Count > weapons.Count))
        {
            weaponsAmount.Clear();
            for (int i = 0; i < weapons.Count; i++)
            {
                weaponsAmount.Add(1);
            }
        }
        else if (weaponsAmount.Count == weapons.Count)
        {
            for (int i = 0; i < weaponsAmount.Count; i++)
            {
                if (weaponsAmount[i] == 0)
                {
                    weaponsAmount[i] = 1;
                }
                if (weaponsAmount[i] > 2)
                {
                    weaponsAmount[i] = 2;
                }
                if (weapons[i].DualWeildingShootingStyle == Weapon.DualWeildingShooting.notAvailable && weaponsAmount[i] == 2)
                {
                    weaponsAmount[i] = 1;
                }
            }
        }
    }
    private void AddWeapon(Weapon newWeapon)
    {
        if(!weapons.Contains(newWeapon))
        {
            weapons.Add(newWeapon);
            weaponsAmount.Add(1);
            if (weapons.Count == 1)
            {
                mainPlayerScript.baseMovementScript.shootingScript1.SetParents();
            }
            SortWeapons();
        }
        else
        {
            for(int i=0;i<weapons.Count; i++)
            {
                if(weapons[i]==newWeapon && weapons[i].DualWeildingShootingStyle != Weapon.DualWeildingShooting.notAvailable && weaponsAmount[i] == 1)
                {
                    weaponsAmount[i]++;
                }
            }
        }
    }
    private void SortWeapons()
    {
        List<Weapon> weaponsTemp = new List<Weapon>();
        List<int> weaponsAmountTemp = new List<int>();
        int indexTemp;
        for (int i = 0; i < weaponsOrder.Count; i++)
        {
            if (weapons.Contains(weaponsOrder[i]))
            {
                indexTemp = 0;
                for (int j = 0; j < weapons.Count; j++)
                {
                    if (weapons[j] == weaponsOrder[i])
                    {
                        indexTemp = j;
                        break;
                    }
                }
                weaponsTemp.Add(weaponsOrder[i]);
                weaponsAmountTemp.Add(weaponsAmount[indexTemp]);
            }
        }
        weapons = weaponsTemp;
        weaponsAmount = weaponsAmountTemp;
    }
}
