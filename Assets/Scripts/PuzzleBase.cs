using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleBase : MonoBehaviour
{
    public bool isUnlocked;

    
    
    public virtual void solveAction()
    {
        Debug.Log("puzzle solved");
        gameObject.SetActive(false);
    }

    public virtual void checkIsSolved()
    {
        
    }

    private void Update()
    {
        if (isUnlocked)
        {
            solveAction();
        }
        
    }



}
