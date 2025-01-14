﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public List<InventoryObjects> items;
    public int index;
    public InventoryObjects selectedItem;
    public SpriteRenderer SelectedRenderer;

    public InventoryWeapon iw;
    private float fireRate;
    public float[] nextAttackTimes;

    public InventoryWeapon weapon;
    public InventoryAccessory acs;
    public InventoryChip chip;

    GameObject bulletInstance;

    GameObject weaponSpawnedTargetPrefab;
    GameObject acsSpawnedTargetPrefab;

    float Zfactor;

    //bool isAcsSpawned;

    public int maxItemSize;

    public bool isInvFull;

    public GameObject emptyItem;

    public Vector2 emptySpawnOffset;

    public GameObject wepChipPanel;
    private GameObject wepChipPanelChild;

    private InventoryWeapon selectedWeapon;

    private bool acsSpawned;

    private int curSelectedKey = 0;

    List<InventoryWeapon> wep;
    Dropdown dpd;


    public EffectBase eb;

    

    private void Start()
    {
        wepChipPanelChild = wepChipPanel.transform.GetChild(0).gameObject;
        wepChipPanel.SetActive(false);
        FindObjectOfType<InventoryUI>().updateSprite(items);
    }

    public void AddItem(InventoryObjects item)
    {
        bool isFullReplace = false;
        if (items.Count <= maxItemSize)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    isFullReplace = true;
                    items[i] = item;
                    break;
                }

            }

            if (isFullReplace == false)
            {
                items.Add(item);
            }

            index += 1;

        }
        else
        {
            Debug.Log("INVENTORY FULL");

        }

        FindObjectOfType<InventoryUI>().updateSprite(items);

    }

    public void removeItem(InventoryObjects item)
    {

        //items.RemoveAt(items.IndexOf(item));


        items[items.IndexOf(item)] = null;

        if (item == selectedItem)
        {
            selectedItem = null;
        }

        FindObjectOfType<InventoryUI>().updateSprite(items);

        index--;
    }

    
    private void Update()
    {
        
        int count = 0;

        
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                isInvFull = false;
                break;
            }
            else
            {
                count++;
            }
        }

        if (count == maxItemSize)
        {
            isInvFull = true;
        }
        



        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (items.Count > 0)
            {
                selectedItem = items[0];
            }
            else
            {
                selectedItem = null;
            }

            
            if (curSelectedKey != 1)
            {
                acsSpawned = false;
                curSelectedKey = 1;
            }
            
            
            weaponSpawnedTargetPrefab = null;
            acsSpawnedTargetPrefab = null;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (items.Count > 1)
            {
                selectedItem = items[1];
            }
            else
            {
                selectedItem = null;
            }

            if (curSelectedKey != 2)
            {
                acsSpawned = false;
                curSelectedKey = 2;
            }

            weaponSpawnedTargetPrefab = null;
            acsSpawnedTargetPrefab = null;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (items.Count > 2)
            {
                selectedItem = items[2];
            }
            else
            {
                selectedItem = null;
            }

            if (curSelectedKey != 3)
            {
                acsSpawned = false;
                curSelectedKey = 3;
            }

            weaponSpawnedTargetPrefab = null;
            acsSpawnedTargetPrefab = null;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (items.Count > 3)
            {
                selectedItem = items[3];
            }
            else
            {
                selectedItem = null;
            }

            if (curSelectedKey != 4)
            {
                acsSpawned = false;
                curSelectedKey = 4;
            }

            weaponSpawnedTargetPrefab = null;
            acsSpawnedTargetPrefab = null;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (items.Count > 4)
            {
                selectedItem = items[4];
            }
            else
            {
                selectedItem = null;
            }

            if (curSelectedKey != 5)
            {
                acsSpawned = false;
                curSelectedKey = 5;
            }

            weaponSpawnedTargetPrefab = null;
            acsSpawnedTargetPrefab = null;
        }

        

        if (selectedItem != null)
        {
            SelectedRenderer.sprite = selectedItem.itemSprite;

            if (selectedItem.itemType == InventoryObjects.ItemTypes.Weapon)
            {
                weapon = (InventoryWeapon)selectedItem;
                fireRate = weapon.coolDown;

                if (Input.GetButton("Fire1"))
                {
                    if (Time.time >= nextAttackTimes[weapon.CDIndex])
                    {
                        bulletInstance = Instantiate(weaponSpawnedTargetPrefab, GameObject.FindGameObjectWithTag("SelectedItem").transform.position, GameObject.FindGameObjectWithTag("SelectedItem").transform.rotation);
                        nextAttackTimes[weapon.CDIndex] = Time.time + fireRate;
                    }

                }

                //to eject chip outside when holding weapon
                if (weapon.chip)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (isInvFull == false)
                        {
                            AddItem(weapon.chip);
                        }
                        else
                        {
                            GameObject instance = Instantiate(emptyItem, new Vector2(transform.position.x + emptySpawnOffset.x, transform.position.y + emptySpawnOffset.y), Quaternion.identity);
                            instance.GetComponent<Pickupable>().item = weapon.chip;
                            instance.GetComponent<SpriteRenderer>().sprite = weapon.chip.itemSprite;
                        }


                        weapon.chip = null;
                    }

                }
                

                bulletSwitch(weapon.spawnedPrefab1, weapon.spawnedPrefab2, weapon.isWeaponSwitchBullet);
            }
            if (selectedItem.itemType == InventoryObjects.ItemTypes.Accesory)
            {

                
                acs = (InventoryAccessory)selectedItem;
                
                
                setAcsPrefab(acs.spawnPrefab);


                if (acsSpawned == false)
                {
                    eb = Instantiate(acs.spawnPrefab, GameObject.FindGameObjectWithTag("SelectedItem").transform.position, Quaternion.identity).GetComponent<EffectBase>();
                    FindObjectOfType<Effectable>().addEffect(eb.GetComponent<EffectBase>());
                    
                    acsSpawned = true;
                }
                
                

            }
            /*
            else
            {


                if (curAcsTime <= 0)
                {
                    if (isAcsUndo == false)
                    {
                        for(int i = 0; i < curEffectorInstances.Count; i++)
                        {
                            if (curEffectorInstances[i])
                            {
                                
                                curEffectorInstances[i].GetComponent<AcsBase>().unDo();
                                Destroy(curEffectorInstances[i]);
                                

                                isAcsUndo = true;
                            }
                            
                        }
                    }

                }
                else
                {
                    isAcsUndo = false;
                    curAcsTime -= Time.deltaTime;

                }
            }
            */

            if (selectedItem.itemType == InventoryObjects.ItemTypes.Chip)
            {

                chip = (InventoryChip)selectedItem;


                if (Input.GetKeyDown(KeyCode.E))
                {
                    //isWepChipActive = !isWepChipActive;
                    wepChipPanel.SetActive(true);
                    dpd = wepChipPanelChild.transform.GetChild(0).GetComponent<Dropdown>();

                    wepChipPanelChild.transform.GetChild(1).GetComponent<Image>().sprite = chip.itemSprite;

                    List<string> wepNames = new List<string>();
                    dpd.ClearOptions();
                    dpd.AddOptions(new List<string>() { "None" });

                    wep = new List<InventoryWeapon>();

                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i])
                        {
                            if (items[i].itemType == InventoryObjects.ItemTypes.Weapon)
                            {

                                wepNames.Add(items[i].itemName);

                                wep.Add((InventoryWeapon)items[i]);



                            }
                        }
                        
                    }

                    /*
                    if (dpd.options.Count > 1)
                    {
                        for (int i = 0; i < wepNames.Count; i++)
                        {
                            if (i > 0)
                            {
                                if (dpd.options[i].text != wepNames[i - 1] || i < dpd.options.Count)
                                {
                                    FindObjectOfType<InventoryUI>().setDropdown(dpd, wepNames);
                                    break;
                                }
                                
                            }
                        }
                    }
                    else
                    {
                        FindObjectOfType<InventoryUI>().setDropdown(dpd, wepNames);
                    }
                    */
                    FindObjectOfType<InventoryUI>().setDropdown(dpd, wepNames);







                }

            }



        }
        else
        {
            SelectedRenderer.sprite = null;
        }

        if (bulletInstance) Destroy(bulletInstance, weapon.destroyAfterTime);


        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        Vector2 objectPos = GameObject.FindGameObjectWithTag("SelectedItem").transform.position;
        target.x = target.x - objectPos.x;
        target.y = target.y - objectPos.y;

        float Zangle = Mathf.Atan2(target.x, target.y) * Mathf.Rad2Deg;

        if (transform.rotation.y >= 0)
        {
            Zfactor = -1;
            GameObject.FindGameObjectWithTag("SelectedItem").GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            Zfactor = 1;
            GameObject.FindGameObjectWithTag("SelectedItem").GetComponent<SpriteRenderer>().flipX = false;
        }
        GameObject.FindGameObjectWithTag("SelectedItem").transform.rotation = Quaternion.Euler(new Vector3(0, 0, -Zangle - (90 * Zfactor)));





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
            if (Input.GetKeyDown(KeyCode.C))
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

    public void setChip()
    {
        Debug.Log(selectedWeapon);

        if (dpd.value > 0)
        {
            selectedWeapon = wep[dpd.value - 1];
        }
        else
        {
            selectedWeapon = null;
        }

        if (selectedWeapon)
        {
            if (selectedWeapon.chip == null)
            {
                selectedWeapon.chip = chip;

                removeItem(selectedWeapon.chip);

                Debug.Log("set");
            }
            else
            {
                //returns original chip back to inv

                if (isInvFull == false)
                {
                    AddItem(selectedWeapon.chip);
                }
                else
                {
                    GameObject instance = Instantiate(emptyItem, new Vector2(transform.position.x + emptySpawnOffset.x, transform.position.y + emptySpawnOffset.y), Quaternion.identity);
                    instance.GetComponent<Pickupable>().item = selectedWeapon.chip;
                    instance.GetComponent<SpriteRenderer>().sprite = selectedWeapon.chip.itemSprite;
                }


                //sets original chip to current chip
                Debug.Log("switch");
                selectedWeapon.chip = chip;

                removeItem(chip);




            }
        }


        wepChipPanel.SetActive(false);
        
    }



}
