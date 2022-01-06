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
        Bottle,
        Old_Shotgun,
        BreakOfDawn,
        Flamethrower,
        StabbySword,
        Breaketh
    }
    public WeaponTypes weaponType;

    public GameObject spawnedPrefab1;
    public GameObject spawnedPrefab2;
    public float coolDown;

    public bool isMelee = false;

    public float destroyAfterTime;

    public float curAttackTime;

    public bool isWeaponSwitchBullet;

    public InventoryChip chip;

    public Ability1Base ab1;


    private void OnEnable()
    {
        chip = null;
        curAttackTime = 0;
    }
    


}
