using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBitmap : MonoBehaviour
{
    public string myString;
    string glyphs = "210";
    
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

            for (int k = 0; k < myString.Length; k++)
            {

                if (myString[k] == '2')
                {
                    Debug.Log("working1");
                    if (myString[k + 1] <= myString[myString.Length - 1]) 
                    {
                        if (myString[k + 1] == '1')
                        {
                            Debug.Log("working2");
                            myString.Remove(k + 1);
                            myString.Insert(k + 1, "0");
                        }

                    }
                    
                }
                else
                {
                    Debug.Log("not a 2");
                }
                //Debug.Log(myString);
            }

            myString += "@";
            myString = myString.Replace("@", System.Environment.NewLine);
            Debug.Log(myString[1]);
            

        }
            
        
        
    }
    


}
    
    
