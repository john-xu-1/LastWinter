﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public InventoryObjects[] items;
    public int index;
    public InventoryObjects selectedItem;
    public SpriteRenderer SelectedRenderer;

    public InventoryWeapon iw;
    private float fireRate;
    public float[] nextAttackTimes;

    public InventoryWeapon weapon;
    public InventoryAccessory acs;

    GameObject bulletInstance;

    GameObject weaponSpawnedTargetPrefab;
    GameObject acsSpawnedTargetPrefab;

    float Zfactor;

    bool isAcsSpawned;

    public void AddItem(InventoryObjects item)
    {
        if (index <= items.Length - 1)
        {
            items[index] = item;
            index += 1;
        }
        else
        {
            Debug.Log("INVENTORY FULL");
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedItem = items[0];
            weaponSpawnedTargetPrefab = null;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedItem = items[1];
            weaponSpawnedTargetPrefab = null;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedItem = items[2];
            weaponSpawnedTargetPrefab = null;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedItem = items[3];
            weaponSpawnedTargetPrefab = null;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedItem = items[4];
            weaponSpawnedTargetPrefab = null;
        }

        if (selectedItem)
        {
            SelectedRenderer.sprite = selectedItem.itemSprite;
            
            if(selectedItem.itemType == InventoryObjects.ItemTypes.Weapon)
            {
                weapon = (InventoryWeapon) selectedItem;
                fireRate = weapon.coolDown;

                if (Input.GetButton("Fire1"))
                {
                    if (Time.time >= nextAttackTimes[weapon.CDIndex])
                    {
                        bulletInstance = Instantiate(weaponSpawnedTargetPrefab, GameObject.FindGameObjectWithTag("SelectedItem").transform.position, GameObject.FindGameObjectWithTag("SelectedItem").transform.rotation);
                        nextAttackTimes[weapon.CDIndex] = Time.time + fireRate;
                    }

                }

                bulletSwitch(weapon.spawnedPrefab1, weapon.spawnedPrefab2, weapon.isWeaponSwitchBullet);
            }
            if (selectedItem.itemType == InventoryObjects.ItemTypes.Accesory)
            {
                
                acs = (InventoryAccessory) selectedItem;
                setAcsPrefab(acs.spawnPrefab);

                if (isAcsSpawned == false)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        Instantiate(acs.spawnPrefab, GameObject.FindGameObjectWithTag("SelectedItem").transform.position, Quaternion.identity);
                        isAcsSpawned = true;
                    }
                    
                }

            }

            

        }
        else
        {
            if (SelectedRenderer.sprite) SelectedRenderer.sprite = null;
        }

        if (bulletInstance) Destroy(bulletInstance, weapon.destroyAfterTime);


        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        Vector2 objectPos = GameObject.FindGameObjectWithTag("SelectedItem").transform.position;
        target.x = target.x - objectPos.x;
        target.y = target.y - objectPos.y;

        float Zangle = Mathf.Atan2(target.x, target.y) * Mathf.Rad2Deg;

        if(transform.rotation.y >= 0)
        {
            Zfactor = -1;
            GameObject.FindGameObjectWithTag("SelectedItem").GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            Zfactor = 1;
            GameObject.FindGameObjectWithTag("SelectedItem").GetComponent<SpriteRenderer>().flipX = false;
        }
        GameObject.FindGameObjectWithTag("SelectedItem").transform.rotation = Quaternion.Euler(new Vector3(0, 0, -Zangle - (90*Zfactor)));
        
        
    }

    public void setAcsPrefab(GameObject prefab)
    {
        if (!acsSpawnedTargetPrefab) acsSpawnedTargetPrefab = prefab;
    }

    public void bulletSwitch(GameObject a, GameObject b, bool isSwitch)
    {
        if (isSwitch)
        {
            if (!weaponSpawnedTargetPrefab) weaponSpawnedTargetPrefab = a;
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (weaponSpawnedTargetPrefab == a)
                {
                    weaponSpawnedTargetPrefab = b;
                }
                else if (weaponSpawnedTargetPrefab == b)
                {
                    weaponSpawnedTargetPrefab = a;
                }
                else
                {
                    Debug.Log("Something Went Wrong");
                }
            }
        }
        else
        {
            if (!weaponSpawnedTargetPrefab) weaponSpawnedTargetPrefab = a;
        }

        

    }

    

}
