using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;

    public SpriteRenderer melSr;




    void Start()
    {
        
    }

    
    void Update()
    {
        
    }


    public void melee()
    {
        anim.SetTrigger("isMelee");
    }

    public void meleeHeavy()
    {
        anim.SetTrigger("isMeleeHeavy");
    }

    public void setMelSr(Sprite sprite)
    {
        melSr.sprite = sprite;
    }

    
}
