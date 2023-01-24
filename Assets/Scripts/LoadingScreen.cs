using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Text counterText;
    private float counterStartTime;
    public bool active = false;
    
    public void StartCounter()
    {
        active = true;
        counterStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (active) counterText.text = ((int)(Time.time - counterStartTime )).ToString();
    }
}
