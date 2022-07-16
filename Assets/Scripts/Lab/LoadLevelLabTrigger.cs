using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelLabTrigger : LabTrigger
{
    public string loadLevelScene = "EnemyTestScene";
    public override void Trigger()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(loadLevelScene);
        }
    }

    public override bool Valid()
    {
        //what needs to be completed for exiting
        return true;
    }

}
