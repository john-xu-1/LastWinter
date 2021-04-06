using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSpawn : MonoBehaviour
{
    public GameObject puz;
    public GameObject canv;
    bool isGoOnce;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (puz && isGoOnce == false)
        {
            Instantiate(puz, canv.transform);
            isGoOnce = true;
        }
        
    }


}
