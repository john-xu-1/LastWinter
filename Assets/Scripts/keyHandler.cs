using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyHandler : MonoBehaviour
{
    public bool hasDoorKey, hasWaterKey, hasLavaKey, hasEnemyKey;

    public void KeyFound(WorldBuilder.GateTypes keyTypes)
    {
        if (keyTypes == WorldBuilder.GateTypes.door) hasDoorKey = true;
        if (keyTypes == WorldBuilder.GateTypes.enemy) hasEnemyKey = true;
        if (keyTypes == WorldBuilder.GateTypes.water) hasWaterKey = true;
        if (keyTypes == WorldBuilder.GateTypes.lava) hasLavaKey = true;
    }
}
