using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnections : MonoBehaviour
{
    public bool upEgress, upIngress, rightEgress, rightIngress, downEgress, downIngress, leftEgress, leftIngress;

    public override string ToString()
    {
        return $"[{toString(upEgress)}, {toString(upIngress)}, {toString(rightEgress)}, {toString(rightIngress)}, {toString(downEgress)}, {toString(downIngress)}, {toString(leftEgress)}, {toString(leftIngress)}]";

    }
    private int toString(bool value)
    {
        return value ? 1 : 0;
    }

    public static RoomConnections[] GetRoomConnectionsArray(int size)
    {
        RoomConnections[] connections = new RoomConnections[size];
        for(int i = 0; i < size; i += 1)
        {
            connections[i] = new RoomConnections();
        }
        return connections;
    }
}
