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

        Instantiate(this.playerPrefab, playerSpawnPoint, Quaternion.identity);
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
