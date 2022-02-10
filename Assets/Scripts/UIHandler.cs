using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] float playerHealth;
    public float PlayerHealth { set { playerHealth = value; } get { return playerHealth; } }
    [SerializeField] UIPlayerHealthHandler playerHealthHandler;

    [SerializeField] List<InventoryObjects> inventory;

    public void DisplayPlayerHealth()
    {
        playerHealthHandler.Display(playerHealth);
    }

    public void setOrgHealth(float maxHealth)
    {
        playerHealthHandler.setMaxDisplay(maxHealth);
        playerHealthHandler.Display(playerHealth);
    }
}
