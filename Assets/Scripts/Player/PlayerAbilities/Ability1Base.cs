using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponAbilities", menuName = "CustomObject/WeaponAbilities")]
public class Ability1Base : ScriptableObject
{
    public float curAbility1Time;
    public float coolDown;
    public AnimatorOverrideController newAnim;

    public enum abilityTypes
    {
        StabbySword,
        Breaketh,
        Nefarious,
        Unruly_Rhythm
    }

    public virtual void action(Animator anim)
    {
        anim.runtimeAnimatorController = newAnim;
        Debug.Log("Switched");
    }


    private void OnEnable()
    {
        curAbility1Time = 0;
    }
}
