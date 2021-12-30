using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;

    public SpriteRenderer melSr;

    InventorySystem inventorySystem;
    public InventoryWeapon weapon;
    private float fireRate;

    

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
            fireRate = weapon.coolDown;

            fire1Down();

            if (!weapon.isMelee)
            {
                fire2();
                fire2Up();
            }
            else
            {
                fire2Down();
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

        if (bulletInstance) Destroy(bulletInstance, weapon.destroyAfterTime);

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

    private void fire1Down()
    {
        if (PlayerController.canMelee && Input.GetButtonDown("Fire1"))
        {
            if (Time.time >= inventorySystem.nextAttackTimes[weapon.CDIndex])
            {
                if (!weapon.isMelee)
                {
                    bulletInstance = Instantiate(weaponSpawnedTargetPrefab, GameObject.FindGameObjectWithTag("SelectedItem").transform.position, GameObject.FindGameObjectWithTag("SelectedItem").transform.rotation);
                }
                else
                {
                    melee();
                }

            }
        }
        
    }

    private void fire2()
    {
        
        if (PlayerController.canRange && Input.GetButton("Fire2"))
        {
            //if (damage < cap) damage += Time.deltaTime;
            Debug.Log("Charge Holding");
        }
        
    }

    private void fire2Down()
    {
        if (PlayerController.canMelee && Input.GetButtonDown("Fire2"))
        {
            if (weapon.isMelee)
            {
                if (Time.time >= inventorySystem.nextAttackTimes[weapon.CDIndex])
                {
                    meleeHeavy();

                    inventorySystem.nextAttackTimes[weapon.CDIndex] = Time.time + fireRate;

                }
            }

        }
        
    }

    private void fire2Up()
    {
        if (PlayerController.canRange && Input.GetButtonUp("Fire2"))
        {
            bulletInstance = Instantiate(weaponSpawnedTargetPrefab, GameObject.FindGameObjectWithTag("SelectedItem").transform.position, GameObject.FindGameObjectWithTag("SelectedItem").transform.rotation);
            Debug.Log("Charge Release");
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
