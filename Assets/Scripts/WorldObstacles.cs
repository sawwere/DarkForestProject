using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldObstacles
{
    private static readonly WorldObstacles instance = new WorldObstacles();
    private static GameObject[] hidingSpots;

    static WorldObstacles()
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("Hide");
    }

    private WorldObstacles() { }

    public static WorldObstacles Instance
    {
        get { return instance; }
    }

    public GameObject[] GetHidingSpots()
    {
        return hidingSpots;
    }
}
