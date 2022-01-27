using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPlant : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<Animator>().SetBool("Lit", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<Animator>().SetBool("Lit", false);
    }
}
