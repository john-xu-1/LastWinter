using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryWeapons", menuName = "CustomObject/InventoryItems/InvetoryWeapons")]
public class InventoryWeapon : InventoryObjects
{
    public enum WeaponTypes
    {
        Mechanica_Arms,
        Magnetized_Shifter,
        Drone_Controller,
        Gaeas_Touch,
        Rainy_Day,
        Flash_Light,
        Bottle
    }
    public WeaponTypes weaponType;
    public GameObject spawnedPrefab;
    public float coolDown;

    public float destroyAfterTime;

    public int CDIndex;
}
