using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryWeapons", menuName = "CustomObject/InventoryItems/InvetoryWeapons")]
public class InventoryWeapon : InventoryObjects
{
    public enum WeaponTypes
    {
        Vol,
        Magnetized_Shifter,
        Drone_Controller,
        Gaeas_Touch,
        Rainy_Day,
        Flash_Light,
        Bottle
    }
    public WeaponTypes weaponType;

    public GameObject spawnedPrefab1;
    public GameObject spawnedPrefab2;
    public float coolDown;

    public float destroyAfterTime;

    public int CDIndex;

    public bool isWeaponSwitchBullet;
    
}
