using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSceneHandler : GameHandler
{
    public override void EnemyDied(GameObject enemy)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayerDied(GameObject player)
    {
        //FindObjectOfType<InventorySystem>().SaveInventory();
        Destroy(player);


        GameObject instance = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);

        ReAssignPlayer(instance);
    }

    public override void ReAssignPlayer(GameObject player)
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy && player) enemy.GetComponent<EnemyBehaviorBase>().p = player;
        }


        
        //MainCamera.GetComponent<CameraController>().target = player.transform;


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
