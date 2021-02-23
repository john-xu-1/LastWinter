using System.Collections;
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
    private float nextAttackTime;

    InventoryWeapon weapon;

    GameObject bulletInstance;
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
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedItem = items[1];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedItem = items[2];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedItem = items[3];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedItem = items[4];
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
                    if (Time.time >= nextAttackTime)
                    {
                        bulletInstance = Instantiate(weapon.spawnedPrefab, GameObject.FindGameObjectWithTag("SelectedItem").transform.position, GameObject.FindGameObjectWithTag("SelectedItem").transform.rotation);
                        nextAttackTime = Time.time + fireRate;
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

        float Zfactor;
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
        Debug.Log(Mathf.Abs(GameObject.FindGameObjectWithTag("SelectedItem").transform.rotation.y));
        
    }



}
