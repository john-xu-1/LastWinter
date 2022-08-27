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

    public GameObject weaponSpawnedTargetPrefab;

    [SerializeField] private DoDamage dd;

    [SerializeField] private float curDmg;

    public float disappearCD = 2;
    private float nextDisappearTime;

    public void setCurDmg(float d)
    {
        curDmg = d;
    }


    void Start()
    {
        inventorySystem = GetComponent<InventorySystem>();


        nextDisappearTime = disappearCD;
        GameObject.FindGameObjectWithTag("SelectedItem").GetComponent<SpriteRenderer>().sprite = null;
    }

    
    void Update()
    {

        if (weapon)
        {
            fireRate = weapon.coolDown;

            ability1();
            if (!weapon.isMelee)
            {
                fire1DownRange();
                fire2();
                fire2Up();
            }
            else
            {

                fire1DownMelee();
                fire2Down();
            }


            //to eject chip outside when holding weapon
            //if (weapon.chip)
            //{
            //    if (Input.GetKeyDown(KeyCode.E))
            //    {
            //        if (inventorySystem.isInvFull == false)
            //        {
            //            inventorySystem.AddItem(weapon.chip);
            //        }
            //        else
            //        {
            //            GameObject instance = Instantiate(inventorySystem.emptyItem, new Vector2(transform.position.x + inventorySystem.emptySpawnOffset.x, transform.position.y + inventorySystem.emptySpawnOffset.y), Quaternion.identity);
            //            instance.GetComponent<Pickupable>().item = weapon.chip;
            //            instance.GetComponent<SpriteRenderer>().sprite = weapon.chip.itemSprite;
            //        }


            //        weapon.chip = null;
            //    }

            //}


            bulletSwitch(weapon.spawnedPrefab1, weapon.spawnedPrefab2, weapon.isWeaponSwitchBullet);
        }

        if (bulletInstance) Destroy(bulletInstance, weapon.destroyAfterTime);

    }

    public void damageInflict()
    {
        dd.setDamage(curDmg);
        dd.DamageInflict();
    }

    private float Zfactor = 0;

    public void pivotFire()
    {
        GameObject.FindGameObjectWithTag("SelectedItem").GetComponent<SpriteRenderer>().sprite = weapon.itemSprite;

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

        Debug.Log("pivotting");


        GameObject.FindGameObjectWithTag("SelectedItem").transform.rotation = Quaternion.Euler(new Vector3(0, 0, -Zangle - (90 * Zfactor)));


    }

    public void offFire()
    {
        if (GameObject.FindGameObjectWithTag("SelectedItem").GetComponent<SpriteRenderer>().sprite != null)
        {
            GameObject.FindGameObjectWithTag("SelectedItem").GetComponent<SpriteRenderer>().sprite = null;

            Debug.Log("offing");
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

    private void ability1()
    {
        if (PlayerController.canMelee && PlayerController.canRange && Input.GetKeyDown(KeyCode.F))
        {
            if (Time.time >= weapon.ab1.curAbility1Time)
            {
                weapon.ab1.action(anim);

                anim.SetTrigger("isAbility1");

                weapon.ab1.curAbility1Time = Time.time + weapon.ab1.coolDown;

            }
        }
    }

    private void fire1DownMelee()
    {
        if (PlayerController.canMelee && Input.GetButtonDown("Fire1"))
        {
            if (Time.time >= weapon.curAttackTime)
            {
                melee();

                //pivotFire();

                weapon.curAttackTime = Time.time + fireRate;

            }
        }

        //offFire();

    }

    private void fire1DownRange()
    {

        if (PlayerController.canMelee && Input.GetButtonDown("Fire1"))
        {
            if (Time.time >= weapon.curAttackTime)
            {
                nextDisappearTime = disappearCD;

                bulletInstance = Instantiate(weaponSpawnedTargetPrefab, GameObject.FindGameObjectWithTag("SelectedItem").transform.position, GameObject.FindGameObjectWithTag("SelectedItem").transform.rotation);

                pivotFire();


                bulletInstance.GetComponent<DoDamage>().setDamage(curDmg);

                Invoke("offFire", 2);

                weapon.curAttackTime = Time.time + fireRate;

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
                if (Time.time >= weapon.curAttackTime)
                {
                    meleeHeavy();

                    weapon.curAttackTime = Time.time + fireRate;

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
