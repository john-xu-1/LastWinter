using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvailableMenu : MonoBehaviour
{

    public InventorySystem invSys;

    public GameObject C, W, F, Aiming;
    public Sprite COff, WOff, FOff, AimingOff;
    public Sprite COn, WOn, FOn, AimingOn;


   


    void Update()
    {
        if (invSys)
        {
            InventoryWeapon wep = (InventoryWeapon)invSys.selectedItem;
            if (wep.weaponType == InventoryWeapon.WeaponTypes.Magnetized_Shifter)
            {
                C.GetComponent<Image>().sprite = COn;
            }
            else
            {
                C.GetComponent<Image>().sprite = COff;
            }
        }
        else
        {
            invSys = GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySystem>();
        }
        
    }
}
