using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerHealthHandler : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text healthText;
    virtual public void Display(float playerHealth)
    {
        healthText.text = playerHealth.ToString();
    }
}
