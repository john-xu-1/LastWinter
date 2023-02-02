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

    public static bool canDash = true;

    public static bool canDoubleJump = true;

    public static bool canWallClimb = true;

    public static bool canSurviveWater = true;

    public static bool canSurviveLava = true;

    public static void SetAll (bool canMelee, bool canRange, bool canJump, bool canMove, bool canOpenMenu, bool canExitScene, bool canGameUI, bool canDoubleJump, bool canWallClimb, bool canSurviveWater, bool canSurviveLava, bool canDash)
    {
        PlayerController.canMelee = canMelee;
        PlayerController.canRange = canRange;
        PlayerController.canJump = canJump;
        PlayerController.canMove = canMove;
        PlayerController.canOpenMenu = canOpenMenu;
        PlayerController.canExitScene = canExitScene;
        PlayerController.canGameUI = canGameUI;
        PlayerController.canDoubleJump = canDoubleJump;
        PlayerController.canWallClimb = canWallClimb;
        PlayerController.canSurviveWater = canSurviveWater;
        PlayerController.canSurviveLava = canSurviveLava;
        PlayerController.canDash = canDash;
    }
}
