using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;

    public SpriteRenderer melSr;

    public InventoryWeapon weapon;
    private float fireRate;

    InventorySystem inventorySystem;
    GameObject bulletInstance;
    GameObject weaponSpawnedTargetPrefab;

    void Start()
    {
        inventorySystem = GetComponent<InventorySystem>();
    }

    
    void Update()
    {
        if (weapon)
        {

            if (Input.GetButtonDown("Fire1"))
            {
                Fire1Down();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                Fire2Down();
            }
            if (Input.GetButton("Fire2"))
            {
                Fire2();
            }
            if (Input.GetButtonUp("Fire2"))
            {
                Fire2Up();
            }


            //to eject chip outside when holding weapon
            if (weapon.chip)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (inventorySystem.isInvFull == false)
                    {
                        inventorySystem.AddItem(weapon.chip);
                    }
                    else
                    {
                        GameObject instance = Instantiate(inventorySystem.emptyItem, new Vector2(transform.position.x + inventorySystem.emptySpawnOffset.x, transform.position.y + inventorySystem.emptySpawnOffset.y), Quaternion.identity);
                        instance.GetComponent<Pickupable>().item = weapon.chip;
                        instance.GetComponent<SpriteRenderer>().sprite = weapon.chip.itemSprite;
                    }


                    weapon.chip = null;
                }

            }


            bulletSwitch(weapon.spawnedPrefab1, weapon.spawnedPrefab2, weapon.isWeaponSwitchBullet);
        }

        
    }


    public void melee()
    {
        anim.SetTrigger("isMelee");
    }

    public void meleeHeavy()
    {
        anim.SetTrigger("isMeleeHeavy");
    }

    public void setMelSr(Sprite sprite)
    {
        melSr.sprite = sprite;
    }

    private void Fire1Down()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time >= inventorySystem.nextAttackTimes[weapon.CDIndex])
            {

                if (PlayerController.canRange && !weapon.isMelee)
                {
                    bulletInstance = Instantiate(weaponSpawnedTargetPrefab, GameObject.FindGameObjectWithTag("SelectedItem").transform.position, GameObject.FindGameObjectWithTag("SelectedItem").transform.rotation);
                }
                else if(PlayerController.canMove)
                {
                    melee();
                }


            }

        }
    }

    private void Fire2Down()
    {
        if (Time.time >= inventorySystem.nextAttackTimes[weapon.CDIndex])
        {

            if (weapon.isMelee)
            {
                meleeHeavy();
            }


            inventorySystem.nextAttackTimes[weapon.CDIndex] = Time.time + fireRate;

        }
    }

    private void Fire2()
    {
        if (!weapon.isMelee)
        {
            if (Input.GetButton("Fire2"))
            {
                //if (damage < cap) damage += Time.deltaTime;
                Debug.Log("Charge Holding");
            }
            
        }
    }

    private void Fire2Up()
    {
        if (!weapon.isMelee)
        {
            if (Input.GetButtonUp("Fire2"))
            {
                bulletInstance = Instantiate(weaponSpawnedTargetPrefab, GameObject.FindGameObjectWithTag("SelectedItem").transform.position, GameObject.FindGameObjectWithTag("SelectedItem").transform.rotation);
                Debug.Log("Charge Release");
            }
        }
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

}
