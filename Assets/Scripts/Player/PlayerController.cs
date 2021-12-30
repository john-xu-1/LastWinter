using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class PlayerController
{
    public static bool canMelee = true;
    public static bool canRange = true;
    public static bool canJump = true;
    public static bool canMove = true;
    public static bool canOpenMenu = true;
    public static bool canExitScene = true;
    public static bool canGameUI = true;

    public static void SetAll(bool canMelee, bool canRange, bool canJump, bool canMove, bool canOpenMenu, bool canExitScene, bool canGameUI)
    {
        PlayerController.canMelee = canMelee;
        PlayerController.canRange = canRange;
        PlayerController.canJump = canJump;
        PlayerController.canMove = canMove;
        PlayerController.canOpenMenu = canOpenMenu;
        PlayerController.canExitScene = canExitScene;
        PlayerController.canGameUI = canGameUI;
    }

}
