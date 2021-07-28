using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleWire01 : PuzzleBase
{
    public List<string> ordah;
    public string[] ordahAr;
    public int correct;
    public override void checkIsSolved()
    {
        
        if(ordah.Contains("green") && ordah.Contains("blue") && ordah.Contains("red") && ordah.Count >= 6)
        {
            ordahAr = ordah.ToArray();
            for (int i = 0; i < ordahAr.Length; i += 2)
            {
                Debug.Log(i);
                Debug.Log(i + 1);
                if ((ordahAr[i] == "red" && ordahAr[i + 1] == "red") || (ordahAr[i] == "blue" && ordahAr[i + 1] == "blue") || (ordahAr[i] == "green" && ordahAr[i + 1] == "green"))
                {
                    correct += 1;

                }

            }

            for (int i = 0; i < ordah.Count; i += 1)
            {
                ordah[i] = null;
                
            }


        }



        if (correct >= 3)
        {
            isUnlocked = true;
        }
        else
        {
            correct = 0;
        }
    }

    public void wireLinkL(string type)
    {
        ordah.Add(type);
        
    }

    public void wireLinkR(string type)
    {
        ordah.Add(type);
        
    }
}
