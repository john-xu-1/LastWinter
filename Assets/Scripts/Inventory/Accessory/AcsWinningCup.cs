using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcsWinningCup : AcsBase
{
    public override void setUp()
    {
        PlatformerController pc = player.GetComponent<PlatformerController>();

        pc.MaxSpeedNormal += magnitude;
        pc.MaxSpeedWater += magnitude;
        pc.MaxSpeedLava += magnitude;
    }

    public override void unDo()
    {
        PlatformerController pc = player.GetComponent<PlatformerController>();

        pc.MaxSpeedNormal -= magnitude;
        pc.MaxSpeedWater -= magnitude;
        pc.MaxSpeedLava -= magnitude;
    }
}
