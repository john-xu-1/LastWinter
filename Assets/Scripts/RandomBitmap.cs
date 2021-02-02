using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBitmap : MonoBehaviour
{
    public string myString;
    string glyphs = "10";
    
    private void Awake()
    {
        int XcharAmount = Random.Range(15, 17);
        int YcharAmount = Random.Range(8, 9);

        for (int i = 0; i < YcharAmount; i++)
        {
            for (int j = 0; j < XcharAmount; j++)
            {
                myString += glyphs[Random.Range(0, glyphs.Length)];

            }
            
            myString += "@";
            myString = myString.Replace("@", System.Environment.NewLine);
            Debug.Log(myString[1]);
            

        }
            
        
        
    }
    


}
    
    
