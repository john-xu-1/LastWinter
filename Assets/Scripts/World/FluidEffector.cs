using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FluidEffector : MonoBehaviour
{

    public enum FluidTypes
    {
        Water,
        Lava
    }
    public FluidTypes FluidType;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(FluidType == FluidTypes.Lava)
            {
                TileBase tileBase = UtilityTilemap.GetTile(GetComponent<Tilemap>(), collision.transform.position);

                if(tileBase == null)
                {
                    collision.GetComponent<PlatformerController>().EffectorState = PlatformerController.EffectorStates.None;
                }
                else if(tileBase.name == "LW_LavaTile")
                {
                    collision.GetComponent<PlatformerController>().EffectorState = PlatformerController.EffectorStates.LavaPartial;
                }
                else
                {
                    collision.GetComponent<PlatformerController>().EffectorState = PlatformerController.EffectorStates.Lava;
                }
            }else if(FluidType == FluidTypes.Water)
            {
                TileBase tileBase = UtilityTilemap.GetTile(GetComponent<Tilemap>(), collision.transform.position);
                if (tileBase == null)
                {
                    collision.GetComponent<PlatformerController>().EffectorState = PlatformerController.EffectorStates.None;
                }
                else if (tileBase.name == "LW_WaterTile")
                {
                    collision.GetComponent<PlatformerController>().EffectorState = PlatformerController.EffectorStates.WaterPartial;
                }
                else
                {
                    collision.GetComponent<PlatformerController>().EffectorState = PlatformerController.EffectorStates.Water;
                }
            }
            else
            {
                Debug.LogWarning("FluidType not handled: " + FluidType);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlatformerController>().EffectorState = PlatformerController.EffectorStates.None;
        }
    }
}
