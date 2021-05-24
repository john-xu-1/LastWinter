using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int[,] test1;
    // Start is called before the first frame update
    void Start()
    {
        test1 = new int[2, 2];
        test1[0, 0] = 1;
        test1[1, 1] = 2;
        print(test1[0, 0] + " " + test1[1, 0] + " "+ test1[0, 1] + " "+ test1[1, 1] + " ");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
