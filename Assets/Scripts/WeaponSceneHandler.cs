using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponSceneHandler : GameHandler
{
    public int lives = 1;
    public override void EnemyDied(GameObject enemy)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayerDied(GameObject player)
    {
        //FindObjectOfType<InventorySystem>().SaveInventory();
        Destroy(player);

        lives -= 1;

        if (lives > 0)
        {
            GameObject instance = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);

            ReAssignPlayer(instance);
        }
        else
        {
            FindObjectOfType<InventorySystem>().SaveInventory();
            SceneManager.LoadScene(1);
        }


       
    }

    public override void ReAssignPlayer(GameObject player)
    {

        EnemyBehaviorBase[] enemies = FindObjectsOfType<EnemyBehaviorBase>();

        foreach (EnemyBehaviorBase enemy in enemies)
        {
            if (enemy && player) enemy.p = player;
        }


        
        //MainCamera.GetComponent<CameraController>().target = player.transform;


    }

    public override void StartGameHandler(GameObject player)
    {
        playerSpawnPoint = player.transform.position;
    }
}
