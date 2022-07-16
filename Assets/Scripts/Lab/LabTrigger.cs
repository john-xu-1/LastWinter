using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LabTrigger : MonoBehaviour
{
    public GameObject triggeredCanvas;
    public abstract bool Valid();
    public abstract void Trigger();
    private bool visible;

    int counter = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            counter += 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            counter -= 1;
        }
    }

    private void Start()
    {
        triggeredCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!visible && counter > 0 && Valid())
        {
            visible = true;
            triggeredCanvas.SetActive(true);
            
        }
        else if (visible && counter <= 0 || !Valid())
        {
            visible = false;
            triggeredCanvas.SetActive(false);
        }

        if (visible)
        {
            Trigger();
        }
    }
}