using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcsBase : MonoBehaviour
{
    public InventoryAccessory acs;
    public float magnitude;

    public GameObject player;
    public InventorySystem inv;

    public bool isContainItem;

    

    private void Start()
    {
        acs = (InventoryAccessory)FindObjectOfType<InventorySystem>().selectedItem;
        player = GameObject.FindGameObjectWithTag("Player");
        inv = player.GetComponent<InventorySystem>(); 
        setUp();

        
    }


    private void Update()
    {

        updateBehavior();

        if (isContainItem == false)
        {
            foreach (InventoryObjects item in inv.items)
            {
                if (item == acs)
                {
                    isContainItem = true;
                }
                else
                {
                    isContainItem = false;
                }
            }
        }
        

    }

    private void FixedUpdate()
    {
        fixedUpdateBehavior();
    }

    public virtual void updateBehavior()
    {

    }

    public virtual void setUp()
    {

    }

    public virtual void fixedUpdateBehavior()
    {

    }
}
